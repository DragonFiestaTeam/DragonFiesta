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

        public ZoneCharacter pCharTo { get; private set; }
        public Dictionary<byte, Item> pToHandelItemList = new Dictionary<byte, Item>();
        private long pToHandelMoney { get;  set; }
        private bool pToLocket { get;  set; }

        private long pFromHandelMoney { get;  set; }
        private bool pFromLocket { get; set; }
        public Dictionary<byte, Item> pFromHandelItemList = new Dictionary<byte, Item>();
        public ZoneCharacter pCharFrom { get; private set; }

        #endregion
        #region Methods
        #region public
        public void ChangeMoneyToCommercial(ZoneCharacter pChar, long money)
        {
            if (this.pCharFrom == pChar)
            {
                this.pFromHandelMoney = money;
                SendChangeMoney(this.pCharTo.Client,money);
            }
            else if (this.pCharTo == pCharTo)
            {
                this.pToHandelMoney = money;
                SendChangeMoney(this.pCharFrom.Client, money);
            }

        }
        public void RemoveItemToHandel(ZoneCharacter pChar,byte pSlot)
        {
            if (this.pCharFrom == pChar)
            {
                this.pFromHandelItemList.Remove(pSlot);
            }
            else if (this.pCharTo == pCharTo)
            {
                this.pToHandelItemList.Remove(pSlot);
            }
        }
        public void AddItemToHandel(ZoneCharacter pChar,byte pSlot)
        {
            Item pItem;
            if (!pChar.Inventory.InventoryItems.TryGetValue(pSlot, out pItem))
                return;
            if (this.pCharFrom == pChar)
            {
                this.pFromHandelItemList.Add(pSlot, pItem);  
            }
            else if(this.pCharTo == pCharTo)
            {
                this.pToHandelItemList.Add(pSlot, pItem);
            }
               
        }
        public void CommercialLock(ZoneCharacter pChar)
        {
            if (this.pCharFrom == pChar)
            {
                this.pFromLocket = true;
                SendCommercialLock(this.pCharTo.Client);
            }
            else if (this.pCharTo == pCharTo)
            {
                this.pToLocket = true;
                SendCommercialLock(this.pCharFrom.Client);
            }

        }
        public void CommercialUnlock(ZoneCharacter pChar)
        {
            if (this.pCharFrom == pChar)
            {
                this.pFromLocket = false;
            }
            else if (this.pCharTo == pCharTo)
            {
                this.pToLocket = false;
            }
        }
        public void CommercialBreak()
        {
            //TodoSendBreakpacket
        }
        public void AcceptCommercial(ZoneCharacter pChar)
        {
        }
        #endregion
        #region privat
        private void SendPacketToAllCommercialVendors(Packet packet)
        {
            pCharFrom.Client.SendPacket(packet);
            pCharTo.Client.SendPacket(packet);
        }
        #endregion 
        #region Packets
        private void SendCommercialLock(ZoneClient pClient)
        {
            using (var packet = new Packet(SH19Type.SendCommercialLock))
            {
                pClient.SendPacket(packet);
            }

        }
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
        private void SendChangeMoney(ZoneClient pClient, long money)
        {
            using (var packet = new Packet(SH19Type.SendChangeMoney))
            {
                packet.WriteLong(money);
                pClient.SendPacket(packet);
            }
        }
        #endregion
        #endregion
    }
}
