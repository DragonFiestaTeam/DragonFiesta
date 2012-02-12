using System.Collections.Generic;
using System.Linq;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;

namespace Zepheus.World.Data
{
	public class Group
	{
		#region .ctor
		public Group()
		{
			this.Members = new GroupMember[5];
		}
		#endregion

		#region Properties

		public const int MAX_MEMBERS = 5;
		private GroupMember[] Members;
		public GroupMember this[int index] { get { return Members[index]; }}
		public GroupMember Master { get { return Members.Single(m => m.Role == GroupRole.Master); }}
		public IEnumerable<GroupMember> NormalMembers { get { return from m in Members where m.Role != GroupRole.Master select m; }}
		public DropState DropState { get; private set; }
		public ushort Id { get; private set; }

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
		#endregion
		#region EventHandler

		#endregion
		#endregion
	}

	public enum DropState : byte
	{
		// TODO: Add states!
		Unk = 0x0,
	}
}
