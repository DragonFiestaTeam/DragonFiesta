using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.InterLib.Networking;
using Zepheus.World.InterServer;
using Zepheus.World.Networking;

namespace Zepheus.World.Data
{
	public class Group
	{
		#region .ctor

		public Group(long id)
		{
			this.members = new List<GroupMember>();
			this.openRequests = new List<GroupRequest>();
			this.Id = id;
			this.members = new List<GroupMember>();
			this.DropState = DropState.FreeForAll;
			this.gotLastDrop = 0;
		}

		#endregion
		#region Properties

		public const int MaxMembers = 5;
		private readonly List<GroupMember> members;
		private readonly List<GroupRequest> openRequests;
		public GroupMember this[string pName]
		{
			get
			{
				return this.members.Single(m => m.Name == pName);
			}
		}
		public GroupMember Master { get { return members.Single(m => m.Role == GroupRole.Master); }}
		public IEnumerable<GroupMember> NormalMembers { get { return from m in members where m.Role != GroupRole.Master select m; }}
		public DropState DropState { get; private set; }
		public long Id { get; private set; }
		public bool Exists { get; private set; }

		private int gotLastDrop;
		#endregion
		#region Methods

		#region Public
		public bool HasMember(string pName)
		{
			return this.members.Any(m => m.Name == pName);
		}
		public bool IsFull()
		{
			return members.Count() >= MaxMembers;
		}
		public void InviteNewMember(WorldCharacter pSender, string pTarget)
		{
			if (!ClientManager.Instance.IsOnline(pTarget))
				return;
			if(Master.Name != pSender.Character.Name)
				return;		// only the master may invite new members
			
			GroupManager.Instance.Invite(pSender, pTarget); // trololol
		}
		public void ChangeDropType(WorldCharacter pBy, byte pDropState)
		{
			if(pBy.Character.Name != Master.Name)
				return;		// only the master may change drop state!
			this.DropState = (DropState) pDropState;

			UpdateDropStateToMembers();
		}
		public void BreakUp()
		{
			this.Exists = false;
			GroupMember[] mems = new GroupMember[this.members.Count];
			this.members.CopyTo(mems);
			BreakUpInDatabase();
			OnBrokeUp();
		}
		public void ChangeMaster(GroupMember pNewMaster)
		{
			Master.Role = GroupRole.Member;
			pNewMaster.Role = GroupRole.Master;

			AnnounceChangeMaster();
		}
		public bool HasOpenRequestFor(string pName)
		{
			return openRequests.Any(r => r.InvitedClient.Character.Character.Name == pName);
		}
		public void MemberLeaves(WorldClient pClient)
		{
			var otherMembers = from m in members
							   where m.Name != pClient.Character.Character.Name
							   select m.Client;
			if (pClient.Character.GroupMember.Role == GroupRole.Master)
				ChangeMaster(otherMembers.First().Character.GroupMember);
			SendMemberLeavesPacket(pClient.Character.Character.Name, otherMembers);
			UpdateInDatabase();
		}
		public void KickMember(string pMember)
		{
			var otherMembers = from m in members
							   where m.Name != pMember
							   select m.Client;
			
			SendMemberLeavesPacket(pMember, otherMembers);
			UpdateInDatabase();
		}
		public void MemberJoin(string pMember)
		{
			WorldClient client = ClientManager.Instance.GetClientByCharname(pMember);
			GroupMember gMember = new GroupMember(client, GroupRole.Member);
			AddMember(gMember);

			AnnouncePartyList();
			UpdateInDatabase();
		}
		public void AnnouncePartyList()
		{
			using (var packet = new Packet(SH14Type.PartyList))
			{
				packet.WriteByte((byte)members.Count);
				foreach (var groupMember in members)
				{
					packet.WriteString(groupMember.Name, 16);
					packet.WriteBool(groupMember.IsOnline);
				}

				AnnouncePacket(packet);
			}
		}
		public void Chat(WorldClient pFrom, string pMessage)
		{
			using (var packet = new Packet(SH8Type.PartyChat))
			{
				packet.WriteString(pFrom.Character.Character.Name, 16);
				packet.WriteByte((byte) pMessage.Length);
				packet.WriteString(pMessage);

				AnnouncePacket(packet);
			}
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Group))
				return false;
			var grp = (Group)obj;
			return grp.Id == this.Id;
		}
		public bool Equals(Group other)
		{
			return other.Id.Equals(this.Id);
		}
		public override int GetHashCode()
		{
			return 0;
		}

		internal void AddMember(GroupMember pMember)
		{
			this.members.Add(pMember);
			pMember.Group = this;
			SendAddMemberInterPacket(pMember);
		}
		internal void AddInvite(GroupRequest pRequest)
		{
			this.openRequests.Add(pRequest);
		}
		internal void RemoveMember(GroupMember pMember)
		{
			this.members.Remove(pMember);
			pMember.Character.Group = null;
			pMember.Character.GroupMember = null;
			// TEMP
			KickMember(pMember);
			// NOTE: Send packet to other members to update GroupList!
			AnnouncePartyList();
		}
		internal void RemoveInvite(GroupRequest pRequest)
		{
			this.openRequests.Remove(pRequest);
		}
		internal void UpdateInDatabase()
		{
			UpdateGroupTableInDatabase();
			UpdateMembersInDatabase();
		}
		internal void UpdateGroupTableInDatabase()
		{
			//--------------------------------------------------
			// Queries used in this function
			//--------------------------------------------------

			const string update_group_table_query = 
				"UPDATE `groups` " +
				"SET " +
					"`Member1` = {1} ," +
					"`Member2` = {2} ," +
					"`Member3` = {3} ," +
					"`Member4` = {4} ," +
					"`Member5` = {5} " +
				"WHERE `Id` = {0}";

			//--------------------------------------------------
			// Update table
			//--------------------------------------------------

			using (var client = Program.DatabaseManager.GetClient())
			{
				string query = string.Format(update_group_table_query,
								this.Id,
								this.members[0],
								this.members[1],
								this.members.Count >= 3 : this.members[2] ? "NULL",
								this.members.Count >= 4 : this.members[3] ? "NULL",
								this.members.Count >= 5 : this.members[4] ? "NULL");
				client.ExecuteQuery(query);
			}
		}
		internal void UpdateMembersInDatabase()
		{
			//--------------------------------------------------
			// Queries used in this function
			//--------------------------------------------------
			const string update_character_table_query =
				"UPDATE `characters` " +
				"SET " +
					"`GroupID` = {1} ," +
					"`IsGroupMaster` = {2} " +
				"WHERE `CharID` = {0}";

			//--------------------------------------------------
			// Update table
			//--------------------------------------------------
			using (var client = Program.DatabaseManager.GetClient())
			{
				foreach (var member in this.members)
				{
					string query = string.Format(update_character_table_query,
								member.Character.ID,
								this.Id,
								member.Character.GroupMember.Role == GroupRole.Master);
					client.ExecuteQuery(query);
				}
			}
		}
		internal void CreateInDatabase()
		{
			//--------------------------------------------------
			// Queries used in this function
			//--------------------------------------------------

			const string create_group_query =
				"INSERT INTO `groups` " +
					"(`Id`, `Member1`, `Member2`, `Member3`, `Member4`, `Member5`) " +
				"VALUES " +
					"({0}, {1}, {2}, {3}, {4}, {5})";
			//--------------------------------------------------
			// create entry in table
			//--------------------------------------------------
			using (var client = Program.DatabaseManager.GetClient())
			{
				string query = string.Format(create_group_query,
								this.Id,
								this.members.Count > 0 : this.members[0] ? "NULL",
								this.members.Count > 1 : this.members[1] ? "NULL",
								this.members.Count > 2 : this.members[2] ? "NULL",
								this.members.Count > 3 : this.members[3] ? "NULL",
								this.members.Count > 4 : this.members[4] ? "NULL");
				client.ExecuteQuery(query);
			}
			// keep character also up to date
			UpdateMembersInDatabase();
		}
		internal static Group ReadFromDatabase(long pId)
		{
			const string query = "SELECT * FROM `groups` WHERE Id = @gid";
			Group g = new Group(pId);
			
			using(var con = Program.DatabaseManager.GetClient())
			using(var cmd = new MySqlCommand(query, con.Connection))
			{
				cmd.Parameters.AddWithValue("@gid", pId);
				using (var rdr = cmd.ExecuteReader())
				{
					while (rdr.Read())
					{
						for (int i = 1; i < 6; i++)
						{
							UInt16 mem = rdr.GetUInt16(string.Format("Member{0}", i));
							if(mem != null)
								g.members.Add(GroupMember.LoadFromDatabase(mem));
						}
					}
				}
			}

			return g;
		}
		#endregion
		#region Private
		private void UpdateDropStateToMembers()
		{
			using (var packet = new Packet(SH14Type.PartyDropState))
			{
				packet.WriteByte((byte) DropState);

				foreach (var m in members)
				{
					m.Client.SendPacket(packet);
				}
			}
		}
		private void SendMemberLeavesPacket(string pLeaver, IEnumerable<WorldClient> pOthers)
		{
			bool isOnline = ClientManager.Instance.IsOnline(pLeaver);
			WorldClient client = isOnline ? ClientManager.Instance.GetClientByCharname(pLeaver) : null;
			using (var packet = new Packet(SH14Type.KickPartyMember))
			{
				packet.WriteString(pLeaver, 16);
				packet.WriteUShort(1345);

				foreach (var other in pOthers)
				{
					other.SendPacket(packet);
				}
				if (isOnline)
					client.SendPacket(packet);
			}
			if (isOnline)
			{
				ZoneConnection z = Program.GetZoneByMap(client.Character.Character.PositionInfo.Map);
				using (var interleave = new InterPacket(InterHeader.RemovePartyMember))
				{
					interleave.WriteString(client.Character.Character.Name, 16);
					interleave.WriteString(client.Character.Character.Name, 16);
					z.SendPacket(interleave);
				}
			}
		}
		private void DeleteGroupByNameInDatabase(string pName)
		{
			DatabaseHelper.RemoveCharacterGroup(pName);
		}
		private void AnnounceChangeMaster()
		{
			using (var packet = new Packet(SH14Type.ChangePartyMaster))
			{
				packet.WriteString(Master.Name, 16);
				packet.WriteUShort(1352);

				// Send to all online members
				members.ForEach(m => { if (m.IsOnline) m.Client.SendPacket(packet); });
			}
		}
		private void SendAddMemberInterPacket(GroupMember pMember)
		{
			ZoneConnection con = Program.GetZoneByMap(pMember.Character.Character.PositionInfo.Map);
			using (var pack = new InterPacket(InterHeader.AddPartyMember))
			{
				pack.WriteString(this.Master.Name, 16);
				pack.WriteString(pMember.Name, 16);
				con.SendPacket(pack);
			}
		}
		private void AnnouncePacket(Packet pPacket)
		{
			foreach (var grpMem in members.Where(m => m.IsOnline))
			{
				grpMem.Client.SendPacket(pPacket);
			}
		}
		private void BreakUpInDatabase()
		{
			//--------------------------------------------------
			// Queries used in function
			//--------------------------------------------------

			const string break_group_query =
				"UPDATE `groups` " +
				"SET `Exists` = 0 " +
				"WHERE `Id` = {0}";

			const string reset_char_group_query = 
				"UPDATE `character` "+
				"SET `GroupID` = NULL, "+
					"`IsMaster = NULL "+
				"WHERE `GroupId` = {0}";

			//--------------------------------------------------
			// Execute queries
			//--------------------------------------------------

			using(var client = Program.DatabaseManager.GetClient())
			{
				string query = string.Format(break_group_query, this.Id)				
				client.ExecuteQuery(query);

				string query = string.Format(reset_char_group_query, this.Id);
				client.ExecuteQuery(query);
			}

		}
		#endregion		
		#region EventExecuter
		protected virtual void OnBrokeUp()
		{
			if(BrokeUp != null)
				BrokeUp(this, new EventArgs());
		}
		#endregion
		#region EventHandler

		#endregion

		#endregion
		#region Events

		public event EventHandler BrokeUp;

		#endregion
	}
}
