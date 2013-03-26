using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using DragonFiesta.Util;
using System.Threading.Tasks;

namespace DragonFiesta.Networking
{
	public class PacketProcessQueue
	{
		#region .ctor

		public PacketProcessQueue()
		{
			packetQueue = new ConcurrentQueue<Pair<ClientBase, Packet>>();
			blockingCollection = new BlockingCollection<Pair<ClientBase, Packet>>(packetQueue);
			consumer = blockingCollection.GetConsumingEnumerable();
			for(int i = 0; i < PacketThreads; i++)
			{
				Task.Factory.StartNew(Worker);
			}
		}
		~PacketProcessQueue()
		{
			blockingCollection.CompleteAdding();
			blockingCollection.Dispose();
		}

		#endregion
		#region Properties

		public static PacketProcessQueue Instance { get; private set; }

		public static int PacketThreads = 3;
		private readonly ConcurrentQueue<Pair<ClientBase, Packet>> packetQueue;
		private readonly BlockingCollection<Pair<ClientBase, Packet>> blockingCollection;
		private readonly IEnumerable<Pair<ClientBase, Packet>> consumer;

		#endregion
		#region Methods

		public static void Initialize()
		{
			Instance = new PacketProcessQueue();
		}

		public void AddToQueue(ClientBase pClient, Packet pPacket)
		{
			blockingCollection.Add(new Pair<ClientBase, Packet>(pClient, pPacket));
		}

		private void Worker()
		{
			foreach (var pair in consumer)
			{
				try
				{
					if (!HandlerStore.Instance.Handle(pair.First, pair.Second))
					{
						Logs.Network.WarnFormat("Unknown packet of header {0}; type {1} [{2}]", 
							pair.Second.Header, 
							pair.Second.Type, 
							pair.Second.OpCode.ToString("x4"));
						DumpPacket(pair.Second);
					}
				}
				catch (Exception e)
				{
					Logs.Network.Error("Could not handle packet", e);
					if (!ExceptionManager.Instance.HandleException(e))
						throw;
				}
			}
		}

		private void DumpPacket(Packet pPacket)
		{
			if (!Directory.Exists("packet_dumps"))
				Directory.CreateDirectory("packet_dumps");
			StringBuilder builder = new StringBuilder();
			builder
				.Append(@"packet_dumps\")
				.AppendFormat("{0}-{1} ", pPacket.Header, pPacket.Type)
				.Append(DateTime.Now.ToString("yyyy-MM-dd HH-mm"))
				.Append(".dmp");
			FileStream stream = File.Create(builder.ToString());
			byte[] packet = pPacket.ToArray();
			stream.Write(packet, 0, packet.Length);
			stream.Close();
			stream.Dispose();
			packet = null;
			pPacket.Dispose();
		}
		#endregion
		#region Events

		#endregion
	}
}