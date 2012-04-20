using System;
using System.Data;
using MySql.Data.MySqlClient;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Zone.Data;
using Zepheus.Database.Storage;
using Zepheus.FiestaLib.Data;
using Zepheus.Database.DataStore;
using Zepheus.Database;

namespace Zepheus.Zone.Game
{
    public class Equip : Item
    {
        private const string GiveEquip = "give_equip;";
        private const string UpdateEquip = "update_equip;";
        private const string DeleteEquip = "DELETE FROM equips WHERE ID=@id";
        public bool IsEquipped { get; set; }
        public byte Upgrades { get;  set; }

        public byte StatCount { get; private set; }
        public ushort Str { get; private set; }
        public ushort End { get; private set; }
        public ushort Dex { get; private set; }
        public ushort Int { get; private set; }
        public ushort Spr { get; private set; }
        public DateTime? Expires { get { return Expires; } set { Expires = value; } }
 

        public Equip(uint pOwner, ushort pEquipID, short pSlot) : base(pOwner, pEquipID, 1)
        {
            if (pSlot < 0)
            {
                this.Slot = (byte)-pSlot;
                this.IsEquipped = true;
            }
            else
            {
                this.Slot = (byte)pSlot;
            }
        }
        public ItemSlot SlotType
        {
            get
            {
                return DataProvider.Instance.GetItemInfo(ID).Slot;
            }
        }
        private uint GetExpiringTime()
        {
           if (Expires == null)
            {
                return 0;
            }
            else
            {
                return Expires.Value.ToFiestaTime();
            }
        }
        public override bool Delete()
        {
           if(this.UniqueID > 0)
            {
                using (var command = new MySqlCommand(DeleteEquip))
                {
                    command.Parameters.AddWithValue("@id", this.UniqueID);
                    Program.CharDBManager.GetClient().ExecuteQueryWithParameters(command);
                }
                UniqueID = 0;
                Owner = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Save()
        {
           if (UniqueID <= 0)
            {
                    using (var command = new MySqlCommand(GiveEquip))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@puniqueid", MySqlDbType.Int64);
                        command.Parameters["@puniqueid"].Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.AddWithValue("@powner", this.Owner);
                        command.Parameters.AddWithValue("@pslot", this.Slot);
                        command.Parameters.AddWithValue("@pequipid", this.ID);
                       Program.CharDBManager.GetClient().ExecuteQueryWithParameters(command);
                        this.UniqueID = Convert.ToUInt64(command.Parameters["@puniqueid"].Value);
                    }
            }
            else
            {
                using (var command = new MySqlCommand(UpdateEquip))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@puniqueid", this.UniqueID);
                    command.Parameters.AddWithValue("@powner", this.Owner);
                    command.Parameters.AddWithValue("@pslot", this.IsEquipped ? (this.Slot * -1) : this.Slot);
                    command.Parameters.AddWithValue("@pupgrades", this.Upgrades);
                    command.Parameters.AddWithValue("@pstr", this.Str);
                    command.Parameters.AddWithValue("@pend", this.End);
                    command.Parameters.AddWithValue("@pdex", this.Dex);
                    command.Parameters.AddWithValue("@pspr", this.Spr);
                    command.Parameters.AddWithValue("@pint", this.Int);
                    Program.CharDBManager.GetClient().ExecuteQueryWithParameters(command);
                }
            }
        }
        public byte CalculateDataLen()
        {
            byte length = 0;
            switch (this.SlotType)
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
   	public void WritEquipInfo(Packet packet)
		{
			byte statCount = 0;
			if (Str > 0) statCount++;
			if (End > 0) statCount++;
			if (Dex > 0) statCount++;
			if (Spr > 0) statCount++;
			if (Int > 0) statCount++;

			byte length = CalculateDataLen();
			length += (byte)(statCount * 3);    // Stat data length
			packet.WriteByte(length);
			packet.WriteByte((byte)Math.Abs(this.Slot));
			packet.WriteByte(IsEquipped ? (byte)0x20 : (byte)0x24);
			WriteEquipStats(packet);
        }
        public void WriteEquipStats(Packet packet)
        {
            byte statCount = 0;
            if (Str > 0) statCount++;
            if (End > 0) statCount++;
            if (Dex > 0) statCount++;
            if (Spr > 0) statCount++;
            if (Int > 0) statCount++;

            packet.WriteUShort(this.ID);
            switch (this.SlotType)
            {
                case ItemSlot.Helm:
                case ItemSlot.Armor:
                case ItemSlot.Pants:
                case ItemSlot.Boots:
                // case ItemSlot.Bow: // Shield = same
                case ItemSlot.Weapon2:
                case ItemSlot.Weapon:
                    packet.WriteByte(this.Upgrades);   // Refinement
                    packet.WriteByte(0);
                    packet.WriteShort(0); // Or int?
                    packet.WriteShort(0);
                    if (this.SlotType == ItemSlot.Weapon || (this.SlotType == ItemSlot.Weapon2 &&  Info.TwoHand))
                    {
                        packet.WriteByte(0);
                        // Licence data
                        packet.WriteUShort(0xFFFF);    // Nr.1 - Mob ID
                        packet.WriteUInt(0);           // Nr.1 - Kill count
                        packet.WriteUShort(0xFFFF);    // Nr.2 - Mob ID
                        packet.WriteUInt(0);           // Nr.2 - Kill count
                        packet.WriteUShort(0xFFFF);    // Nr.3 - Mob ID
                        packet.WriteUInt(0);           // Nr.3 - Kill count
                        packet.WriteUShort(0xFFFF);        // UNK
                        packet.WriteString("", 16);    // First licence adder name
                    }
                    packet.WriteByte(0);
                    packet.WriteUInt(GetExpiringTime());               // Expiring time (1992027391 -  never expires)
                    //packet.WriteShort(0);
                    break;
                case ItemSlot.Pet:
                    packet.WriteByte(this.Upgrades);   // Pet Refinement Lol
                    packet.Fill(2, 0);                     // UNK
                    packet.WriteUInt(GetExpiringTime());               // Expiring time (1992027391 -  never expires)
                    packet.WriteUInt(0);               // Time? (1992027391 -  never expires)
                    break;
                case ItemSlot.Earings:
                case ItemSlot.Necklace:
                case ItemSlot.Ring:
                    packet.WriteUInt(GetExpiringTime());               // Expiring time (1992027391 -  never expires)
                    packet.WriteUInt(0);               // Time? (1992027391 -  never expires)
                    packet.WriteByte(this.Upgrades);   // Refinement
                    // Stats added while refining
                    packet.WriteUShort(0);             // it may be byte + byte too (some kind of counter when item downgrades)
                    packet.WriteUShort(0);             // STR
                    packet.WriteUShort(0);             // END
                    packet.WriteUShort(0);             // DEX
                    packet.WriteUShort(0);             // INT
                    packet.WriteUShort(0);             // SPR
                    break;
                case ItemSlot.CostumeWeapon:
                case ItemSlot.CostumeShield:
                    packet.WriteUInt(25000);           // Skin Durability
                    break;
                default:
                    packet.WriteUInt(GetExpiringTime());      // Expiring time (1992027391 -  never expires)
                    packet.WriteUInt(0);                        // Time? (1992027391 -  never expires)
                    break;
            }

            // Random stats data (Not those what were added in refinement)
            switch (this.SlotType)
            {                       // Stat count (StatCount << 1 | Visible(0 or 1 are stats shown or not))
                case ItemSlot.Earings:
                    packet.WriteByte((byte)(statCount << 1 | 1));
                    break;
                case ItemSlot.Necklace:
                case ItemSlot.Ring:
                    packet.WriteByte((byte)(statCount << 1 | 1));
                    break;
                case ItemSlot.Helm:
                case ItemSlot.Armor:
                case ItemSlot.Pants:
                case ItemSlot.Boots:
                case ItemSlot.Weapon2:
                case ItemSlot.Weapon:
                    packet.WriteByte((byte)(statCount << 1 | 1));
                    break;
                case ItemSlot.Pet:          // Yes!! Its possible to give stats to pet also (It overrides default one(s)).
                    packet.WriteByte((byte)(statCount << 1 | 1));
                    break;
            }
            // foreach stat
            //pPacket.WriteByte(type);              // Stat type ( 0 = STR, 1 = END, 2 = DEX, 3 = INT, 4 = SPR )
            //pPacket.WriteUShort(amount);          // Amount
            // end foreach
            if (Str > 0) { packet.WriteByte(0); packet.WriteUShort(Str); }
            if (End > 0) { packet.WriteByte(1); packet.WriteUShort(End); }
            if (Dex > 0) { packet.WriteByte(2); packet.WriteUShort(Dex); }
            if (Spr > 0) { packet.WriteByte(3); packet.WriteUShort(Spr); }
            if (Int > 0) { packet.WriteByte(4); packet.WriteUShort(Int); }
        }


        public ItemInfo GetInfo()
        {
            ItemInfo itemInfo;
            DataProvider.GetItemInfo(this.ID, out itemInfo);
            return itemInfo;
        }

        public static Equip LoadEquip(DataRow row)
        {
            ulong uniqueID = GetDataTypes.GetUlong(row["ID"]);
            uint owner = GetDataTypes.GetUint(row["Owner"]);
            ushort equipID = GetDataTypes.GetUshort(row["EquipID"]);
            short slot = GetDataTypes.Getshort(row["Slot"]);
            byte upgrade = GetDataTypes.GetByte(row["Upgrades"]);
            // TS: What's the "i" stand for? It's not very pretty :<
            ushort strByte = GetDataTypes.GetUshort(row["iSTR"]);
            ushort endByte = GetDataTypes.GetUshort(row["iEND"]);
            ushort dexByte = GetDataTypes.GetUshort(row["iDEX"]);
            ushort sprByte = GetDataTypes.GetUshort(row["iSPR"]);
            ushort intByte = GetDataTypes.GetUshort(row["iINT"]);
            Equip equip = new Equip(owner, equipID, slot)
            {
                UniqueID = uniqueID,
                Upgrades = upgrade,
                Str = strByte,
                End = endByte,
                Dex = dexByte,
                Spr = sprByte,
                Int = intByte
            };
            return equip;
        }
    }
}
