using Zepheus.Util;
using Zepheus.Database;
using System.Collections.Generic;
using System.Data;
using Zepheus.Zone.Data;
using System.Collections;
namespace Zepheus.Zone.Game
{
    public sealed class GuildStorage
    {
        public Dictionary<byte,Item> GuildStorageItems { get; private set; }
        public GuildStorage(int GuildID)
        {
            GuildStorageItems = new Dictionary<byte, Item>();
          LoadGuildStorageFromDatabase(GuildID);
        }
        private void LoadGuildStorageFromDatabase(int GuildID)
        {
            DataTable GuildItemData = null;
            using (DatabaseClient DBClient = Program.CharDBManager.GetClient())
            {
                GuildItemData = DBClient.ReadDataTable("SELECT * FROM GuildStorage WHERE GuildID=" + GuildID + "");
            }
            if (GuildItemData != null)
            {
                foreach(DataRow row in GuildItemData.Rows)
                {
                    ushort ItemID = System.Convert.ToUInt16(row["ItemID"]);
                    ushort Amount = System.Convert.ToUInt16(row["Amount"]);
                    byte pSlot = System.Convert.ToByte(row["Slot"]);
                    Item pItem = new Item(GuildID,ItemID, pSlot, Amount);
                    this.GuildStorageItems.Add(pSlot,pItem);
                }
            }
        }
    }
}
