using Zepheus.World.Networking;

namespace Zepheus.World.Data
{
	public class GroupMember
	{
		#region .ctor

		public GroupMember(WorldClient client, GroupRole role)
		{
			this.Client = client;
			this.Character = client.Character;
			this.Role = role;
		}
		#endregion
		
		#region Properties
		public WorldCharacter Character { get; private set; }
		public string Name { get; private set; }
		public Group Group { get; internal set; }
		public GroupRole Role { get; internal set; }
		public WorldClient Client { get; private set; }
		public bool IsOnline { get; set; }
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
