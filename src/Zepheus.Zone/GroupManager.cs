using System.Collections.Generic;
using Zepheus.Zone.Data;
using System;

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
		private void UpdateGroup(Group grp
		{
			grp.Update();
			updateQueue.Enqueue(grp);
		})
		#endregion
	}
}
