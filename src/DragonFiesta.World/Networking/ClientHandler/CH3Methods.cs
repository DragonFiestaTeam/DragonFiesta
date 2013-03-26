using System;
using DragonFiesta.Networking;
using DragonFiesta.Networking.Handler.Client;
using DragonFiesta.Util;
using DragonFiesta.FiestaLib;
using DragonFiesta.World;
using DragonFiesta.Data.Transfer;
using DragonFiesta.World.Database;
using DragonFiesta.World.Networking.ServerHandler;

namespace DragonFiesta.World.Networking.ClientHandler
{
    [PacketHandlerClass(CH3Type._Header)]
    public static class CH3Methods
    {
        [PacketHandler(CH3Type.WorldClientKey)]
        public static void WorldClientKey(WorldClient sender, Packet packet)
        {
            string key;
            if (!packet.ReadSkip(18) || !packet.TryReadString(out key, 32))
            {
                Logs.Main.Warn("Invalid connection request.");
                sender.Socket.Close();
                return;
            }
            ClientTransfer transfer;
            if (ClientTransferManager.Instance.GetTransfer(key, out transfer))
            {
                if (transfer.IP != sender.IP.ToString())
                {
                    Logs.Main.Warn(string.Format("Remotehack from {0}", sender.Socket.RemoteEndPoint.ToString()));
                    ServerHandler.SH3Methods.ServerError(sender, ServerError.InvalidCredentials);
                }
                else
                {

                    sender.AccountInfo = new DragonFiesta.Data.Acount
                    {
                        Admin = transfer.Access_level,
                        ID = transfer.AccountID,
                        Username = transfer.UserName,
                        Logged = true,
                    };
                    sender.Authed = true;
                    sender.LastPong = DateTime.Now;
                    sender.LastPing = DateTime.Now;
                    sender.RandomID = (ushort)new Random().Next(ushort.MaxValue);
                    ClientTransferManager.Instance.RemoveTransfer(transfer.AuthHash);
                    Logs.Main.Debug(string.Format("{0} authenticated.", sender.IP));//use username?
                    sender.CharacterList = CharacterDB.GetCharacterListFromDatabase(sender.AccountInfo.ID);
                    WorldClientManager.Instance.RegisterWorldClient(sender);
                    SH3Methods.SendCharacterList(sender);
                }
            }
            else
            {
                Logs.Main.Warn(string.Format("Invalid client authentication from {0}", sender.Socket.RemoteEndPoint.ToString()));
                ServerHandler.SH3Methods.ServerError(sender, ServerError.InvalidCredentials);
            }
        }
    }
}
