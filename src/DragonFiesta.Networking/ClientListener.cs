using System;
using System.Net;
using System.Net.Sockets;
using DragonFiesta.Util;
using DragonFiesta.Configuration;

namespace DragonFiesta.Networking
{
	public class ClientListener
	{
		#region .ctor
		public ClientListener(int port) : this("0.0.0.0", port)
		{
		}
		public ClientListener(string ip, int port)
		{
			listener = new TcpListener(IPAddress.Parse(ip), port);
			asyncState = new object();
		}
		~ClientListener()
		{
			Stop();
		}
		#endregion
		#region Properties
		public static ClientListener Instance { get; private set; }
		private readonly TcpListener listener;
		private readonly object asyncState;
		#endregion
		#region Methods
		public static void Initialize()
		{
		    var settings = Configuration.Configuration.Instance.ServerSettings;
			int port = settings.Port;
			string ip = settings.ListenIP;
		    Logs.Main.DebugFormat("Initializing Listener to IP '{0}' on port {1}", ip, port);
			Instance = new ClientListener(ip, port);
            Instance.Start();
		}
		public void Start()
		{
			listener.Start();
			BeginAcceptClient();
		}
		public void Stop()
		{
			listener.Stop();
		}

		protected virtual void OnNewClientConnected(Socket client)
		{
			if(NewClientAccepted != null)
				NewClientAccepted(this, new NewClientAcceptedEventArgs(client));
		}

		private void BeginAcceptClient()
		{
			listener.BeginAcceptSocket(EndAcceptClient, asyncState);
		}
		private void EndAcceptClient(IAsyncResult ar)
		{
			Socket client = listener.EndAcceptSocket(ar);
		    Logs.Network.DebugFormat("Accepted client from '{0}'", client.RemoteEndPoint);
			OnNewClientConnected(client);
			BeginAcceptClient();
		}

		#endregion
		#region Events

		public EventHandler<NewClientAcceptedEventArgs> NewClientAccepted;

		#endregion
	}
}
