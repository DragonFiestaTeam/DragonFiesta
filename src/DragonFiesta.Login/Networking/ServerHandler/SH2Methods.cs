using DragonFiesta.Networking;
using DragonFiesta.Networking.Handler.Server;
using DragonFiesta.Util;

namespace DragonFiesta.Login.Networking.ServerHandler
{
	public static class SH2Methods
	{
		public static void SetXorPosition(ClientBase pClient, short position)
		{
		    Logs.Network.DebugFormat("Set key position to {0}", position);
			using(var packet = new Packet(SH2Type._Header, SH2Type.SetXorKeyPosition))
			{
				packet.WriteShort(position);
				pClient.SendPacket(packet);
			}
		}
	}
}
