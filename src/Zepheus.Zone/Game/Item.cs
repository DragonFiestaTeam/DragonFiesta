using System;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Data;
using Zepheus.FiestaLib.Networking;
using Zepheus.Zone.Data;
using Zepheus.Database.Storage;
namespace Zepheus.Zone.Game
{
    public class Item
    {
        public virtual short Amount { get; set; }
        public ushort ItemID { get;  set; } 
        public virtual Character Owner { get; set; } 
        public virtual DateTime? Expires { get; set; }
        public virtual sbyte Slot { get; set;  }
        public ItemInfo Info { get { return DataProvider.Instance.GetItemInfo(this.ItemID); } }

        public Item(DatabaseItem item)
        {
        }

        public Item(DroppedItem item, ZoneCharacter pNewOwner, sbyte pSlot)
        {
         Item dbi = new Item();
            dbi.Amount = item.Amount;
            dbi.ItemID= item.ItemID;
            dbi.Slot = pSlot;
            dbi.Owner = pNewOwner.character;
            Program.CharDBManager.GetClient().ExecuteQuery("INSERT INTO Items (Owner,Slot,ItemID,Amount) VALUES ('" + pNewOwner.ID + "','" + pSlot + "','" + item.ItemID + "','" + item.Amount + "')");
            ItemID = item.ItemID;
            pNewOwner.InventoryItems.Add(pSlot, dbi);
        }

        public Item()
        {
        }
        public static Item ItemInfoToItem(ItemInfo inf,short amount)
        {
                Item mItem = new Item
                {
                    ItemID = inf.ItemID,
                    Amount = amount,
                    Slot = (sbyte)inf.Slot,
                };
                return mItem;
        }
        public virtual void Remove()
        {
            if (this != null)
            {
                Program.CharDBManager.GetClient().ExecuteQuery("DELETE  FROM Items WHERE Owner='" + this.Owner.ID + "' AND ItemID='" + this.ItemID + "' AND Slot='" + this.Slot + "'");
            }
        }

        public uint GetExpirationTime()
        {
            return this.Expires.HasValue ? this.Expires.Value.ToFiestaTime() : 0;
        }

        public virtual void WriteInfo(Packet packet)
        {
            packet.WriteByte(5); //entry length
            packet.WriteSByte(this.Slot);
            packet.WriteByte(0x24); //status?
            WriteItemStats(packet);
        }

        public void WriteItemStats(Packet packet)
        {
            packet.WriteUShort(ItemID);
            packet.WriteByte((byte)Amount);
        }
    }
}
