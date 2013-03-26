using System;
using DragonFiesta.Login.Core;
using DragonFiesta.Login.DataTypes;
using DragonFiesta.Login.Networking.ServerHandler;
using DragonFiesta.Networking;
using DragonFiesta.Networking.Handler.Client;
using DragonFiesta.Util;
using DragonFiesta.FiestaLib;
using DragonFiesta.Messages.World;
using DragonFiesta.InterNetwork;

namespace DragonFiesta.Login.Networking.ClientHandler
{
	[PacketHandlerClass(CH3Type._Header)]
	public class CH3Handler
	{
		[PacketHandler(CH3Type.Version)]
		public static void Version(ClientBase sender, Packet packet)
		{
			ushort year, version;
			if(!packet.TryReadUShort(out year)
			|| !packet.TryReadUShort(out version))
			{
				throw new InvalidPacketException();
			}
		    LoginManager.Instance.VersionCheck(sender, new LoginVersion(year, version));
		}
		[PacketHandler(CH3Type.Login)]
		public static void Login(ClientBase sender, Packet packet)
		{
			string username, password;
			if(!packet.TryReadString(out username, 18)
			|| !packet.TryReadString(out password, 16))
			{
				throw new InvalidPacketException();
			}
			LoginManager.Instance.Auth(sender, username, password);
		}
		[PacketHandler(CH3Type.WorldListRequest)]
		public static void WorldRequest(ClientBase sender, Packet packet)
		{
			var client = sender as LoginClient;
			if(client == null)
			{
				throw new UnexpectedTypeException(typeof (LoginClient), sender.GetType());
			}
			SH3Methods.WorldList(client);
		}

		[PacketHandler(CH3Type.BackToCharSelect)]
		public static void BackToCharList(ClientBase sender, Packet packet)
		{
            // TODO: EVEN USED WTF?
            /* Note - at least not within the Login-Server I guess?
             * -skeleten
             * UPDATE: SENT WHEN CLICKED "END" FROM SERVER SELECT
             */
            // throw new NotImplementedException();
		}
        [PacketHandler(CH3Type.FileHash)]
        public static void FileHashes(ClientBase sender, Packet packet)
        {
            string hash;
            if(!packet.TryReadString(out hash))
            {
                throw new InvalidPacketException();
            }

            LoginManager.Instance.FileHashes(sender, hash);
        }
        [PacketHandler(CH3Type.WorldSelect)]
        public static void WorldSelect(LoginClient pSender, Packet pPacket)
        {
            byte id;
            if(!pPacket.TryReadByte(out id))
                return;

           WorldServerInfo server = WorldManager.Instance.Servers.Find(m => m.ID == id);
           if (server != null)
           {
               switch (server.Status)
               {
                   case WorldStatus.Maintenance:
                       SH3Methods.LoginError(pSender, ServerError.ServerMaintenance);
                       break;
                   case WorldStatus.Offline:
                       SH3Methods.LoginError(pSender, ServerError.ServerMaintenance);
                       break;
                   default:

                       break;
               }

           }
           string hash = System.Guid.NewGuid().ToString().Replace("-", "");
           SH3Methods.SendWorldServerIP(pSender, server, hash);
           ClientFromLoginToWorld message = new ClientFromLoginToWorld()
           {
               Id = Guid.NewGuid(),
               Access_level = pSender.Access_level,
               AccountID = pSender.AccountID,
               UserName = pSender.Username,
               AuthHash = hash,
                IP = pSender.IP.ToString(),
           };
           InternMessageManager.Instance.Send(message);
        }

	}
}