using System;
using DragonFiesta.Networking;
using DragonFiesta.Networking.Handler.Client;
using DragonFiesta.Util;
using DragonFiesta.FiestaLib;
using DragonFiesta.World.Game;
using DragonFiesta.Messages.World;
using DragonFiesta.Data.Transfer;
using DragonFiesta.World.Database;
using DragonFiesta.World.Networking.ServerHandler;
using DragonFiesta.Data;

namespace DragonFiesta.World.Networking.ClientHandler
{
    [PacketHandlerClass(CH4Type._Header)]
    public static class CH4Methods
    {
        [PacketHandler(CH4Type.CharSelect)]
        public static void CharacterSelectHandler(WorldClient sender, Packet packet)
        {
            byte slot;
            WorldCharacter pchar;
            if (!packet.TryReadByte(out slot) || slot > 5 || !sender.CharacterList.TryGetValue((int)slot,out pchar))
            {
                Logs.Main.Error("" + sender.AccountInfo.Username + " selected an invalid character.");
                return;
            }
            Map map;
            if(DataProvider.DataProvider.Instance.GetMap(pchar.Position.Map,out map))
            {
                if (map.pZoneServer != null)
                {
                    sender.pCharacter = pchar;
                    sender.pCharacter.pClient = sender;
                    SH4Methods.SendZoneServerIP(sender, map.pZoneServer);

                    ClientFromWorldToZone Transfer = new ClientFromWorldToZone
                    {
                        Id = Guid.NewGuid(),
                        Access_level = sender.AccountInfo.Admin,
                        CharacterID = pchar.CharacterID,
                        RandomID = sender.RandomID,
                        AccountID = sender.AccountInfo.ID,
                        ZoneID = map.pZoneServer.ID,
                        IP = map.pZoneServer.IP,
                        UserName = pchar.Name,
                    };

                    ClientTransfer transfer = new ClientTransfer
                    {
                        Type = TransferType.WorldToZone,
                        Access_level = sender.AccountInfo.Admin,
                        RandomID = sender.RandomID,
                        AccountID = sender.AccountInfo.ID,
                        UserName = sender.pCharacter.Name,
                        CharacterID = sender.pCharacter.CharacterID,
                        TransferStartTime = DateTime.Now,
                        IP = Transfer.IP

                    };
                    ClientTransferManager.Instance.AddTransfer(transfer);
                    Core.ZoneManager.Instance.SendToZone(map.pZoneServer, Transfer);
                }
                else
                {
                    SH4Methods.SendConnectError(sender, ConnectErrors.FailedToConnectToMapServer);
                    Logs.Main.Error("Character tried to ZonServer   map: " + map.ID + "");
                }
            }
            else
            {
                SH4Methods.SendConnectError(sender, ConnectErrors.MapUnderMaintenance);
                Logs.Main.Error("Character tried to join unloaded map: " + map.ID + "");
            }
        }
    }
}
