﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.World.Networking;

namespace Zepheus.World
{
	[ServerModule(InitializationStage.Clients)]
	public sealed class ClientManager
	{
		public static ClientManager Instance { get; private set; }
		public int WorldLoad { get { return ClientCount(); } }
		private readonly List<WorldClient> clients = new List<WorldClient>();
		private readonly ConcurrentDictionary<string, WorldClient> clientsByName = new ConcurrentDictionary<string, WorldClient>();
		private readonly ConcurrentDictionary<string, WorldClient> zoneAdd = new ConcurrentDictionary<string, WorldClient>();
		private readonly ConcurrentDictionary<string, ClientTransfer> transfers = new ConcurrentDictionary<string, ClientTransfer>();
		private readonly Timer expirator;
		private int transferTimeout = 1;

		public ClientManager()
		{
			expirator = new Timer(2000);
			expirator.Elapsed += ExpiratorElapsed;
			expirator.Start();
		}

		private int ClientCount()
		{
			lock (clients)
			{
				return clients.Count;
			}
		}

		public void AddClient(WorldClient client)
		{
			lock (clients)
			{
				try
				{
					clients.Add(client);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}

			}
		}
        public void UpdateClientTime(DateTime dateTime)
        {
            lock (clients)
            {
                foreach (WorldClient kvp in clientsByName.Values)
                {
                    Handlers.Handler2.SendClientTime(kvp, dateTime);
                }
            }
        }
		public void AddClientByName(WorldClient client)
		{
			if (client.Character != null && !clientsByName.ContainsKey(client.Character.Character.Name))
			{
				clientsByName.TryAdd(client.Character.Character.Name, client);
			
			}
			else Log.WriteLine(LogLevel.Warn, "Trying to register client by name without having Character object.");
		}
		public void AddZoneTrans(string name,WorldClient client)
		{
			zoneAdd.TryAdd(name,client);
		}
		public void RemoveZoneTrand(string name , WorldClient ccclient)
		{
			zoneAdd.TryRemove(name, out ccclient);
		}
		readonly List<WorldClient> pingTimeouts = new List<WorldClient>();
		public void PingCheck(DateTime now)
		{
			lock (clients)
			{

				foreach (var client in clients)
				{
					if (!client.Authenticated) continue; //they don't have ping shit, since they don't even send a response.
					if (client.Pong)
					{
						Handlers.Handler2.SendPing(client);
						client.Pong = false;
						client.lastPing = now;
					}
					else
					{
						if (now.Subtract(client.lastPing).TotalSeconds >= 300)
						{
							pingTimeouts.Add(client);
							Log.WriteLine(LogLevel.Debug, "Ping timeout from {0} ({1})", client.Username, client.Host);
						}
					}
				}

				foreach (var client in pingTimeouts)
				{
					clients.Remove(client);
					client.Disconnect();
				}
				pingTimeouts.Clear();
			}
		}

		public WorldClient GetClientByCharname(string name)
		{
			WorldClient client;
			if (clientsByName.TryGetValue(name, out client))
			{
				return client;
			}
			else return null;
		}
	  
		public void RemoveClient(WorldClient client)
		{
			lock (clients)
			{
				clients.Remove(client);
			}

			if (client.Character != null)
			{
				WorldClient deleted;
				clientsByName.TryRemove(client.Character.Character.Name, out deleted);
				if (deleted != client)
				{
					Log.WriteLine(LogLevel.Warn, "There was a duplicate client in clientsByName: {0}", client.Character.Character.Name);
				}
			}
		}

		public void AddTransfer(ClientTransfer transfer)
		{
			if (transfer.Type != TransferType.World)
			{
				Log.WriteLine(LogLevel.Warn, "Received a GAME transfer request. Trashing it.");
				return;
			}
			if (transfers.ContainsKey(transfer.Hash))
			{
				ClientTransfer trans;
				if (transfers.TryRemove(transfer.Hash, out trans))
				{
					Log.WriteLine(LogLevel.Warn, "Duplicate client transfer hash. {0} hacked into {1}", transfer.HostIP, trans.HostIP);
				}
			}

			if (!transfers.TryAdd(transfer.Hash, transfer))
			{
				Log.WriteLine(LogLevel.Warn, "Error registering client transfer.");
			}
		}

		public bool RemoveTransfer(string hash)
		{
			ClientTransfer trans;
			return transfers.TryRemove(hash, out trans);
		}

		public ClientTransfer GetTransfer(string hash)
		{
			ClientTransfer trans;
			if(transfers.TryGetValue(hash, out trans)){
				return trans;
			} else return null;
		}

		public void SendPacketToAll(Packet pPacket, WorldClient pExcept = null)
		{
			foreach (var client in clients.FindAll(c => c != pExcept))
			{
				client.SendPacket(pPacket);
			}
		}

		private readonly List<string> toExpire = new List<string>();
		void ExpiratorElapsed(object sender, ElapsedEventArgs e)
		{
			//this is actually executed in the main thread! (ctor is in STAThread)
			foreach (var transfer in transfers.Values)
			{
				if ((DateTime.Now - transfer.Time).TotalMilliseconds >= transferTimeout)
				{
					toExpire.Add(transfer.Hash);
					Log.WriteLine(LogLevel.Debug, "Transfer timeout for {0}", transfer.Username);
				}
			}

			if (toExpire.Count > 0)
			{
				foreach (var expired in toExpire)
				{
					ClientTransfer trans;
					transfers.TryRemove(expired, out trans);
				}
				toExpire.Clear();
			}
		}
		
		public bool IsOnline(string pCharname)
		{
			return clientsByName.ContainsKey(pCharname);
		}

		[InitializerMethod]
		public static bool Load()
		{
			Instance = new ClientManager
			{
				transferTimeout = Settings.Instance.TransferTimeout
			};
			Log.WriteLine(LogLevel.Info, "ClientManager initialized.");
			return true;
		}

		[CleanUpMethod]
		public static void CleanUp()
		{
			Log.WriteLine(LogLevel.Info, "Cleaning up ClientManager.");
			while (Instance.clients.Count > 0)
			{
				var cl = Instance.clients[0];
				cl.Disconnect();
				Instance.clients.Remove(cl);
			}
			Instance = null;
		}
	}
}
