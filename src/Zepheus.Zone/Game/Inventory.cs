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
                  using (DatabaseClient dbClient = Program.CharDBManager.GetClient())
                {
                    items = dbClient.ReadDataTable("SELECT * FROM items WHERE Owner=" + pChar.ID + "");
                    eq = dbClient.ReadDataTable("SELECT * FROM equips WHERE Owner="+pChar.ID+" AND Slot >= 0");
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
        public Equip GetEquiptBySlot(byte slot, out Equip Eq)
        {

            Eq = this.EquippedItems.Find(d => d.Slot == slot);
            return Eq;
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
