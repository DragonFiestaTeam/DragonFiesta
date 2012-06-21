using System;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.Zone.Game;
using Zepheus.Zone.Networking;
using Zepheus.Zone.Managers;


namespace Zepheus.Zone.Data
{
    public sealed class Commercial
    {
        #region .ctor
        public Commercial(ZoneCharacter pFrom,ZoneCharacter pTo)
        {
            this.pCharFrom = pFrom;
            this.pCharTo = pTo;
        }
        #endregion
        #region Properties
        public ZoneCharacter pCharFrom { get; private set; }
        public ZoneCharacter pCharTo { get; private set; }

        #endregion
        #region Methods
        #region privat
        private void SendPacketToAllCommercialVendors(Packet packet)
        {
            pCharFrom.Client.SendPacket(packet);
            pCharTo.Client.SendPacket(packet);
        }
        private void SendCommercialBeginn()
        {
            using(var packet = new  Packet(SH19Type.SendCommecialAccept))
            {
                packet.WriteUShort(pCharFrom.MapObjectID);
                SendPacketToAllCommercialVendors(packet);
            }
        }
        #endregion 
        #endregion
    }
}
