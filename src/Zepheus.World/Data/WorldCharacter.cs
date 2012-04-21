using System;
using System.Collections.Generic;
using System.Linq;
using Zepheus.FiestaLib;
using Zepheus.Util;
using Zepheus.Database.Storage;
using System.Data;
using Zepheus.Database;
using Zepheus.World.Networking;
using Zepheus.FiestaLib.Networking;
using Zepheus.FiestaLib.Data;

namespace Zepheus.World.Data
{
	public class WorldCharacter
	{
		public Character Character { get; set;  }
		public WorldClient Client { get; set; }
	//	public Character Character { get { return _character ?? (_character = LazyLoadMe()); } set { _character = value; } }
		public int ID { get; private set; }
		public Dictionary<byte, ushort> Equips { get; private set; }
		public bool IsDeleted { get; private set; }
		public bool IsIngame { get;  set; }
		public bool IsPartyMaster { get; set;  }
		public Group Group { get; internal set; }
		public long GroupId {get; internal set;}
		public GroupMember GroupMember { get; internal set; }
		private List<Friend> friends;
		private List<Friend> friendsby;
        public Inventory Inventory = new Inventory();

		public WorldCharacter(Character ch)
		{
			Character = ch;
			Client = ClientManager.Instance.GetClientByCharname(ch.Name);
			ID = Character.ID;
			Equips = new Dictionary<byte, ushort>();
            Inventory.LoadBasic(this);
            LoadEqupippet();
		   // LoadGroup();
		}
		public List<Friend> Friends
		{
			get
			{
				if (this.friends == null)
				{
					LoadFriends(ClientManager.Instance.GetClientByCharname(this.Character.Name));
				}
				return this.friends;
			}
		}
		public void LoadFriends(WorldClient c)
		{
		   
			this.friends = new List<Friend>();
			this.friendsby = new List<Friend>();
			DataTable frenddata = null;
			DataTable frenddataby = null;
			using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
			{
				frenddata = dbClient.ReadDataTable("SELECT * FROM friends WHERE CharID='" + this.ID + "'");
				frenddataby = dbClient.ReadDataTable("SELECT * FROM friends WHERE FriendID='" + this.ID + "'");
			}

			if (frenddata != null)
			{
				foreach (DataRow row in frenddata.Rows)
				{
					this.friends.Add(Friend.LoadFromDatabase(row));
				}
			}
			if (frenddataby != null)
			{
				foreach (DataRow row in frenddata.Rows)
				{
					this.friendsby.Add(Friend.LoadFromDatabase(row));
				}
			}
			foreach (var friend in this.friendsby)
			{
				DataTable frendsdata = null;
				using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
				{
					frendsdata = dbClient.ReadDataTable("SELECT * FROM Characters WHERE CharID='" + friend.ID + "'");
				}
				if (frenddata != null)
				{
					foreach (DataRow row in frendsdata.Rows)
					{
						friend.UpdateFromDatabase(row);
					}
				}
			}
			foreach (var friend in this.Friends)
			{
				DataTable frendsdata = null;
				using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
				{
					frendsdata = dbClient.ReadDataTable("SELECT * FROM Characters WHERE CharID='" + friend.ID + "'");
				}
				if (frenddata != null)
				{
					foreach (DataRow row in frendsdata.Rows)
					{
						friend.UpdateFromDatabase(row);
					}
				}
			}
			UpdateFriendStates(c);
		}
		public void ChangeMap(string mapname)
		{
			foreach (var friend in friends)
			{
				WorldClient client = ClientManager.Instance.GetClientByCharname(friend.Name);
				if (client == null) return;
				using (var packet = new Packet(SH21Type.FriendChangeMap))
				{
					packet.WriteString(this.Character.Name, 16);
					packet.WriteString(mapname, 12);
					client.SendPacket(packet);
				}
			}
		}

        public void LoadEqupippet()
        {
            foreach (var eqp in this.Inventory.EquippedItems.Where(eq => eq.Slot < 0))
            {
                byte realslot = (byte)(eqp.Slot * -1);
                if (Equips.ContainsKey(realslot))
                {
                    Log.WriteLine(LogLevel.Warn, "{0} has duplicate equip in slot {1}", eqp.EquipID, realslot);
                    Equips.Remove(realslot);
                }
                Equips.Add(realslot, (ushort)eqp.EquipID);
            }
        }
		public string GetMapname(ushort mapid)
		{
			MapInfo mapinfo;
			if (DataProvider.Instance.Maps.TryGetValue(mapid, out mapinfo))
			{
				return mapinfo.ShortName;
			}
			return "";
		}
		public Friend AddFriend(WorldCharacter pChar)
		{

			Friend pFrend = pChar.friends.Find(f => f.Name == pChar.Character.Name);
			Friend pFrendby = pChar.friendsby.Find(f => f.Name == pChar.Character.Name);
			Friend friend = Friend.Create(pChar);
			if (pFrend != null)
			{
				Program.DatabaseManager.GetClient().ExecuteQuery("INSERT INTO Friends (CharID,FriendID,Pending) VALUES ('" + pChar.Character.ID + "','" + this.Character.ID + "','1')");

			}
			if (pFrendby == null)
			{
				this.friendsby.Add(friend);
			}
			Program.DatabaseManager.GetClient().ExecuteQuery("INSERT INTO Friends (CharID,FriendID) VALUES ('" + this.Character.ID + "','" + pChar.Character.ID + "')");
			friends.Add(friend);

			return friend;
		}
		public bool DeleteFriend(string pName)
		{
			Friend friend = this.friends.Find(f => f.Name == pName);
			Friend friendby = this.friendsby.Find(f => f.Name == pName);
			if (friend != null)
			{
				bool result = this.friends.Remove(friend);
				if (result)
				{
					if (friendsby != null)
					{
						Program.DatabaseManager.GetClient().ExecuteQuery("DELETE FROM friends WHERE CharID=" + friend.ID + " AND FriendID=" + this.ID);
						this.friendsby.Remove(friendby);
					}
					Program.DatabaseManager.GetClient().ExecuteQuery("DELETE FROM friends WHERE CharID=" + this.ID + " AND FriendID=" + friend.ID);
				}
				UpdateFriendStates(friend.client);
				return result;
			}
			return false;
		}
		public void UpdateFriendsStatus(bool state, WorldClient sender)
		{
			foreach (Friend frend in friendsby)
			{
				WorldClient client = ClientManager.Instance.GetClientByCharname(frend.Name);
				if (client != null)
				{
					if (state)
					{
						if (client != sender && !client.Character.IsIngame)
							frend.Online(client, sender);
					}
					else
					{
						frend.Offline(client, this.Character.Name);
					}
				}
			}
		}
		public void UpdateFriendStates(WorldClient pclient)
		{
			List<Friend> unknowns = new List<Friend>();
			foreach (var friend in this.Friends)
			{
				if (friend.Name == null)
				{
					unknowns.Add(friend);
					continue;
				}
				WorldClient friendCharacter = ClientManager.Instance.GetClientByCharname(friend.Name);
				if (friendCharacter != null)
				{
					friend.Update(friendCharacter.Character);
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
		public void Loggeout(WorldClient pChar)
		{
			this.IsIngame = false;
			this.UpdateFriendsStatus(false,pChar);
			this.UpdateFriendStates(pChar);
			
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
			string data = ByteArrayToStringForBlobSave(Character.QuickBar) ?? ByteArrayToStringForBlobSave(new byte[1024]);
			Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Characters SET QuickBar='" +data+ "' WHERE CharID='" + Character.ID + "';");
		}
		public void SetQuickBarStateData(byte[] pData)
		{
			Character.QuickBarState = pData;
			string data = ByteArrayToStringForBlobSave(Character.QuickBarState) ?? ByteArrayToStringForBlobSave(new byte[24]);
			 Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Characters SET QuickBarState='"+data+"' WHERE CharID='" + Character.ID + "'");
		}
		public void SetGameSettingsData(byte[] pData)
		{
			Character.GameSettings = pData;
			string data =  ByteArrayToStringForBlobSave(Character.GameSettings) ?? ByteArrayToStringForBlobSave(new byte[64]);
			Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Characters SET GameSettings='" + data + "' WHERE CharID='" + Character.ID + "';");
		}
		public void SetClientSettingsData(byte[] pData)
		{
			Character.ClientSettings = pData;
			string data = ByteArrayToStringForBlobSave(Character.ClientSettings) ?? ByteArrayToStringForBlobSave(new byte[392]);
			 Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Characters SET ClientSettings='"+data + "' WHERE CharID='" + Character.ID + "';");
		}
		public void SetShortcutsData(byte[] pData)
		{
			Character.Shortcuts = pData;
			string data = ByteArrayToStringForBlobSave(Character.Shortcuts) ?? ByteArrayToStringForBlobSave(new byte[308]);
			 Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Characters SET Shortcuts='" + data+ "' WHERE CharID='" + Character.ID + "';");
		}

		private void UpdateGroupStatus()
		{
			this.GroupMember.IsOnline = false;
			this.Group.AnnouncePartyList();
		}
		private void LoadGroup()
		{
			this.Group = GroupManager.Instance.GetGroupById(this.Character.GroupId);
			this.GroupMember = this.Group[this.Character.Name];
			this.UpdateGroupStatus();
		}
	}
}
