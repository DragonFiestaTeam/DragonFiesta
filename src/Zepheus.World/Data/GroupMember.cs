using Zepheus.World.Networking;

namespace Zepheus.World.Data
{
	public class GroupMember
	{
		#region .ctor
		#endregion
		
		#region Properties
		public WorldCharacter Character { get; private set; }
		public string Name { get; private set; }
		public Group Group { get; private set; }
		public GroupRole Role { get; internal set; }
		public WorldClient Client { get; private set; }
		#endregion

		#region Methods
		#endregion
	}

	public enum GroupRole
	{
		None = 0x0,
		Master,
		Member
	}
}
