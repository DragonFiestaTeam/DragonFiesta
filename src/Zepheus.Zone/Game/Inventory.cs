using System.Collections.Generic;
using Zepheus.FiestaLib.Data;
using Zepheus.FiestaLib;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Data;
using Zepheus.Database;

namespace Zepheus.Zone.Game
{
    public sealed class Inventory
    {
        public long Money { get; set; }
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
            try
            {
                locker.ReleaseMutex();
            }
            catch { }
        }

        public void LoadFull(ZoneCharacter pChar)
        {
            try
            {
                locker.WaitOne();
                DataTable eq = null;
                DataTable items = null;
                DataTable Equipped = null;
                  using (DatabaseClient dbClient = Program.CharDBManager.GetClient())
                {
                    items = dbClient.ReadDataTable("SELECT * FROM items WHERE Owner=" + pChar.ID + "");
                    eq = dbClient.ReadDataTable("SELECT * FROM equips WHERE Owner="+pChar.ID+" AND Slot >= 0");
                    Equipped = dbClient.ReadDataTable("SELECT * FROM equips WHERE Owner=" + pChar.ID + " AND Slot < 0");
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
                  //we load all equippeditem

                  if (Equipped != null)
                  {
                      foreach (DataRow row in Equipped.Rows)
                      {
                          Equip loaded = Equip.LoadEquip(row);
                          loaded.IsEquipped = true;
                          loaded.Owner = (uint)pChar.ID;
                          this.EquippedItems.Add(loaded);
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
        public ushort GetEquippedBySlot(ItemSlot pSlot)
        {
            //double check if found
            Equip equip = EquippedItems.Find(d => d.SlotType == pSlot && d.IsEquipped);
            if (equip == null)
            {
                return 0xffff;
            }
            else
            {
                return equip.ID;
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
