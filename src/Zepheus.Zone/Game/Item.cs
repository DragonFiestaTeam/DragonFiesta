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
        public Mount Mount { get; set; }

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
        public void WriteItemInfo(Packet pPacket)
        {
            pPacket.WriteByte(5);//lenght
            pPacket.WriteByte((byte)this.Slot);
            pPacket.WriteByte(0x24);//status
            WriteItemStats(pPacket);
        }

        public void WriteItemStats(Packet pPacket)
        {
            pPacket.WriteUShort(this.ID);
            pPacket.WriteByte((byte)this.Count);
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