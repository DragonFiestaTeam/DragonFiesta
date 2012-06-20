using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.Zone.Game;
using Zepheus.Zone.Networking;

namespace Zepheus.Zone.Handlers
{
    public sealed class Handler19
    {
        [PacketHandler(CH19Type.CommercialReqest)]
        public static void CommercialReqest(ZoneClient pClient, Packet pPacket)
        {
            ushort unk;
            if (!pPacket.TryReadUShort(out unk))
                return;
            System.Console.WriteLine(unk);
            System.Console.WriteLine(pClient.Character.MapObjectID);
            System.Console.WriteLine(pClient.Character.ID);
            SendTestReqest(pClient);
        }
        public static void SendTestReqest(ZoneClient pClient)
        {
            using(var pPacket = new Packet(SH19Type.SendCommercialReqest))
            {
                pPacket.WriteUShort(45);
                pClient.SendPacket(pPacket);
            }
        }
    }
}
