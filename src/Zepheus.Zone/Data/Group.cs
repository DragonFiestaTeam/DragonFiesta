using System;
using System.Collections.Generic;
using System.Linq;
using Zepheus.Zone.Game;
using Zepheus.FiestaLib.Networking;
using Zepheus.FiestaLib;

namespace Zepheus.Zone.Data
{
	public class Group
	{
		#region .ctor
		public Group()
		{
			this.members = new List<GroupMember>();
		}
		#endregion
		#region Properties
		public long Id { get; private set; }
		public GroupMember Master { get { return this.members.Single(m => m.IsMaster); } }
		public IEnumerable<GroupMember> NormalMembers { get { return this.members.Where(m => !m.IsMaster); } }

		private readonly List<GroupMember> members;
		#endregion
		#region Methods
		public void AddMemberToGroup(ZoneCharacter pCharacter, bool pIsMaster)
		{
			GroupMember mem = new GroupMember();
			mem.Character = pCharacter;
			mem.Group = this;
			mem.IsMaster = pIsMaster;
			mem.IsOnline = true;
			mem.Name = pCharacter.Name;
			pCharacter.GroupMember = mem;
			pCharacter.LevelUp += OnCharacterLevelUp;
			this.members.Add(mem);
		}

		#region Private
		public void UpdateCharacterLevel(ZoneCharacter pChar)
		{
			using (Packet packet = new Packet(SH14Type.SetMemberStats))
			{
				packet.WriteByte(0x01);             // UNK
				packet.WriteString(pChar.Name, 16);
				packet.WriteByte((byte)pChar.Job);
				packet.WriteByte(pChar.Level);
				packet.WriteUInt(pChar.MaxHP);
				packet.WriteUInt(pChar.MaxSP);
				packet.WriteByte(0x01);             // UNK

				AnnouncePacket(packet);
			}
		}
		public void UpdateCharacterHpSp(ZoneCharacter pChar)
		{
			using (Packet packet = new Packet(SH14Type.UpdatePartyMemberStats))
			{
				packet.WriteByte(0x01);             // UNK
				packet.WriteString(pChar.Name, 16);
				packet.WriteUInt(pChar.HP);
				packet.WriteUInt(pChar.SP);

				AnnouncePacket(packet);
			}
		}
		private void AnnouncePacket(Packet pPacket)
		{
			foreach (var mem in this.members)
			{
				mem.Character.Client.SendPacket(pPacket);
			}
		}
		#endregion
		#region Eventhandlers
		private void OnCharacterLevelUp(object sender, LevelUpEventArgs args)
		{
			UpdateCharacterLevel((ZoneCharacter)sender);
			UpdateCharacterHpSp((ZoneCharacter)sender);
		}
		#endregion
		#endregion
	}
}
