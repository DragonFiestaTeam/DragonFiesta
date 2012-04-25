﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.InterLib.Networking;
using Zepheus.Zone.InterServer;

namespace Zepheus.Zone.Game
{
	public class Group
	{
		#region .ctor
		public Group()
		{
			this.Members = new List<GroupMember>();
			this.LastUpdate = DateTime.Now;
		}

		#endregion
		#region Properties
		public long Id { get; private set; }
		public GroupMember Master
		{
			get
			{
				return this.Members.Single(m => m.IsMaster);
			}
		}
		public IEnumerable<GroupMember> NormalMembers
		{
			get
			{
				return this.Members.Where(m => !m.IsMaster);
			}
		}
		public DateTime LastUpdate { get; private set; }

		public readonly List<GroupMember> Members;
		#endregion
		#region Methods
		public static Group LoadGroupFromDatabaseById(long pId)
		{
			//--------------------------------------------------
			// Queries used in this function
			//--------------------------------------------------
			const string read_group_query =
				"USE `fiesta_world`; "	+				
				"SELECT * FROM `groups` " +
				"WHERE `Id` = {0}";

			//--------------------------------------------------
			// Reading the group out of the database
			//--------------------------------------------------

			Group grp = new Group();
			grp.Id = pId;

			using (var client = Program.DatabaseManager.GetClient())
			using (var table = client.ReadDataTable(string.Format(read_group_query, pId)))
			{
				foreach (DataRow row in table.Rows)
				{
					for (int i = 1; i < 6; i++)
					{
						string colName = string.Format("Member{0}", i);
						if(!row.IsNull(colName))
							grp.Members.Add(ReadGroupMemberFromDatabase((long) row[colName]));
					}
				}
			}

			return grp;
		}

		public void AddMember(ZoneCharacter pCharacter, bool pIsMaster)
		{
			GroupMember mem = new GroupMember();
			mem.Character = pCharacter;
			mem.Group = this;
			mem.IsMaster = pIsMaster;
			mem.IsOnline = true;
			mem.Name = pCharacter.Name;
			pCharacter.GroupMember = mem;
			pCharacter.LevelUp += OnCharacterLevelUp;
			pCharacter.Group = this;
			pCharacter.GroupMember = mem;
			this.Members.Add(mem);
		}
		
		public void AddMember(string pName, bool pIsMaster = false)
		{
			GroupMember mem = new GroupMember();
			mem.Character = null;
			mem.Group = this;
			mem.IsMaster = pIsMaster;
			mem.IsOnline = false;
			mem.Name = pName;

			if (ClientManager.Instance.HasClient(pName))
			{
				mem.IsOnline = true;
				var client = ClientManager.Instance.GetClientByCharName(pName);
				mem.Character = client.Character;
				mem.Character.LevelUp += OnCharacterLevelUp;
				mem.Character.Group = this;
				mem.Character.GroupMember = mem;
			}

			this.Members.Add(mem);
		}

		public void Update()
		{
			/* Note									*
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
			foreach (var m in Members.Where(mem => mem.IsOnline))
			{
				UpdateMemberPosition(m);
			}
		}
		public static GroupMember ReadGroupMemberFromDatabase(long pCharId)
		{
			//--------------------------------------------------
			// Quries used in this function
			//--------------------------------------------------
			const string get_groupmem_query =
							"SELECT `Name`, `IsGroupMaster` FROM `characters` " +
							"WHERE `GroupId` = {0}";

			//--------------------------------------------------
			// Read member from database.
			//--------------------------------------------------
			string name = "";
			bool isOnline = false;
			bool isMaster = false;

			using (var client = Program.DatabaseManager.GetClient())
			using (var cmd = new MySqlCommand(string.Format(get_groupmem_query, pCharId), client.Connection))
			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					name = reader.GetString("Name");
					isMaster = reader.GetBoolean("IsGroupMaster");
				}
			}

			GroupMember member = new GroupMember(name, isMaster, isOnline);
			if (ClientManager.Instance.HasClient(name))
			{
				var client = ClientManager.Instance.GetClientByCharName(name);
				member.IsOnline = true;
				member.Character = client.Character;
			}
			else
				member.IsOnline = (bool)InterFunctionCallbackProvider.Instance.QueuePacket(id =>
				{
					var packet = new InterPacket(InterHeader.FunctionCharIsOnline);
					packet.WriteLong(id);
					packet.WriteString(name, 16);
					return packet;
				}, packet =>
				{
					bool value = false;
					packet.TryReadBool(out value);
					return value;
				});
			return member;
		}

		#region Private
		private void AnnouncePacket(Packet pPacket)
		{
			foreach (var mem in this.Members)
			{
				mem.Character.Client.SendPacket(pPacket);
			}
		}
		private void UpdateMemberPosition(GroupMember member)
		{
			if (!member.IsOnline)
				return;
			using (var packet = new Packet(SH14Type.UpdatePartyMemberLoc))
			{
				packet.WriteString(member.Name, 0x10);
				packet.WriteInt(member.Character.Position.X);
				packet.WriteInt(member.Character.Position.Y);

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
