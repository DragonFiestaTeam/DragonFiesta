using System;
using System.Collections.Generic;
using System.Linq;
using Zepheus.FiestaLib;
using Zepheus.Util;
using Zepheus.Database.Storage;
using System.Data;
using Zepheus.Database;
using Zepheus.FiestaLib.Networking;

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
		public Group Group { get; internal set; }
		public GroupMember GroupMember { get; internal set; }
        private List<Friend> friends;
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
        public List<Friend> Friends
        {
            get
            {
                if (this.friends == null)
                {
                    LoadFriends();
                }
                return this.friends;
            }
        }

        private void LoadFriends()
        {
            this.friends = new List<Friend>();
                   DataTable frenddata = null;
            using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
            {
                frenddata = dbClient.ReadDataTable("SELECT * FROM friends WHERE CharID='" + this.ID + "'");
            }

            if (frenddata != null)
            {
                foreach (DataRow Row in frenddata.Rows)
                {
                    this.friends.Add(Friend.LoadFromDatabase(Row));
                }
            }
            foreach (var friend in this.Friends)
            {
                DataTable frendsdata = null;
                using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
                {
                    frendsdata = dbClient.ReadDataTable("SELECT * FROM friends WHERE CharID='" + friend.ID + "'");
                }
                if (frenddata != null)
                {
                    foreach (DataRow Row in frenddata.Rows)
                    {
                        friend.UpdateFromDatabase(Row);
                    }
                }
            }
            UpdateFriendStates();
        }
        public Friend AddFriend(WorldCharacter pChar)
        {
            Friend friend = Friend.Create(pChar);
            friends.Add(friend);
            return friend;
        }
        public bool DeleteFriend(WorldCharacter pChar)
        {
            Friend friend = this.friends.Find(f => f.Name == pChar.Character.Name);
            bool result = this.friends.Remove(friend);
            if (result)
            {
                Program.DatabaseManager.GetClient().ExecuteQuery("DELETE FROM friends WHERE CharID=" + this.ID + " AND FriendID=" + pChar.ID);
            }
            return result;
        }
        public bool DeleteFriend(string pName)
        {
            Friend friend = this.friends.Find(f => f.Name == pName);
            if (friend != null)
            {
                bool result = this.friends.Remove(friend);
                if (result)
                {
                    Program.DatabaseManager.GetClient().ExecuteQuery("DELETE FROM friends WHERE CharID="+this.ID+" AND FriendID="+friend.ID);
                }
                return result;
            }
            return false;
        }
        public void UpdateFriendStates()
        {
            List<Friend> unknowns = new List<Friend>();
            foreach (var friend in this.Friends)
            {
                if (friend.Name == null)
                {
                    unknowns.Add(friend);
                    continue;
                }

                WorldCharacter friendCharacter = ClientManager.Instance.GetClientByCharname(friend.Name).Character;
                if (friendCharacter != null)
                {
                    friend.Update(friendCharacter);
                }
                else
                {
                    friend.IsOnline = false;
                }
            }
            foreach (var friend in unknowns)
            {
                this.Friends.Remove(friend);
            }
            unknowns.Clear();
        }
        
        public void WriteFriendData(Packet pPacket)
        {
            foreach (var friend in this.Friends)
            {
                friend.WritePacket(pPacket);
            }
        }
		public void RemoveGroup()
		{
			this.Group = null;
			this.GroupMember = null;

			string query = string.Format(
				"UDPATE characters SET GroupID = NULL WHERE CharID = {0}", this.ID);
			Program.DatabaseManager.GetClient().ExecuteQuery(query);
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
