using System.Collections.Generic;
using Zepheus.Zone.Data;
using Zepheus.Zone.Game;
using System;
using MySql.Data.MySqlClient;

namespace Zepheus.Zone
{
	[Util.ServerModule(Util.InitializationStage.Clients)]
	public class GroupManager
	{
		#region .ctor
		private GroupManager()
		{
			this.groups = new List<Group>();
			this.groupsById = new Dictionary<long, Group>();
			this.groupsByMaster = new Dictionary<string, Group>();
		}
		#endregion
		#region Properties
		public static readonly TimeSpan GroupUpdateInterval = TimeSpan.FromSeconds(3); 
		public static GroupManager Instance { get; privat set; }

		private List<Group> groups;
		private Dictionary<string, Group> groupsByMaster;
		private Dictionary<long, Group> groupsById;
		private Queue<Group> updateQueue;
		#endregion
		#region Methods
		public void AddGroup(Group grp)
		{
			groups.Add(grp);
			groupsByMaster.Add(grp.Master.Name, grp);
			groupsById.Add(grp.Id, grp);
			updateQueue.Enqueue(grp);
		}
		public void LoadGroupFromDatabase(long pId)
		{
			Group group = Group.LoadGroupFromDatabaseById(pId);
			this.AddGroup(group);
		}
		public void Update()
		{
			// while the front group is has to be updated
			while (updateQueue.Peek().LastUpdate < (DateTime.Now - GroupUpdateInterval))
			{
				Group grp = updateQueue.Dequeue();
				UpdateGroup(grp);
			}

			// this will make it into a loop w/ the worker
			Worker.Instance.AddCallback(Update);
		}

		internal bool CheckCharacterHasGroup(ZoneCharacter pCharacter)
		{
			//--------------------------------------------------
			// Queries used 
			//--------------------------------------------------
			const string get_group_id_query = 
				"SELECT `GroupId` " +
				"FROM `characters` " +
				"WHERE `CharId` = {0}";
			//--------------------------------------------------
			// Get group id and check if char haz group
			//--------------------------------------------------
			string query = string.Format(get_group_id_query, pCharacter.ID);
			using(var client = Program.DatabaseManager.GetClient())
			using(var cmd = new MySqlCommand(query, client.Connection))
			using(var reader = cmd.ExecuteReader())
			{
				long id = null;
				while(reader.Read())
					id = reader.GetInt64(0);

				if(id == -1 || id == null)
					return false;
			}
		}
		internal Group GetGroupForCharacter(ZoneCharacter pCharacter)
		{
			//--------------------------------------------------
			// Quries used in function
			//--------------------------------------------------
			const string get_group_id_query = 
				"SELECT `GroupId` FROM `characters` " + 
				"WHERE `CharId` = {0} ";
			
			//--------------------------------------------------
			// get group id
			//--------------------------------------------------
			string query = string.Format(get_group_id_query, pCharacter.ID);
			long groupId = null;
			using(var client = Program.DatabaseManager.GetClient())
			using(var cmd = new MySqlCommand(query, client.Connection))
			using(var reader = cmd.ExecuteReader())
			{
				while(reader.Read())
					groupId = reader.GetInt64(0);
			}

			LoadGroupFromDatabase(groupId);
			return groupsById[groupId];
		}

		private void UpdateGroup(Group grp
		{
			grp.Update();
			updateQueue.Enqueue(grp);
		})
		#endregion
	}
}
