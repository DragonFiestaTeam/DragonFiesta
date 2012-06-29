using System;
using System.Data;
using MySql.Data.MySqlClient;
using Zepheus.Database.DataStore;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Data;
using Zepheus.FiestaLib.Networking;
using Zepheus.Zone.Data;
using Zepheus.Database.Storage;

namespace Zepheus.Zone.Game
{
    public class Item
    {
        private const string GiveItem = "give_item;";
        private const string UpdateItem = "update_item;";
        private const string DeleteItem = "DELETE FROM items WHERE ID=@id";

        public ulong UniqueID { get; protected set; }
        public virtual ushort ID { get;  set; }
        public uint Owner { get; set; }
        public virtual DateTime? Expires { get; set; }
        // public ItemSlot SlotType { get; private set; }
        public virtual sbyte Slot { get; set; }

        public virtual ushort Count { get; set; }
        public virtual ItemInfo Info { get { return DataProvider.Instance.GetItemInfo(this.ID); } }
        public Mount Mount { get; set; }

        public virtual byte StatCount { get;  set; }
        public bool IsEquipped { get; set; }
        public ushort Str { get; set; }
        public ushort End { get; set; }
        public ushort Dex { get; set; }
        public ushort Int { get; set; }
        public ushort Spr { get; set; }

        public Item(uint pOwner, ushort pID, ushort pCount)
        {
            ItemSlot type;
            if (!DataProvider.GetItemType(pID, out type))
            {
                throw new InvalidOperationException("Invalid item ID.");
            }
            this.Slot = (sbyte)type;

            this.Owner = pOwner;
            this.ID = pID;
            this.Count = pCount;
        }
        public Item()
        {
        }
        public virtual bool Delete()
        {
            // Read up on inheritance and virtual method resolution.
            // This will NOT execute for objects of type Equip unless you call base.Delete() in Equip.Delete().

            if (this.UniqueID > 0)
            {
                Program.DatabaseManager.GetClient().ExecuteQuery("DELETE FROM items WHERE ID=" + this.UniqueID + " AND Slot='" + this.Slot + "'");
                UniqueID = 0;
                Owner = 0;
                return true;
            }
            else
            {
                return false;
            }
        }
        public uint GetExpirationTime()
        {
            return this.Expires.HasValue ? this.Expires.Value.ToFiestaTime() : 0;
        }

        public virtual void Save()
        {
            if (UniqueID == 0)
            {
                using (var command = new MySqlCommand(GiveItem))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@puniqueid", MySqlDbType.Int64);
                    command.Parameters["@puniqueid"].Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.AddWithValue("@powner", this.Owner);
                    command.Parameters.AddWithValue("@pslot", this.Slot);
                    command.Parameters.AddWithValue("@pitemid", this.ID);
                    command.Parameters.AddWithValue("@pamount", this.Count);
                    Program.CharDBManager.GetClient().ExecuteQueryWithParameters(command);
                    this.UniqueID = Convert.ToUInt64(command.Parameters["@puniqueid"].Value);
                }
            }
            else
            {
                using (var command = new MySqlCommand(UpdateItem))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@puniqueid", this.UniqueID);
                    command.Parameters.AddWithValue("@powner", this.Owner);
                    command.Parameters.AddWithValue("@pslot", this.Slot);
                    command.Parameters.AddWithValue("@pamount", this.Count);
                    Program.CharDBManager.GetClient().ExecuteQueryWithParameters(command);
                }
            }
        }
        public byte CalculateDataLen()
        {
            byte length = 0;
            switch (this.Info.Class)
            {
                case ItemClass.Rider:
                    length = 16;
                    break;
                case ItemClass.Emotion:
                   length = 8;
                    break;
                case ItemClass.Furniture:
                    length = 8;
                    break;
                case ItemClass.House:
                    length = 8;
                    break;
                case ItemClass.SilverWingsOnly:
                    length = 15;
                    break;
                case ItemClass.Accessory:
                    length = 12;
                    break;
                case ItemClass.PresentBox:
                    length = 29;
                    break;
                default:
                    length = 5;
                    break;
            }
            return length;
        }
        public virtual void WriteItemInfo(Packet pPacket)
        {
            byte lenght = CalculateDataLen();
            pPacket.WriteByte(lenght);//lenght
            pPacket.WriteByte((byte)this.Slot);
           // pPacket.WriteByte(1);
           pPacket.WriteByte(0x24);//status
            WriteItemStats(pPacket);
        }

        public void WriteItemStats(Packet pPacket)
        {
            byte lengt = CalculateDataLen();
            pPacket.WriteUShort(this.ID);
            if (lengt == 5)
            {
                pPacket.WriteByte((byte)this.Count);
            }
            // pPacket.WriteByte((byte)this.Count);
            WriteItemExtraData(pPacket);
        }
        public void WriteItemExtraData(Packet pPacket)
        {
            byte statCount = 0;
            if (Str > 0) statCount++;
            if (End > 0) statCount++;
            if (Dex > 0) statCount++;
            if (Spr > 0) statCount++;
            if (Int > 0) statCount++;
            switch (this.Info.Class)
            {
                case ItemClass.Rider:
                    pPacket.WriteByte(1);   // Pet Refinement Lol
                    pPacket.Fill(2, 0);                     // UNK
                    pPacket.WriteUInt(1992027391);               // Expiring time (1992027391 -  never expires)
                    pPacket.WriteUInt(1992027391);               // Time? (1992027391 -  never expires)
                    break;
                case ItemClass.PremiumItem:
                    //todo sniff
                    break;
                case ItemClass.Accessory:
                      pPacket.WriteUInt(0); //time shit
                    pPacket.WriteUInt(0);
                    break;
                case ItemClass.Furniture:
                    pPacket.WriteUInt(0);//expires time 0 = 0 never
                    break;
                case ItemClass.Skillbook:
                    break;
                case ItemClass.Emotion:
                    pPacket.WriteUInt(0);//expires time 0 = 0 never
                    break;
 
                case ItemClass.SilverWingsOnly:
                    //now Client crashing?
                    pPacket.WriteHexAsBytes("90 C8 09 01 FF FF 10 27 00 00 10 27 00 00 0F");
                    break;
                case ItemClass.PresentBox:
                    pPacket.WriteByte(10);
                    pPacket.WriteHexAsBytes("00 0C 98 3A A7 ED C4 00 00 0C 9C 3A A7 ED C4 00 00 0C A0 3A A7 ED C4 00");
                    break;
                case ItemClass.House:
                    pPacket.WriteUInt(0);//expires time 0 = 0 never
                    break;
                case ItemClass.CollectCard:
                    pPacket.WriteShort(30663);//unk
                    pPacket.WriteByte(15);//unk
                    pPacket.WriteByte(0);
                    pPacket.WriteByte(0x01);
                    break;
                default:
                    break;
            }
            switch (this.Info.Class)
            {                       // Stat count (StatCount << 1 | Visible(0 or 1 are stats shown or not))
                case ItemClass.Rider:
                    pPacket.WriteByte((byte)(statCount << 1 | 1));
                    break;
                case ItemClass.PremiumItem:
                    break;
                case ItemClass.Skillbook:
                    break;
                case ItemClass.PresentBox:
                 //   pPacket.WriteByte((byte)(statCount << 1 | 1));
                    break;
                case ItemClass.CraftStones:
                    pPacket.WriteByte((byte)(statCount << 1 | 1));
                    break;
                default:
                    break;
            }
            if (Str > 0) { pPacket.WriteByte(0); pPacket.WriteUShort(Str); }
            if (End > 0) { pPacket.WriteByte(1); pPacket.WriteUShort(End); }
            if (Dex > 0) { pPacket.WriteByte(2); pPacket.WriteUShort(Dex); }
            if (Spr > 0) { pPacket.WriteByte(3); pPacket.WriteUShort(Spr); }
            if (Int > 0) { pPacket.WriteByte(4); pPacket.WriteUShort(Int); }
        }
        public static Item LoadItem(DataRow Row)
        {
            ulong id = GetDataTypes.GetUlong(Row["ID"]);
            uint owner = GetDataTypes.GetUint(Row["Owner"]);
            sbyte slot = GetDataTypes.GetSByte(Row["Slot"]);
            ushort equipID = GetDataTypes.GetUshort(Row["ItemID"]);
            ushort amount = GetDataTypes.GetUshort(Row["Amount"]);
            Item item = new Item(owner, equipID, amount)
            {
                UniqueID = id,
                Slot = slot,
            };
            return item;
        }
    }
}