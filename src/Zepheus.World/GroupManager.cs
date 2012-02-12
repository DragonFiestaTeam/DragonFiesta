using System.Collections.Generic;
using Zepheus.Util;
using Zepheus.World.Data;

namespace Zepheus.World
{
	[ServerModule(Util.InitializationStage.Clients)]
	public class GroupManager
	{
		#region .ctor
		public GroupManager()
		{
			
		}
		#endregion

		#region Properties
		public static GroupManager Instance { get; private set; }

		public Dictionary<string, Group> groupsByMaster;
		public Dictionary<Group, List<GroupRequest>> requestsByGroup;
		#endregion

		#region Methods

		public static bool Initialize()
		{
			Instance = new GroupManager()
			{

			};
			Log.WriteLine(LogLevel.Info, "GroupManager iniialized.");
			return true;
		}
		#endregion
	}
}
