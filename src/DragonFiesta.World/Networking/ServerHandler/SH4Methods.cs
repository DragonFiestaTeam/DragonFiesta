using DragonFiesta.Networking;
using DragonFiesta.Networking.Handler.Server;
using DragonFiesta.Util;
using DragonFiesta.FiestaLib;
using DragonFiesta.Data;

namespace DragonFiesta.World.Networking.ServerHandler
{
    public class SH4Methods
    {
        public static void SendZoneServerIP(WorldClient client,ZoneServer pZoneServer)
        {
            using (var packet = new Packet(SH4Type._Header,SH4Type.ServerIP))
            {
                packet.WriteString(pZoneServer.IP, 16);
                packet.WriteUShort((ushort)pZoneServer.Port);
                client.SendPacket(packet);
            }
        }

        public static void SendConnectError(WorldClient client, ConnectErrors error)
        {
            using (var packet = new Packet(SH4Type._Header,SH4Type.ConnectError))
            {
                packet.WriteUShort((ushort)error);
                client.SendPacket(packet);
            }
        }

    }
}
