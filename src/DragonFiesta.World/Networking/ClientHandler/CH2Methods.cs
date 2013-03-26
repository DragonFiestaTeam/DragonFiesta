using System;
using DragonFiesta.Networking;
using DragonFiesta.Networking.Handler.Client;
using DragonFiesta.Util;


namespace DragonFiesta.World.Networking.ClientHandler
{
    [PacketHandlerClass(CH2Type._Header)]
    public static class CH2Methods
    {
        [PacketHandler(CH2Type.Pong)]
        public static void Pong(ClientBase pSender, Packet pPacket)
        {
            var client = pSender as WorldClient;
            if(client == null)
                return;

            client.LastPong = DateTime.Now;
            // ToDo: Check for timeout 
        }

        [PacketHandler(CH2Type.Unk1)]
        public static void Handunk1(WorldClient character, Packet packet)
        {
            ServerHandler.SH2Methods.Sendunk1(character);
        }
    }
}