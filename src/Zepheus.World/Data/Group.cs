using System;
using System.Collections.Generic;
using System.Linq;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;

namespace Zepheus.World.Data
{
	public class Group
	{
		#region .ctor
		public Group(long id)
		{
			this.Members = new List<GroupMember>();
			this.Id = id;
			this.Members = new List<GroupMember>();
			this.DropState = DropState.FreeForAll;
			this._gotLastDrop = 0;
		}
		#endregion

		#region Properties

		public const int MAX_MEMBERS = 5;
		private List<GroupMember> Members;
		public GroupMember this[int index] { get { return Members[index]; }}
		public GroupMember Master { get { return Members.Single(m => m.Role == GroupRole.Master); }}
		public IEnumerable<GroupMember> NormalMembers { get { return from m in Members where m.Role != GroupRole.Master select m; }}
		public DropState DropState { get; private set; }
		public long Id { get; private set; }

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
			// TODO: Send to clients

			ExecuteBreakUpQuery();
			OnBrokeUp();
		}

		internal void AddMember(GroupMember memb)
		{
			this.Members.Add(memb);
			memb.Group = this;
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
		private void ExecuteBreakUpQuery()
		{
			const string query = "DELETE FROM `groups` WHERE `Id` = {0}";
			string realQuery = string.Format(query, this.Id);

			Program.DatabaseManager.GetClient().ExecuteQuery(realQuery);
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
