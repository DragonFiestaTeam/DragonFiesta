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
        public ushort ID { get; private set; }
        public uint Owner { get; set; }
        public virtual DateTime? Expires { get; set; }
        // public ItemSlot SlotType { get; private set; }
        public sbyte Slot { get; set; }

        public ushort Count { get; set; }
        public ItemInfo Info { get { return DataProvider.Instance.GetItemInfo(this.ID); } }

        public byte Upgrades { get; set; }
        public byte StatCount { get; private set; }
        public bool IsEquipped { get; set; }
        public ushort Str { get; private set; }
        public ushort End { get; private set; }
        public ushort Dex { get; private set; }
        public ushort Int { get; private set; }
        public ushort Spr { get; private set; }

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
            switch (this.Info.Slot)
            {
                case ItemSlot.Weapon:
                    length = 53;                // Base data length
                    break;
                case ItemSlot.Weapon2:
                    if (Info.TwoHand)
                    {
                        //bow
                        length = 53;
                    }
                    else
                    {
                        //shield
                        length = 16;                // Base data length
                    }
                    break;
                case ItemSlot.Helm:
                case ItemSlot.Armor:
                case ItemSlot.Pants:
                case ItemSlot.Boots:
                    length = 16;                // Base data length
                    break;
                case ItemSlot.Necklace:
                case ItemSlot.Earings:
                case ItemSlot.Ring:
                    length = 26;                // Base data length
                    break;
                case ItemSlot.CostumeWeapon:
                case ItemSlot.CostumeShield:
                    length = 8;                 // Base data length
                    break;
                case ItemSlot.Pet:
                    length = 16;                // Base data length
                    break;
                default:
                    length = 12;                // Base data length
                    break;
            }
            return length;
        }
        public void WriteItemInfo(Packet pPacket)
        {
            byte statCount = 0;
            if (Str > 0) statCount++;
            if (End > 0) statCount++;
            if (Dex > 0) statCount++;
            if (Spr > 0) statCount++;
            if (Int > 0) statCount++;

            byte length = CalculateDataLen();
            length += (byte)(statCount * 3);    // Stat data length
            pPacket.WriteByte(length);
            pPacket.WriteByte((byte)this.Slot);
            pPacket.WriteByte(0x24);//status
            WriteItemStats(pPacket);
        }

        public void WriteItemStats(Packet pPacket)
        {
            byte statCount = 0;
            if (Str > 0) statCount++;
            if (End > 0) statCount++;
            if (Dex > 0) statCount++;
            if (Spr > 0) statCount++;
            if (Int > 0) statCount++;

            pPacket.WriteUShort(this.ID);
            switch (this.Info.Slot)
            {
                case ItemSlot.Helm:
                case ItemSlot.Armor:
                case ItemSlot.Pants:
                case ItemSlot.Boots:
                // case ItemSlot.Bow: // Shield = same
                case ItemSlot.Weapon2:
                case ItemSlot.Weapon:
                    pPacket.WriteByte(this.Upgrades); //refement?
                    pPacket.WriteUShort(0);//unk
                    pPacket.WriteUInt(0); //unk
                    if (this.Info.Slot == ItemSlot.Weapon || (this.Info.Slot == ItemSlot.Weapon2 && Info.TwoHand))
                    {

                        pPacket.WriteUShort(0);    // Nr.1 - Mob ID
                        pPacket.WriteUInt(0xFFFF);           // Nr.1 - Kill count
                        pPacket.WriteUShort(0xFFFF);    // Nr.2 - Mob ID
                        pPacket.WriteUInt(0);           // Nr.2 - Kill count
                        pPacket.WriteUShort(0xFFFF);    // Nr.3 - Mob ID
                        pPacket.WriteUInt(0);           // Nr.3 - Kill count
                        pPacket.WriteUShort(0);        // UNK
                        // packet.WriteUInt(1992027391);
                        pPacket.WriteString("1234567891234567", 16); //lencen name
                        pPacket.WriteByte(0);//unk
                    }
                    pPacket.WriteByte(0);
                    pPacket.WriteUInt(1992027391);               // Expiring time (1992027391 -  never expires)
                    //packet.WriteShort(0);
                    break;
                case ItemSlot.Pet:
                    pPacket.WriteByte(this.Upgrades);   // Pet Refinement Lol
                    pPacket.Fill(2, 0);                     // UNK
                    pPacket.WriteUInt(1992027391);               // Expiring time (1992027391 -  never expires)
                    pPacket.WriteUInt(0);               // Time? (1992027391 -  never expires)
                    break;
                case ItemSlot.Earings:
                case ItemSlot.Necklace:
                case ItemSlot.Ring:
                    pPacket.WriteUInt(1992027391);               // Expiring time (1992027391 -  never expires)
                    pPacket.WriteUInt(0);               // Time? (1992027391 -  never expires)
                    pPacket.WriteByte(this.Upgrades);   // Refinement
                    // Stats added while refining
                    pPacket.WriteUShort(0);             // it may be byte + byte too (some kind of counter when item downgrades)
                    pPacket.WriteUShort(0);             // STR
                    pPacket.WriteUShort(0);             // END
                    pPacket.WriteUShort(0);             // DEX
                    pPacket.WriteUShort(0);             // INT
                    pPacket.WriteUShort(0);             // SPR
                    break;
                case ItemSlot.CostumeWeapon:
                case ItemSlot.CostumeShield:
                    pPacket.WriteUInt(25000);           // Skin Durability
                    break;
                default:
                    pPacket.WriteUInt(1992027391);      // Expiring time (1992027391 -  never expires)
                    pPacket.WriteUInt(0);                        // Time? (1992027391 -  never expires)
                    break;
            }

            // Random stats data (Not those what were added in refinement)
            switch (this.Info.Slot)
            {                       // Stat count (StatCount << 1 | Visible(0 or 1 are stats shown or not))
                case ItemSlot.Earings:
                    pPacket.WriteByte((byte)(statCount << 1 | 1));
                    break;
                case ItemSlot.Necklace:
                case ItemSlot.Ring:
                    pPacket.WriteByte((byte)(statCount << 1 | 1));
                    break;
                case ItemSlot.Helm:
                    pPacket.WriteByte((byte)(statCount << 1 | 1));
                    break;
                case ItemSlot.Armor:
                case ItemSlot.Wing:
                    pPacket.WriteByte(0);
                    break;
                case ItemSlot.Pants:
                case ItemSlot.Boots:
                case ItemSlot.Weapon2:
                case ItemSlot.Weapon:
                    pPacket.WriteByte((byte)(statCount << 1 | 1));
                    break;
                case ItemSlot.Pet:          // Yes!! Its possible to give stats to pet also (It overrides default one(s)).
                    pPacket.WriteByte((byte)(statCount << 1 | 1));
                    break;
            }
            // foreach stat
            //pPacket.WriteByte(type);              // Stat type ( 0 = STR, 1 = END, 2 = DEX, 3 = INT, 4 = SPR )
            //pPacket.WriteUShort(amount);          // Amount
            // end foreach
            if (Str > 0) { pPacket.WriteByte(0); pPacket.WriteUShort(Str); }
            if (End > 0) { pPacket.WriteByte(1); pPacket.WriteUShort(End); }
            if (Dex > 0) { pPacket.WriteByte(2); pPacket.WriteUShort(Dex); }
            if (Spr > 0) { pPacket.WriteByte(3); pPacket.WriteUShort(Spr); }
            if (Int > 0) { pPacket.WriteByte(4); pPacket.WriteUShort(Int); }
            /* pPacket.WriteUShort(this.ID);
             pPacket.WriteByte((byte)this.Count);*/
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