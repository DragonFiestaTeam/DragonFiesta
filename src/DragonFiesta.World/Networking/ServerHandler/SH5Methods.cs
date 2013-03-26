using DragonFiesta.Networking;
using DragonFiesta.Networking.Handler.Server;
using DragonFiesta.Util;
using DragonFiesta.FiestaLib;


namespace DragonFiesta.World.Networking.ServerHandler
{
    public class SH5Methods
    {
        public static void SendCharCreationError(WorldClient client, CreateCharError error)
        {
            using (Packet packet = new Packet(SH5Type._Header, SH5Type.CharCreationError))
            {
                packet.WriteUShort((ushort)error);
                client.SendPacket(packet);
            }
        }
    }
}
