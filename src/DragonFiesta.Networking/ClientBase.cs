// modified ClientBase
// credits for the original to noodl.

using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Concurrent;
using DragonFiesta.Util;

namespace DragonFiesta.Networking 
{
	public class ClientBase : IDisposable
	{
		public Socket Socket { get; private set; }
		public IPAddress IP { get; private set; }
		public ushort Port { get; private set; }
		public ushort XorPosition;

		public bool Decrypt = true;

		public event EventHandler<PacketReceivedEventArgs> PacketReceived;

		public bool IsDisposed { get { return (IsDisposedInt > 0); } }
		private int IsDisposedInt;
		//receive
		private const int MaxReceiveBuffer = 16384;
		private byte[] ReceiveBuffer;
        private byte[] Remaining;
        private bool HeaderParsed;
        private byte HeaderLength;
        private ushort PacketLength;
        private byte[] PacketBuffer;
        private int PacketIndex;

		//send
		private ConcurrentQueue<byte[]> SendBuffer;
		private int IsSendingInt;

		public ClientBase(Socket Socket)
		{
			this.Socket = Socket;

			var addr = (IPEndPoint)Socket.RemoteEndPoint;
			IP = addr.Address;
			Port = (ushort)addr.Port;
            
			ReceiveBuffer = new byte[MaxReceiveBuffer];

			SendBuffer = new ConcurrentQueue<byte[]>();
		}
		public void Dispose()
		{
            if(this.IsDisposed)
                return;

		    if (Socket != null)
		    {
		        Socket.Close();
		        Socket.Dispose();
		    }
		    Socket = null;

		    IP = null;
			ReceiveBuffer = null;
		    SendBuffer = null;
			PacketReceived = null;
		    IsDisposedInt = 1;
		}
		~ClientBase()
		{
			this.Dispose();
		}

		public virtual void Start()
		{
			BeginReceive();
		}

		private void BeginReceive()
		{
			if (IsDisposed)
				return;

			try
			{
				var args = new SocketAsyncEventArgs();
				args.Completed += EndReceive;
				args.SetBuffer(ReceiveBuffer, 0, MaxReceiveBuffer);

				if (!Socket.ReceiveAsync(args))
					EndReceive(null, args);
			}
			catch (Exception)
			{
				Dispose();
			}
		}
		private void EndReceive(object sender, SocketAsyncEventArgs args)
		{
			if (IsDisposed)
				return;

			try
			{
                var transfered = args.BytesTransferred;

                if (transfered < 1)
                {
                    //socket error
                    Dispose();
                    return;
                }

                //get data to handle
                byte[] data;

                if (Remaining != null)
                {
                    data = new byte[(transfered + Remaining.Length)];

                    Buffer.BlockCopy(Remaining, 0, data, 0, Remaining.Length);
                    Buffer.BlockCopy(ReceiveBuffer, 0, data, Remaining.Length, transfered);


                    //reset remaining bytes
                    Remaining = null;
                }
                else
                {
                    data = new byte[transfered];

                    Buffer.BlockCopy(ReceiveBuffer, 0, data, 0, transfered);
                }


                //handle all bytes
                var dataIndex = 0;
                while (data.Length > dataIndex)
                {
                    //get left bytes
                    var bytesLeft = (data.Length - dataIndex);


                    //parse packet length
                    if (!HeaderParsed)
                    {
                        //at least 3 bytes are required for parsing
                        if (bytesLeft < 3)
                        {
                            if (bytesLeft > 0)
                            {
                                Remaining = new byte[bytesLeft];
                                Buffer.BlockCopy(data, dataIndex, Remaining, 0, bytesLeft);
                            }

                            return;
                        }


                        HeaderLength = 1;
                        PacketLength = data[dataIndex];

                        if (PacketLength == 0)
                        {
                            HeaderLength = 3;
                            PacketLength = BitConverter.ToUInt16(data, (dataIndex + 1));
                        }


                        //update indexes
                        dataIndex += HeaderLength;
                        bytesLeft -= HeaderLength;


                        //create packet buffer
                        PacketBuffer = new byte[PacketLength];
                        PacketIndex = 0;


                        //we got our len
                        HeaderParsed = true;


                        //check if there are any bytes left for handling
                        if (bytesLeft < 1)
                            break;
                    }
                    
                    //get missing bytes
                    var missing = (PacketLength - PacketIndex);

                    //get length of the bytes to copy
                    var copyLen = (bytesLeft <= missing ? bytesLeft : missing);

                    //copy next bytes to packet
                    Buffer.BlockCopy(data, dataIndex, PacketBuffer, PacketIndex, copyLen);
                 
                    //update indexes again
                    dataIndex += copyLen;
                    bytesLeft -= copyLen;
                    PacketIndex += copyLen;

                    //check if packet is finished
                    if (PacketIndex >= PacketLength)
                    {
                        //decrypt the shit
                        DecryptPacket(ref PacketBuffer);

                        //invoke ur event
                        if (PacketReceived != null)
                            PacketReceived(this, new PacketReceivedEventArgs(new Packet(PacketBuffer)));

                        //and reset this packet
                        HeaderParsed = false;
                    }
                }

                //clean up
                data = null;

                //all done =)
            }
			catch (Exception)
			{
				Dispose();
			}
			finally
			{
				args.Dispose();

				BeginReceive();
			}
		}
		private void BeginSend()
		{
			if (IsDisposed)
				return;

			try
			{
				byte[] buffer;
				if (SendBuffer.TryPeek(out buffer))
				{
					var args = new SocketAsyncEventArgs();
					args.Completed += EndSend;
					args.SetBuffer(buffer, 0, buffer.Length);


					if (!Socket.SendAsync(args))
						EndSend(null, args);
				}
				else
				{
					Interlocked.Exchange(ref IsSendingInt, 0);
				}
			}
			catch (Exception)
			{
				Dispose();
			}
		}
		private void EndSend(object sender, SocketAsyncEventArgs args)
		{
			if (IsDisposed)
				return;

			try
			{
				var transfered = args.BytesTransferred;

				if (transfered < 1)
				{
					Dispose();
					return;
				}


				byte[] buffer; // can be sent as state object to args
				if (SendBuffer.TryPeek(out buffer))
				{
					//check if all bytes were send
					if (buffer.Length == transfered)
						SendBuffer.TryDequeue(out buffer);
				}
			}
			catch (Exception)
			{
				Dispose();
			}
			finally
			{
				if (!IsDisposed)
				{
					if (SendBuffer != null &&
                        SendBuffer.Count > 0)
						BeginSend();
					else
						Interlocked.Exchange(ref IsSendingInt, 0);
				}
			}
		}

		public void Send(byte[] data)
		{
			if (IsDisposed)
				return;

            if (data.Length > ushort.MaxValue)
            {
                throw new OutOfMemoryException();
            }

			try
			{
				SendBuffer.Enqueue(data);

				if (Interlocked.CompareExchange(ref IsSendingInt, 1, 0) == 0)
				{
					BeginSend();
				}
			}
			catch (Exception)
			{
				Dispose();
			}
		}
		public void SendPacket(Packet pPacket)
		{
			Send(pPacket.ToArray());
		    // Socket.Send(pPacket.ToArray()); //sync test
		}
        
		protected void DecryptPacket(ref byte[] pPacketData)
		{
			if(Decrypt)
			    FiestaCryptoProvider.Crypt(ref pPacketData, ref XorPosition);
		}
	}
}