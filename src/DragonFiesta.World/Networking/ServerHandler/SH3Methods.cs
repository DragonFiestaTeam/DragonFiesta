using DragonFiesta.Networking;
using DragonFiesta.Networking.Handler.Server;
using DragonFiesta.Util;
using DragonFiesta.FiestaLib;

namespace DragonFiesta.World.Networking.ServerHandler
{
    public static class SH3Methods
    {
        public static void ServerError(WorldClient client, ServerError error)
        {
            using (var packet = new Packet(SH3Type._Header, SH3Type.Error))
            {
                packet.WriteUShort((byte)error);
                client.SendPacket(packet);
            }
        }

        public static void SendCharacterList(WorldClient pClient)
        {
            using (var packet = new Packet(SH3Type._Header, SH3Type.CharacterList))
            {
                packet.WriteUShort((ushort)pClient.RandomID);
                packet.WriteByte((byte)pClient.CharacterList.Values.Count);
                foreach (var tmp in pClient.CharacterList.Values)
                {
                    tmp.WriteBasicInfo(packet);
                }
                pClient.SendPacket(packet);
            }
        }
    }
}
