using System;
using System.Collections.Generic;
using System.Linq;
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
			this.Members = new List<GroupMember>();
			this.openRequests = new List<GroupRequest>();
			this.Id = id;
			this.Members = new List<GroupMember>();
			this.DropState = DropState.FreeForAll;
			this._gotLastDrop = 0;
		}
		#endregion

		#region Properties

		public const int MAX_MEMBERS = 5;
		private List<GroupMember> Members;
		private List<GroupRequest> openRequests;
		public GroupMember this[int index] { get { return Members[index]; }}
		public GroupMember Master { get { return Members.Single(m => m.Role == GroupRole.Master); }}
		public IEnumerable<GroupMember> NormalMembers { get { return from m in Members where m.Role != GroupRole.Master select m; }}
		public DropState DropState { get; private set; }
		public long Id { get; private set; }
		public bool Exists { get; private set; }

		private int _gotLastDrop;
		#endregion

		#region Methods

		#region Public
		public bool HasMember(string pName)
		{
			return this.Members.Any(m => m.Name == pName);
		}
		public bool IsFull()
		{
			return Members.Count() >= MAX_MEMBERS;
		}
		public void InviteNewMember(WorldCharacter pSender, string pTarget)
		{
			if (!ClientManager.Instance.IsOnline(pTarget))
			{
				// TODO: Send Message
				return;
			}
			if(Master.Name != pSender.Character.Name)
				return;		// only the master may invite new members

			WorldClient pTo = ClientManager.Instance.GetClientByCharname(pTarget);
			SendInvitationPacket(pTo);
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
			// TODO: Send to clients and update those in DB
			
			this.Exists = false;
			OnBrokeUp();
		}
		public void ChangeMaster(GroupMember pNewMaster)
		{
			Master.Role = GroupRole.Member;
			pNewMaster.Role = GroupRole.Master;

			AnnounceChangeMaster();
		}

		public override bool Equals(object obj)
		{
			if(!(obj is Group))
				return false;
			var grp = (Group) obj;
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
		public bool HasOpenRequestFor(string pName)
		{
			return openRequests.Any(r => r.InvitedClient.Character.Character.Name == pName);
		}
		public void MemberLeaves(WorldClient pClient)
		{
			var otherMembers = from m in Members
			                   where m.Name != pClient.Character.Character.Name
			                   select m.Client;
			if (pClient.Character.GroupMember.Role == GroupRole.Master)
				ChangeMaster(otherMembers.First().Character.GroupMember);
			SendMemberLeavesPacket(pClient.Character.Character.Name, otherMembers);
			UpdateInDatabase();
		}
		public void KickMember(string pMember)
		{
			GroupMember mem = this.Members.First(m => m.Name == pMember);
			var otherMembers = from m in Members
			                   where m.Name != pMember
			                   select m.Client;
			
			SendMemberLeavesPacket(pMember, otherMembers);
			UpdateInDatabase();
		}
		public void MemberJoin(string pMember)
		{
			WorldClient client = ClientManager.Instance.GetClientByCharname(pMember);
			GroupMember gMember = new GroupMember(client, GroupRole.Member);
			this.Members.Add(gMember);

			AnnouncePartyList();
			UpdateInDatabase();
		}

		internal void AddMember(GroupMember pMember)
		{
			this.Members.Add(pMember);
			pMember.Group = this;
		}
		internal void AddInvite(GroupRequest pRequest)
		{
			this.openRequests.Add(pRequest);
		}
		internal void RemoveMember(GroupMember pMember)
		{
			WorldClient leaver = ClientManager.Instance.GetClientByCharname(pMember.Name);
			this.Members.Remove(pMember);
			SendMemberLeavesPacket(pMember.Name, Members.Select(m => m.Client));

			ZoneConnection z =
				Program.GetZoneByMap(leaver.Character.Character.PositionInfo.Map);
			using (var interleave = new InterPacket(InterHeader.RemovePartyMember))
			{
				interleave.WriteString(leaver.Character.Character.Name, 16);
				interleave.WriteString(leaver.Character.Character.Name, 16);
				z.SendPacket(interleave);
			}
		}
		internal void RemoveInvite(GroupRequest pRequest)
		{
			this.openRequests.Remove(pRequest);
		}
		internal void UpdateInDatabase()
		{
			const string query =
				"UPDATE groups "
				+ "SET Member1 = {0} "
				+ "SET Member2 = {1} "
				+ "SET Member3 = {2} "
				+ "SET Member4 = {3} "
				+ "SET Member5 = {4} "
				+ "SET Exists = {5} "
				+ "WHERE Id = {6}";
			string formated = string.Format(query,
				GetDBMemberName(0),
				GetDBMemberName(1),
				GetDBMemberName(2),
				GetDBMemberName(3),
				GetDBMemberName(4),
				this.Exists,
				this.Id);
			Program.DatabaseManager.GetClient().ExecuteQuery(formated);
		}
		#endregion

		#region Private
		private void UpdateDropStateToMembers()
		{
			using (var packet = new Packet(SH14Type.PartyDropState))
			{
				packet.WriteByte((byte) DropState);

				foreach (var m in Members)
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
		private void AnnouncePartyList()
		{
			using (var packet = new Packet(SH14Type.PartyList))
			{
				packet.WriteByte((byte) Members.Count);
				foreach (var groupMember in Members)
				{
					packet.WriteString(groupMember.Name, 16);
					packet.WriteBool(groupMember.IsOnline);
				}

				foreach (var mem in Members.Where(m => m.IsOnline))
				{
					mem.Client.SendPacket(packet);
				}
			}
		}
		private void DeleteGroupByNameInDatabase(string pName)
		{
			string query = string.Format(
				"UPDATE characters SET GroupID = NULL WHERE Name = \'{0}\'", pName);
			Program.DatabaseManager.GetClient().ExecuteQuery(query);
		}
		private void AnnounceChangeMaster()
		{
			using (var packet = new Packet(SH14Type.ChangePartyMaster))
			{
				packet.WriteString(Master.Name, 16);
				packet.WriteUShort(1352);

				// Send to all online members
				Members.ForEach(m => { if (m.IsOnline) m.Client.SendPacket(packet); });
			}
		}
		private string GetDBMemberName(int pFor)
		{
			if(Members.Count < pFor)
				return "NULL";
			return string.Format("\'{0}\'", Members[pFor].Name);
		}
		private void SendInvitationPacket(WorldClient pTo)
		{
			using (var packet = new Packet(SH14Type.PartyInvite))
			{
				packet.WriteString(Master.Name, 16);
				pTo.SendPacket(packet);
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

	public enum DropState : byte
	{
		FreeForAll = 0,
		InRow = 1,
	}
}
