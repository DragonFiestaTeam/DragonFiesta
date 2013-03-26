using System;
using System.Net;
using System.Net.Sockets;
using log4net;

namespace DragonFiesta.Util
{
	public class ClientAcceptor
	{
		#region .ctor
		public ClientAcceptor(ushort port) : this("0.0.0.0", port)
		{ }
		public ClientAcceptor(string ip, ushort port)
		{
			try
			{
				_listener = new TcpListener(IPAddress.Parse(ip), port);
				_listener.Start();
			}
			catch (Exception e)
			{
				Log.Error("Could not open socket", e);
				throw;
			}
		}
		#endregion
		#region Methods
		public static bool Initialize(ushort port, string ip = "0.0.0.0")
		{
			try
			{
				Instance = new ClientAcceptor(ip, port);
			}
			catch (Exception e)
			{
				Log.Fatal("Could not start ClientAcceptor.");
				return false;
			}
			Log.Info("ClientAcceptor started successfully.");
			return true;
		}
		#endregion
		#region Properties
		public static ClientAcceptor Instance { get; set; }

		private TcpListener _listener;
		private static readonly ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		#endregion
		#region Events
		public EventHandler<NewClientEventArgs> NewClientAccepted;	
		#endregion
	}
	#region EventArgs
	public class NewClientEventArgs : EventArgs
	{
		#region .ctor
		public NewClientEventArgs(TcpClient client)
		{
			this.Client = client;
		}
		#endregion
		#region Properties
		public TcpClient Client { get; private set; }
		#endregion
	}
	#endregion
}
