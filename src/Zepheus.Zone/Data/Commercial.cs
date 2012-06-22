using System;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.Zone.Game;
using Zepheus.Zone.Networking;
using Zepheus.Zone.Managers;
using System.Collections.Generic;

namespace Zepheus.Zone.Data
{
    public sealed class Commercial
    {
        #region .ctor
        public Commercial(ZoneCharacter pFrom,ZoneCharacter pTo)
        {
            this.pCharFrom = pFrom;
            this.pCharTo = pTo;
            this.pCharFrom.Commercial = this;
            this.pCharTo.Commercial = this;
            SendCommercialBeginn();
        }
        #endregion
        #region Properties
        public ZoneCharacter pCharFrom { get; private set; }
        public ZoneCharacter pCharTo { get; private set; }
        public Dictionary<byte, Item> pFromHandelItemList = new Dictionary<byte, Item>();
        public Dictionary<byte, Item> pToHandelItemList = new Dictionary<byte, Item>();
        #endregion
        #region Methods
        #region privat
        private void SendPacketToAllCommercialVendors(Packet packet)
        {
            pCharFrom.Client.SendPacket(packet);
            pCharTo.Client.SendPacket(packet);
        }
        #endregion 
        #region Packets
        private void SendCommercialBeginn()
        {
            using (var packet = new Packet(SH19Type.SendCommecialAccept))
            {
                packet.WriteUShort(pCharFrom.MapObjectID);
                this.pCharTo.Client.SendPacket(packet);
            }
            using (var packet = new Packet(SH19Type.SendCommecialAccept))
            {
                packet.WriteUShort(pCharTo.MapObjectID);
                this.pCharFrom.Client.SendPacket(packet);
            }
        }
        #endregion
        #endregion
    }
}
