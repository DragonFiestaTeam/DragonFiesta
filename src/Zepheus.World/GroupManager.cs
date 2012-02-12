using System;
using System.Collections.Generic;
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
			requestsByGroup = new Dictionary<Group, List<GroupRequest>>();
		}
		#endregion

		#region Properties
		public static GroupManager Instance { get; private set; }

		public List<Group> groups;
		public Dictionary<string, Group> groupsByMaster;
		public Dictionary<Group, List<GroupRequest>> requestsByGroup;
		private long maxId = 0; //  todo: needs to be saved.. :(
		#endregion

		#region Methods
		public long GetNextId()
		{
			long tmp = maxId;
			maxId ++;
			return tmp;
		}
		public Group CreateNewGroup(WorldClient master)
		{
			Group grp = new Group(GetNextId());
			GroupMember mstr = new GroupMember(master, GroupRole.Master);
			grp.AddMember(mstr);
			// TODO: Add group in Database?

			return grp;
		}

		[InitializerMethod]
		public static bool Initialize()
		{
			Instance = new GroupManager
			{
			};
			Log.WriteLine(LogLevel.Info, "GroupManager iniialized.");
			return true;
		}

		[CleanUpMethod]
		public static void CleanUp()
		{
			while (Instance.groups.Count > 0)
			{
				Instance.groups[0].BreakUp();
			}
		}

		#region EventHandler
		
		internal void OnGroupBrokeUp(object sender, EventArgs e)
		{
			Group grp = sender as Group;
			if(grp == null)
				return;

			groups.Remove(grp);
			groupsByMaster.Remove(grp.Master.Name);
			requestsByGroup.Remove(grp);
		}

		#endregion
		#endregion
	}
}