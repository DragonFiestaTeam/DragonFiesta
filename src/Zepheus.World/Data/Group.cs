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
		public GroupMember this[int index]
		{
			get
			{
				return members[index];
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
			{
				// TODO: Send Message
				return;
			}
			if(Master.Name != pSender.Character.Name)
				return;		// only the master may invite new members
			
			// TODO: Handle/Implement!
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
			
			// TODO: Send packet to other members to update GroupList!
		}
		internal void RemoveInvite(GroupRequest pRequest)
		{
			this.openRequests.Remove(pRequest);
		}
		internal void UpdateInDatabase()
		{
			// TODO: Implement
		}
		internal void CreateInDatabase()
		{
			
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
		private void AnnouncePartyList()
		{
			using (var packet = new Packet(SH14Type.PartyList))
			{
				packet.WriteByte((byte) members.Count);
				foreach (var groupMember in members)
				{
					packet.WriteString(groupMember.Name, 16);
					packet.WriteBool(groupMember.IsOnline);
				}

				foreach (var mem in members.Where(m => m.IsOnline))
				{
					mem.Client.SendPacket(packet);
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
