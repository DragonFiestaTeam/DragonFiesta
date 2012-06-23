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
        [PacketHandler(CH19Type.CommercialRemoveItem)]
        public static void CommercialRemovitem(ZoneClient pClient, Packet pPacket)
        {
            byte pSlot;
            if (!pPacket.TryReadByte(out pSlot))
                return;
            if (pClient.Character.Commercial == null)
                return;
            pClient.Character.Commercial.RemoveItemToHandel(pClient.Character, pSlot);
        }
        [PacketHandler(CH19Type.CommercialAccept)]
        public static void CommercialAccept(ZoneClient pClient, Packet pPacket)
        {
            Managers.CommercialManager.Instance.AcceptComercial(pClient);
        }
        [PacketHandler(CH19Type.CommercialChangeMoney)]
        public static void CommercialChangeMoney(ZoneClient pClient, Packet pPacket)
        {
            long money;
            if(!pPacket.TryReadLong(out money))
                return;
            if (pClient.Character.Commercial != null)
            {
                pClient.Character.Commercial.ChangeMoneyToCommercial(pClient.Character, money);
            }
        }
        [PacketHandler(CH19Type.CommercialLock)]
        public static void CommercialLock(ZoneClient pClient, Packet pPacket)
        {
            if (pClient.Character.Commercial != null)
            {
                pClient.Character.Commercial.CommercialLock(pClient.Character);
            }
        }
        [PacketHandler(CH19Type.CommercialAddItem)]
        public static void CommercialAddItem(ZoneClient pClient, Packet pPacket)
        {
            byte pSlot;
            if(!pPacket.TryReadByte(out pSlot))
            return;

            if(pClient.Character.Commercial == null)
                return;
            pClient.Character.Commercial.AddItemToHandel(pClient.Character, pSlot);
        }
        [PacketHandler(CH19Type.CommercialAgree)]
        public static void CommercialAgree(ZoneClient pClient, Packet pPacket)
        {
            if (pClient.Character.Commercial == null)
                return;

            pClient.Character.Commercial.AcceptCommercial(pClient.Character);
        }
        [PacketHandler(CH19Type.CommercialBreak)]
        public static void CommercialBreak(ZoneClient pClient, Packet pPacket)
        {
            if (pClient.Character.Commercial == null)
                return;

            pClient.Character.Commercial.CommercialBreak(pClient.Character);
            pClient.Character.Commercial = null;

        }
    }
}
