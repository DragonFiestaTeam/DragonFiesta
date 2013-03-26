using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using DragonFiesta.Networking;
using DragonFiesta.Util;
using DragonFiesta.World.Networking.ServerHandler;
using DragonFiesta.Data.Transfer;
using DragonFiesta.Networking.Handler.Server;

namespace DragonFiesta.World.Networking
{
    public class WorldClientManager : ClientManager
    {

        private ConcurrentDictionary<string, WorldClient> ClientByName = new ConcurrentDictionary<string, WorldClient>();
        private ConcurrentDictionary<int,WorldClient> ClientByCharID = new ConcurrentDictionary<int,WorldClient>();

        #region .ctor
        public WorldClientManager() : base()
        {
          
        }
        #endregion
        #region Properties

        public new static WorldClientManager Instance { get; private set; }

        #endregion
        #region Methods

        public new static bool Initialize()
        {
            try
            {
                Instance = new WorldClientManager();
                ClientListener.Instance.NewClientAccepted += Instance.NewClient;
            }
            catch(Exception e)
            {
                if (!ExceptionManager.Instance.HandleException(e))
                {
                    Logs.Main.Fatal("Could not initializing WorldClientManager", e);
                    return false;
                }
            }
            return true;
        }
    
        public override void RegisterClient(ClientBase pClient)
        {
            base.RegisterClient(pClient);
            ushort pos = (ushort)Program.Random.Next(0, 480);
            pClient.XorPosition = pos;
            SH2Methods.SetXorPosition(pClient, pos);
        }

        private void NewClient(object sender, NewClientAcceptedEventArgs newClientAcceptedEventArgs)
        {
            var client = new WorldClient(newClientAcceptedEventArgs.Client);

            this.RegisterClient(client);
        }

        public WorldClient GetClientByName(string Name)
        {
            return this.ClientByName[Name];
        }

        public WorldClient GetClientByCharID(int pCharID)
        {
            return this.ClientByCharID[pCharID];
        }

        public void PingCheck()
        {
            foreach (var client in this.ClientByCharID.Values)
            {
                if (client.Authed)
                {
                    using (var packet = new Packet(SH2Type._Header, SH2Type.Ping))
                    {
                        client.SendPacket(packet);
                    }
                    client.LastPing = DateTime.Now;
                }
            }
        }

        public bool RegisterWorldClient(WorldClient pClient)//mutex needet?
        {
            WorldClient client = this.Clients.Find(m => m.Socket == pClient.Socket) as WorldClient;
            if (client != null)
            {
                if (client.AccountInfo != null)
                {
                    if (this.ClientByCharID.TryAdd(client.AccountInfo.ID, client) && this.ClientByName.TryAdd(client.AccountInfo.Username, client))
                    return true;
                }
                return false;
            }

            return false;
        }

        public bool RemoveWorldClient(WorldClient pClient)//character object check needet?
        {
            WorldClient p1,p2;
            if (this.ClientByCharID.TryRemove(pClient.pCharacter.CharacterID, out p1) && this.ClientByName.TryRemove(pClient.pCharacter.Name, out p2))
            {
                base.RemoveClient(pClient);
            }
            return false;
        }
        #endregion
    }
}