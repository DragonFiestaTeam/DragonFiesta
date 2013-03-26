using System;
using DragonFiesta.Networking;
using DragonFiesta.Networking.Handler.Server;

namespace DragonFiesta.World.Networking.ServerHandler
{
    public static class SH2Methods
    {
        public static void SetXorPosition(ClientBase pClient, ushort pPosition)
        {
            using (var packet = new Packet(SH2Type._Header, SH2Type.SetXorKeyPosition))
            {
                packet.WriteUShort(pPosition);
                pClient.SendPacket(packet);
            }
        }

        public static void Ping(ClientBase pClient)
        {
            var client = pClient as WorldClient;
            using (var packet = new Packet(SH2Type._Header, SH2Type.Ping))
            {
                pClient.SendPacket(packet);
            }
            if (client != null)
                client.LastPing = DateTime.Now;
        }

        public static void Sendunk1(ClientBase pClient)
        {
            using (var to = new Packet(SH2Type._Header, SH2Type.Unk1))
            {
                DateTime now = DateTime.Now;
                to.WriteByte(Convert.ToByte(now.Hour));
                to.WriteByte(Convert.ToByte(now.Minute));
                to.WriteByte(Convert.ToByte(now.Second));
                pClient.SendPacket(to);
            }
        }
    }
}