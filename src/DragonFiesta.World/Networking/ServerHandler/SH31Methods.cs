using DragonFiesta.Networking;
using DragonFiesta.Networking.Handler.Server;
using DragonFiesta.Util;
using DragonFiesta.FiestaLib;


namespace DragonFiesta.World.Networking.ServerHandler
{
    public class SH31Methods
    {
        public static void SendUnknown(WorldClient client)
        {
            using (var packet = new Packet(SH31Type.LoadUnk,SH31Type.LoadUnk))
            {
                packet.WriteInt(3505); //lolwut?!  charid or sumtin'
                client.SendPacket(packet);
            }
        }
    }
}
