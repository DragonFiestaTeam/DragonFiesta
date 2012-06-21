using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.Zone.Game;
using Zepheus.Zone.Networking;
using Zepheus.Zone.Managers;

namespace Zepheus.Zone.Handlers
{
    public sealed class Handler19
    {
        [PacketHandler(CH19Type.CommercialReqest)]
        public static void CommercialReqest(ZoneClient pClient, Packet pPacket)
        {
            ushort MapObjectID;
            if (!pPacket.TryReadUShort(out MapObjectID))
                return;
            CommercialManager.Instance.AddComercialRequest(pClient, MapObjectID);
        }
        [PacketHandler(CH19Type.CommercialReqestDecline)]
        public static void CommercialReqestDecline(ZoneClient pClient, Packet pPacket)
        {
            CommercialManager.Instance.RemoveReqest(pClient);
        }
        [PacketHandler(CH19Type.CommercialAccept)]
        public static void CommercialAccept(ZoneClient pClient, Packet pPacket)
        {
          //Todo Commercial
        }
    }
}
