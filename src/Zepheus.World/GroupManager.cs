using System;
using System.Collections.Generic;
using System.Linq;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.World.Data;
using Zepheus.World.Networking;

namespace Zepheus.World
{
	[ServerModule(InitializationStage.Clients)]
	public class GroupManager
	{
		#region .ctor
		public GroupManager()
		{
			groups = new List<Group>();
			groupsByMaster = new Dictionary<string, Group>();
			groupsById = new Dictionary<long, Group>();
			requestsByGroup = new Dictionary<Group, List<GroupRequest>>();
		}

		[InitializerMethod]
		public static bool Initialize()
		{
			Instance = new GroupManager();
			return true;
		}
		#endregion

		#region Properties
		public static GroupManager Instance { get; private set; }

		private List<Group> groups;
		private Dictionary<string, Group> groupsByMaster;
		private Dictionary<Group, List<GroupRequest>> requestsByGroup;
		private Dictionary<long, Group> groupsById;
		private long maxId = 0; //  todo: needs to be saved.. :(
		#endregion

		#region Methods
		public long GetNextId()
		{
			long tmp = maxId;
			maxId ++;
			return tmp;
		}
		public Group CreateNewGroup(WorldClient pMaster)
		{
			Group grp = new Group(GetNextId());
			GroupMember mstr = new GroupMember(pMaster, GroupRole.Master);
			pMaster.Character.GroupMember = mstr;
			grp.AddMember(mstr);
			// TODO: Add group in Database?

			return grp;
		}
		public void Invite(WorldClient pClient, string pInvited)
		{
			Log.WriteLine(LogLevel.Debug, "{0} Invited {1}", pClient.Character.Character.Name, pInvited);
			if(!ClientManager.Instance.IsOnline(pInvited))
				return; // not online

			WorldClient invitedClient = ClientManager.Instance.GetClientByCharname(pInvited);

			if(pClient.Character.Group == null)
				pClient.Character.Group = CreateNewGroup(pClient);

			GroupRequest request = new GroupRequest(pClient, pClient.Character.Group, pInvited);
			AddRequest(request);
			pClient.Character.Group.AddInvite(request);
			SendInvitedPacket(invitedClient, pClient);
		}
		public void DeclineInvite(WorldClient pClient, string pFrom)
		{
			if(!ClientManager.Instance.IsOnline(pFrom))
				return;			// Inviter / master not online!
			WorldClient from = ClientManager.Instance.GetClientByCharname(pFrom);
			if(!groupsByMaster.ContainsKey(pFrom))
				return;			// No such party
			Group grp = groupsByMaster[pFrom];
			GroupRequest request = requestsByGroup[grp].Find(r => r.InvitedClient == pClient);

			RemoveRequest(request);
			grp.RemoveInvite(request);
		}
		public void AcceptInvite(WorldClient pClient, string pFrom)
		{
			if(!ClientManager.Instance.IsOnline(pFrom))
				return;
			WorldClient from = ClientManager.Instance.GetClientByCharname(pFrom);
			if(!groupsByMaster.ContainsKey(pFrom))
				return;
			Group grp = groupsByMaster[pFrom];
			GroupRequest request = requestsByGroup[grp].Find(r => r.InvitedClient == pClient);
			RemoveRequest(request);
			grp.RemoveInvite(request);
			grp.MemberJoin(pClient.Character.Character.Name);
		}
		public void LeaveParty(WorldClient pClient)
		{
			if (pClient.Character.Group.NormalMembers.Count() <= 1) // Not enough members for party to stay
			{
				pClient.Character.Group.BreakUp();
			}
			else
			{
				pClient.Character.Group.MemberLeaves(pClient);
			}
		}
		public void KickMember(WorldClient pClient, string pKicked)
		{
			if(pClient.Character.GroupMember.Role != GroupRole.Master)
				return; // Only master may kick ppl 

			if (pClient.Character.Group.NormalMembers.Count() <= 1)
			{
				pClient.Character.Group.BreakUp();
			}
			else
			{
				pClient.Character.Group.KickMember(pKicked);
			}
		}
		public void ChangeMaster(WorldClient pClient, string pMastername)
		{
			if(pClient.Character.GroupMember.Role != GroupRole.Master)
				return;
			pClient.Character.Group.ChangeMaster(pClient.Character.Group.NormalMembers.Single(m => m.Name == pMastername));
		}

		internal void OnGroupBrokeUp(object sender, EventArgs e)
		{
			Group grp = sender as Group;
			if(grp == null)
				return;

			groups.Remove(grp);
			groupsByMaster.Remove(grp.Master.Name);
			requestsByGroup.Remove(grp);
		}

		private void AddRequest(GroupRequest pRequest)
		{
			if (!this.requestsByGroup.ContainsKey(pRequest.Group))
			{
				this.requestsByGroup.Add(pRequest.Group, new List<GroupRequest>());
			}

			this.requestsByGroup[pRequest.Group].Add(pRequest);
		}
		private void RemoveRequest(GroupRequest pRequest)
		{
			this.requestsByGroup[pRequest.Group].Remove(pRequest);
		}
		private void SendInvitedPacket(WorldClient pInvited, WorldClient pFrom)
		{
			using (var ppacket = new Packet(SH14Type.PartyInvite))
			{
				ppacket.WriteString(pFrom.Character.Character.Name, 0x10);
				pInvited.SendPacket(ppacket);
			}
		}
		private void SendInviteDeclinedPacket(WorldClient pInviter, WorldClient pInvited)
		{
			/*WorldClient InvideClient = ClientManager.Instance.GetClientByCharname(InviteChar);
			packet.WriteString(InvideClient.Character.Character.Name);
			packet.WriteUShort(1217);
			InvideClient.SendPacket(packet);*/
			// TODO: Send Packet
		}

		#endregion
	}
}