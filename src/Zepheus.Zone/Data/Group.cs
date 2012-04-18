using System;
using System.Collections.Generic;
using System.Linq;
using Zepheus.Zone.Game;
using Zepheus.FiestaLib.Networking;
using Zepheus.FiestaLib;

using MySql.Data.MySqlClient;

namespace Zepheus.Zone.Data
{
	public class Group
	{
		#region .ctor
		public Group()
		{
			this.members = new List<GroupMember>();
			this.LastUpdate = DateTime.Now;
		}
		#endregion
		#region Properties
		public long Id { get; private set; }
		public GroupMember Master { get { return this.members.Single(m => m.IsMaster); } }
		public IEnumerable<GroupMember> NormalMembers { get { return this.members.Where(m => !m.IsMaster); } }
		public DateTime LastUpdate { get; private set; }

		private readonly List<GroupMember> members;
		#endregion
		#region Methods
		public static Group LoadGroupFromDatabaseById(long pId)
		{
			//--------------------------------------------------
			// Queries used in this function
			//--------------------------------------------------
			
			const string read_group_query = 
				"SELECT * FROM `groups` "+
				"WHERE `Id` = {0} `";

			//--------------------------------------------------
			// Reading the group out of the database
			//--------------------------------------------------

			Group grp = new Group();
			grp.Id = pId;

			using(var client = Program.DatabaseManager.GetClient())
			{
				string query = string.Format(read_group_query,
									pId);
				using(var cmd = new MySqlCommand(query, client.Connection))
				using(var reader = cmd.ExecuteReader())
				{
					while(reader.Read())
					{
						long?[] members = new long?[5];
						members[0] = reader.GetInt64("Member1");
						members[1] = reader.GetInt64("Member2");
						members[2] = reader.GetInt64("Member3");
						members[3] = reader.GetInt64("Member4");
						members[4] = reader.GetInt64("Member5");

						foreach(long? m in members)
						{
							if(m != null)
								grp.members.Add(ReadGroupMemberFromDatabase(m));
						}
					}
				}
			}
			return grp;
		}
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
		public void Update()
		{
			/* Note										*
			 * Add more update logic here if needed.	*
			 * this will automatically repeated.		*/

			UpdateGroupPositions();
			this.LastUpdate = DateTime.Now;
		}
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
		public void UpdateGroupPositions()
		{
			foreach(var m in members.Where(mem => mem.IsOnline))
			{
				UpdateMemberPosition(m);
			}
		}
		#region Private
		private void AnnouncePacket(Packet pPacket)
		{
			foreach (var mem in this.members)
			{
				mem.Character.Client.SendPacket(pPacket);
			}
		}
		private static GroupMember ReadGroupMemberFromDatabase(long id)
		{
			return null;
		}
		private void UpdateMemberPosition(GroupMember member)
		{
			if(!member.IsOnline)
				return;
			using(var packet = new Packet(SH14Type.UpdatePartyMemberLoc))
			{
				packet.WriteString(member.Name, 0x10);
				packet.WriteString(member.Character.Position.X);
				packet.WriteString(member.Character.Position.Y);

				AnnouncePacket(packet);
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
