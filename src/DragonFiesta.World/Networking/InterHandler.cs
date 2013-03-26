using DragonFiesta.Messages.World;
using DragonFiesta.Messages.Zone;
using DragonFiesta.Messages.Login;
using DragonFiesta.Util;
using DragonFiesta.World.Core;
using DragonFiesta.InterNetwork;
using DragonFiesta.Data;
using DragonFiesta.Data.Transfer;

namespace DragonFiesta.World.Networking
{
	public static class InterHandler
    {
        #region Server
        [InternMessageHandler(typeof(WorldServerSetId))]
		public static void HandleSeverSetId(IMessage pMessage)
        {
            /* HANDLED BY CALLBACK */
		}

        [InternMessageHandler(typeof(ZoneAttach))]
        public static void HandleZoneAttach(IMessage pMessage)
        {
            var message = (ZoneAttach)pMessage;

            if (message.Password == "test")//todo check password from config
            {
                ZoneServer pZone = new ZoneServer
                {
                    IP = message.IP,
                    Port = message.Port,
                    QeueName = message.QueueName,
                };

                ZoneManager.Instance.RegisterZone(pZone);
            }

        }

        [InternMessageHandler(typeof(RegisterMapList))]
        public static void HandleZoneRegister(IMessage pMessage)
        {
            var message = (RegisterMapList)pMessage;

            ZoneReady ready = new ZoneReady
            {
                Id = System.Guid.NewGuid(),
                ZoneID = message.ZoneID
            };
            ZoneServer s = ZoneManager.Instance.GetZone(message.ZoneID);
            ZoneManager.Instance.SendToZone(s, ready);
        }

        [InternMessageHandler(typeof(ZoneReady))]
        public static void HandleZoneReady(IMessage pMessage)
        {
            var message = (ZoneReady)pMessage;

            ZoneManager.Instance.GetZone(message.ZoneID).IsReady = true;

            ZoneServer s = ZoneManager.Instance.GetZone(message.ZoneID);
            ZoneManager.Instance.SendToZone(s, pMessage);
        }

        [InternMessageHandler(typeof(ZonePing))]
        public static void HandleZonePing(IMessage pMessage)
        {

            var message = (ZonePing)pMessage;
            ZoneManager.Instance.UpdatePing(message.ZoneID);
            ZoneServer s =  ZoneManager.Instance.GetZone(message.ZoneID);
            //ZoneManager.Instance.SendToZone(s,message);
        }
        #endregion
        #region Client
        #region World
        [InternMessageHandler(typeof(ClientFromLoginToWorld))]
        public static void HandleClientFromLoginToWorld(IMessage pMessage)
        {
            var message = (ClientFromLoginToWorld)pMessage;
            ClientTransfer transfer = new ClientTransfer
            {
                Access_level = message.Access_level,
                AccountID = message.AccountID,
                AuthHash = message.AuthHash,
                IP = message.IP,
                UserName = message.UserName,
                Type = TransferType.LoginToWorld,
                TransferStartTime = System.DateTime.Now,
            };
            message.Id = System.Guid.NewGuid();
            ClientTransferManager.Instance.AddTransfer(transfer);
        }

        #endregion
        #region Zone
        [InternMessageHandler(typeof(CharacterReadyToLogin))]
        public static void HandleCharacterReadyToLogin(IMessage pMessage)
        {
            var message = (CharacterReadyToLogin)pMessage;
            ClientTransfer trans;
            if (ClientTransferManager.Instance.GetTransfer(message.CharacterName, out trans))
            {
                WorldClient pClient = WorldClientManager.Instance.GetClientByCharID(message.AccountID);
                if (trans.CharacterID != message.CharacterID || trans.AccountID != message.AccountID
                    || trans.Access_level != trans.Access_level || trans.UserName != message.AccountName
                    || trans.Type != TransferType.WorldToZone || pClient == null)
                {
                    Map pMap = DataProvider.DataProvider.Instance.GetMap(message.MapID);
                    if (pMap != null)
                    {
                        message.ReadyOK = false;
                        ZoneManager.Instance.SendToZone(pMap.pZoneServer, message);
                    }
                    return;
                }
                else
                {
                    Map pMap = DataProvider.DataProvider.Instance.GetMap(message.MapID);
                    if (pMap != null)
                    {
                        message.ReadyOK = true;
                        pClient.AccountInfo.Logged = true;
                        ServerHandler.SH2Methods.Ping(pClient);
                        ZoneManager.Instance.SendToZone(pMap.pZoneServer, message);
                        EventManager.Instance.InvokeOnCharacterLogin(pClient.pCharacter);
                    }

                }

            }

        }
        #endregion
        #endregion
    }
}