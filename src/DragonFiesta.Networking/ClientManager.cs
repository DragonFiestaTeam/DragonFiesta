using System;
using System.Collections.Generic;
using DragonFiesta.Util;
using System.Threading;

namespace DragonFiesta.Networking
{
	public class ClientManager
	{
		#region .ctor

	    protected ClientManager()
		{
			Clients = new List<ClientBase>();
		}

		#endregion
		#region Properties

		public static ClientManager Instance { get; private set; }
		public List<ClientBase> Clients { get; private set; }
		#endregion
		#region Methods
		public static bool Initialize()
		{
			try
			{
				Instance = new ClientManager();
			}
			catch(Exception e)
			{
				Logs.Network.Fatal("Could not intiate ClientManager", e);
				return false;
			}

			return true;
		}
		public virtual void RegisterClient(ClientBase client)
		{
			Clients.Add(client);
			client.PacketReceived += HandlePacket;
            client.Start();
		}
		public virtual void RemoveClient(ClientBase client)
		{
			Clients.Remove(client);
			client.Dispose();
		}

		private void HandlePacket(object sender, PacketReceivedEventArgs args)
		{
			PacketProcessQueue.Instance.AddToQueue((ClientBase) sender, args.Packet);
		}

		#endregion
		#region Events

		#endregion
	}
}