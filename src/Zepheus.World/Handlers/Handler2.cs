
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.World.Networking;
using System;
namespace Zepheus.World.Handlers
{
    public sealed class Handler2
    {
        //this is incorrect, somehow?
        [PacketHandler(CH2Type.Pong)]
        public static void Pong(WorldClient client, Packet packet)
        {
            client.Pong = true;
        }
        public static void SendClientTime(WorldClient client, DateTime time)
        {
        
            using (var packet = new Packet(SH2Type.UpdateClientTime))
            {
                packet.WriteInt(37);
                packet.WriteInt(time.Minute);//minutes
                packet.WriteInt(time.Hour);//hourses
                packet.WriteInt(15); //day
                packet.WriteInt(1);//unk
                packet.WriteInt(112);//unk
                packet.WriteInt(3);//unk
                packet.WriteInt(45);
                packet.Fill(3, 0);//unk
                packet.WriteByte(1);
                client.SendPacket(packet);
            }
        }
        [PacketHandler(CH2Type.Unk1)]
        public static void Handunk1(WorldClient character, Packet packet)
        {
            using (var to = new Packet(SH2Type.Unk1))
            {
                DateTime now = DateTime.Now;
                int second = now.Second;
                int minute = now.Minute;
                int hour = now.Hour;
                to.WriteByte(Convert.ToByte(hour));
                to.WriteByte(Convert.ToByte(minute));
                to.WriteByte(Convert.ToByte(second));
            }
        }
        public static void SendPing(WorldClient client)
        {
            using (var packet = new Packet(SH2Type.Ping))
            {
                client.SendPacket(packet);
            }
        }
    }
}
