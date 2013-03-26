using System;
using DragonFiesta.Login.Core;
using DragonFiesta.Networking;
using DragonFiesta.Networking.Handler.Server;
using DragonFiesta.Util;
using DragonFiesta.FiestaLib;
using DragonFiesta.Login.DataTypes;

namespace DragonFiesta.Login.Networking.ServerHandler
{
	public static class SH3Methods
	{
		public static void LoginError(ClientBase client, ServerError error)
		{
			using(var packet = new Packet(SH3Type._Header, SH3Type.Error))
			{
				packet.WriteByte((byte) error);
				client.SendPacket(packet);
			}
		}
		public static void WorldList(ClientBase pClient, bool pPing)
		{
			using(var packet = new Packet(SH3Type._Header, pPing ? SH3Type.WorldListResend : SH3Type.WorldListNew))
			{
                /* BUG: List will be bugged if not filled w/ the max amount of servers
                 * It's on clients side, so dont blame me
                 * - skeleten
                 * UPDATE: w/o they default to "Low" now. lol
                 */
			    byte maxCount = 11;
			    byte count = 0;
                packet.WriteByte(maxCount);
				foreach (var info in WorldManager.Instance.Servers)
				{
					packet.WriteByte((byte) info.ID);
					packet.WriteString(info.Name, 16);
					packet.WriteByte((byte) info.Status);
					count++;
				}
                
			    for(; count < maxCount; count++)
				{
					packet.WriteByte(count);
					packet.WriteString(string.Format("DUMMY#{0}", count), 16);
					packet.WriteByte((byte) WorldStatus.Offline);
				}
				pClient.SendPacket(packet);
			}
		}


        public static void SendWorldServerIP(ClientBase pClient, WorldServerInfo pInfo, string hash)
        {
        	using(var pack = new Packet(SH3Type._Header,SH3Type.WorldServerIP))
			{
                pack.WriteByte((byte)pInfo.Status);

                pack.WriteString(pInfo.Ip, 16);

                pack.WriteUShort(pInfo.Port);
                pack.WriteString(hash, 32);
                pack.Fill(32, 0);
                pClient.SendPacket(pack);
            }
        }

		public static void WorldList(LoginClient pClient)
		{
			WorldList(pClient, pClient.SentWorldListAlready);
		}
	    public static void AllowFiles(ClientBase pClient)
	    {
	        try
	        {
	            LoginClient client = pClient as LoginClient;
                if(client == null)
                    throw new UnexpectedTypeException(typeof (LoginClient), pClient.GetType());

                using(var packet = new Packet(SH3Type._Header, SH3Type.FilecheckAllow))
	            {
	                packet.WriteBool(client.HashesAllowed);
	                client.SendPacket(packet);
	            }
	        }
	        catch (Exception e)
	        {
	            if(!ExceptionManager.Instance.HandleException(e))
	                throw;
	        }
	    }
	    public static void VersionAllowed(ClientBase pClient, bool pAllowed)
	    {
	        using(var packet = new Packet(SH3Type._Header, SH3Type.VersionAllowed))
	        {
	            packet.WriteBool(pAllowed);
                packet.WriteByte(0);        // unk
	            pClient.SendPacket(packet);
	        }
	    }
	}
}
