
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Zone.Game;
using Zepheus.Zone.Networking;
using System;
namespace Zepheus.Zone.Handlers
{
    public static class Handler2
    {
        public static void SendChatBlock(ZoneCharacter character, int seconds)
        {
            using (var packet = new Packet(SH2Type.Chatblock))
            {
                packet.WriteInt(seconds);
                character.Client.SendPacket(packet);
            }
        }
         [PacketHandler(CH2Type.Unk1)]
        public static void Handunk1(ZoneClient character, Packet packet)
        {
            using (var to = new Packet(SH2Type.unk1))
            {
                DateTime now = DateTime.Now;
                int Second = now.Second;
                int Minute = now.Minute;
                int Hour = now.Hour;
                to.WriteByte(Convert.ToByte(Hour));
                to.WriteByte(Convert.ToByte(Minute));
                to.WriteByte(Convert.ToByte(Second));
            }
        }
        [PacketHandler(CH2Type.Pong)]
        public static void HandlePong(ZoneClient character, Packet packet)
        {
            character.HasPong = true;
        }
        public static void SendPing(ZoneClient character)
        {
            using (var packet = new Packet(SH2Type.Ping))
            {
                character.SendPacket(packet);
            }
        }
    }
}
