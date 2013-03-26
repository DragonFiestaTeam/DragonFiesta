using System;
using DragonFiesta.Networking;
using DragonFiesta.Networking.Handler.Server;

namespace DragonFiesta.World.Networking.ServerHandler
{
	public static class SH2Methods
	{
		public static void SetXorPosition(ClientBase pClient, ushort pPosition)
		{
			using(var packet = new Packet(SH2Type._Header, SH2Type.SetXorKeyPosition))
			{
				packet.WriteUShort(pPosition);
				pClient.SendPacket(packet);
			}
		}

        public static void Ping(ClientBase pClient)
        {
            var client = pClient as WorldClient;
            using(var packet = new Packet(SH2Type._Header, SH2Type.Ping))
            {
                pClient.SendPacket(packet);
            }
            if (client != null)
                client.LastPing = DateTime.Now;
        }
	}
}
