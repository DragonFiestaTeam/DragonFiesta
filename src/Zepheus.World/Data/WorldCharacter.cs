using System;
using System.Collections.Generic;
using System.Linq;
using Zepheus.Database;
using Zepheus.FiestaLib;
using Zepheus.Util;
using Zepheus.Database.Storage;
using System.IO;

namespace Zepheus.World.Data
{
    public class WorldCharacter
    {
        public Character Character { get; set;  }
       // public Character Character { get { return _character ?? (_character = LazyLoadMe()); } set { _character = value; } }
        public int ID { get; private set; }
        public Dictionary<byte, ushort> Equips { get; private set; }
        public bool IsDeleted { get; private set; }
        //party
        public List<string> Party = new List<string>();
        public bool IsPartyMaster { get; set;  }
    	public Group Group { get; private set; }

        public WorldCharacter(Character ch)
        {
            Character = ch;
      
            ID = Character.ID;
            Equips = new Dictionary<byte, ushort>();
           
            foreach (var eqp in ch.EquiptetItem.Where(eq => eq.Slot < 0))
            {
                byte realslot = (byte)(eqp.Slot * -1);
                if (Equips.ContainsKey(realslot))
                {
                    Log.WriteLine(LogLevel.Warn, "{0} has duplicate equip in slot {1}", ch.Name, realslot);
                    Equips.Remove(realslot);
                }
                Equips.Add(realslot, (ushort)eqp.EquipID);
            }
        }

        private Character LazyLoadMe()
        {
          //  return Program.Entity.Characters.First(c => c.ID == ID);
            return this.Character;
        }

        public void Detach()
        {
            try
            {
               
                //Program.Entity.Detach(Character);
                Character = null;
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, "Error detaching character from entity: {0}.", ex.ToString());
            }
        }

        public bool Delete()
        {
            if (IsDeleted) return false;
            try
            {
                Program.DatabaseManager.GetClient().ExecuteQuery("DELETE FROM characters WHERE CharID='" + this.Character.ID + "'");
             
                IsDeleted = true;
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, "Error deleting character: {0}", ex.ToString());
                return false;
            }
        }

        public WorldCharacter(Character ch, byte eqpslot, ushort eqpid)
        {
            Character = ch;
            ID = Character.ID;
            Equips = new Dictionary<byte, ushort>();
            Equips.Add(eqpslot, eqpid);
        }

        public ushort GetEquipBySlot(ItemSlot slot)
        {
            if (Equips.ContainsKey((byte)slot))
            {
                return Equips[(byte)slot];
            }
            else
            {
                return ushort.MaxValue;
            }
        }
        public static string ByteArrayToStringForBlobSave(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", ",");
        }
        public void SetQuickBarData(byte[] pData)
        {
            Character.QuickBar = pData;
            Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Characters SET QuickBar='" + ByteArrayToStringForBlobSave(Character.QuickBar) ?? new byte[] { 0x00 } + "' WHERE CharID='" + Character.ID + "';");
        }
        public void SetQuickBarStateData(byte[] pData)
        {
            Character.QuickBarState = pData;
             Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Characters SET QuickBarState='" + ByteArrayToStringForBlobSave(Character.QuickBarState) ?? new byte[] { 0x00 } + "' WHERE CharID='" + Character.ID + "';");
        }

        public void SetGameSettingsData(byte[] pData)
        {
            Character.GameSettings = pData;
             Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Characters SET GameSettings='" + ByteArrayToStringForBlobSave(Character.GameSettings) ?? new byte[] { 0x00 } + "' WHERE CharID='" + Character.ID + "';");
        }

        public void SetClientSettingsData(byte[] pData)
        {
            Character.ClientSettings = pData;
             Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Characters SET ClientSettings='" + ByteArrayToStringForBlobSave(Character.ClientSettings) ?? new byte[] { 0x00 } + "' WHERE CharID='" + Character.ID + "';");
        }

        public void SetShortcutsData(byte[] pData)
        {
            Character.Shortcuts = pData;
             Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Characters SET Shortcuts='" + ByteArrayToStringForBlobSave(Character.Shortcuts) ?? new byte[] { 0x00 } + "' WHERE CharID='" + Character.ID + "';");
        }
    }
}
