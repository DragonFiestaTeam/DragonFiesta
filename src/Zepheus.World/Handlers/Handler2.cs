
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
        public static void SendClientTime(WorldClient client, DateTime Time)
        {
        
            using (var packet = new Packet(SH2Type.UpdateClientTime))
            {
                packet.WriteInt(37);
                packet.WriteInt(Time.Minute);//minutes
                packet.WriteInt(Time.Hour);//hourses
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
        public static void SendPing(WorldClient client)
        {
            using (var packet = new Packet(SH2Type.Ping))
            {
                client.SendPacket(packet);
            }
        }
    }
}
