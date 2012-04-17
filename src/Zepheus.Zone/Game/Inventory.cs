using System.Collections.Generic;
using Zepheus.FiestaLib.Data;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Data;
using Zepheus.Database;

namespace Zepheus.Zone.Game
{
    public sealed class Inventory
    {
        private const string SelectEquippedIdByOwner =
            "SELECT * FROM equips WHERE Owner=@ownerID AND Slot < 0";

        public ulong Money { get; set; }
        public List<Equip> EquippedItems { get; private set; }
        public Dictionary<byte, Item> InventoryItems { get; private set; }
        public byte InventoryCount { get; private set; }
        private Mutex locker = new Mutex();

        public Inventory()
        {
            InventoryCount = 2;
            InventoryItems = new Dictionary<byte, Item>();
            EquippedItems = new List<Equip>();
        }

        public void Enter()
        {
            locker.WaitOne();
        }

        public void Release()
        {
            locker.ReleaseMutex();
        }

        public void LoadFull(ZoneCharacter pChar)
        {
            try
            {
                locker.WaitOne();
                DataTable eq = null;
                DataTable items = null;
                  using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
                {
                    items = dbClient.ReadDataTable("SELECT * FROM items WHERE Owner=" + pChar.ID + "");
                    eq = dbClient.ReadDataTable("SELECT * FROM equips WHERE Owner="+pChar.ID+"ID AND Slot >= 0");
                }
                //we load unequipped equips (slot > 0)
                  if (eq != null)
                  {
                      foreach (DataRow row in eq.Rows)
                      {
                        Equip loaded = Equip.LoadEquip(row);
                        this.AddToInventory(loaded);
                    }
                }
                //we load inventory slots
                  if (items != null)
                  {
                      foreach (DataRow row in items.Rows)
                      {
                        Item loaded = Item.LoadItem(row);
                        this.AddToInventory(loaded);
                    }
                }
            }
            finally
            {
                locker.ReleaseMutex();
            }
        }

        public void LoadBasic(ZoneCharacter pChar)
        {
            try
            {
                locker.WaitOne();
                DataTable equips = null;
                using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
                {
                    equips = dbClient.ReadDataTable("SELECT * FROM equips WHERE Owner=" + pChar.ID + " AND Slot < 0");
                }
                //SELECT * FROM equips WHERE Owner=@ownerID AND Slot < 0
                if (equips != null)
                {
                    foreach (DataRow row in equips.Rows)
                    {
                        Equip loaded = Equip.LoadEquip(row);
                        EquippedItems.Add(loaded);
                    }
                }
            }
            finally
            {
                locker.ReleaseMutex();
            }

        }
        public ushort GetEquippedByType(Zepheus.FiestaLib.ItemSlot pType)
        {
            //double check if found
            Equip equip = EquippedItems.Find(d => d.SlotType == pType && d.IsEquipped);
            if (equip == null)
            {
                return 0xffff;
            }
            else
            {
                return (ushort)equip.ID;
            }
        }

        public byte GetEquippedUpgradesByType(Zepheus.FiestaLib.ItemSlot pType)
        {
            //double check if found

            Equip equip = EquippedItems.Find(d => d.SlotType == pType && d.IsEquipped);
            if (equip == null)
            {
                return 0;
            }
            else
            {
                return equip.Upgrades;
            }
        }

        public void AddToInventory(Item pItem)
        {
            try
            {
                locker.WaitOne();
                if (this.InventoryItems.ContainsKey((byte)pItem.Slot))
                {
                    this.InventoryItems[(byte)pItem.Slot].Delete(); //removes from DB
                    this.InventoryItems.Remove((byte)pItem.Slot);
                }
                this.InventoryItems.Add((byte)pItem.Slot, pItem);
            } finally {
                locker.ReleaseMutex();
            }
        }

        public void AddToEquipped(Equip pEquip)
        {
            try
            {
                locker.WaitOne();
                Equip old = EquippedItems.Find(equip => equip.Slot == pEquip.Slot);
                if (old != null)
                {
                    old.IsEquipped = false;
                    AddToInventory(old);
                    EquippedItems.Remove(old);
                }
                EquippedItems.Add(pEquip);
            }
            finally
            {
                locker.ReleaseMutex();
            }
        }

        public bool GetEmptySlot(out byte pSlot) //cpu intensive?
        {
            pSlot = 0;
            for (byte i = 0; i < (InventoryCount * 24); ++i)
            {
                if (!InventoryItems.ContainsKey(i))
                {
                    pSlot = i;
                    return true;
                }
            }
            return false; //no more empty slots found
        }
    }
}
