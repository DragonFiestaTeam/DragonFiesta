namespace DragonFiesta.Networking.Handler.Client
{
	public class CH29Type : PacketHeader
	{
		public new const byte _Header = 29;
		public const byte CreateGuild = 5;
		public  const byte GuildInviteRequest = 9;
		public  const byte KickGuildMember = 14;
		public  const byte GuildMemberRemoveRequest = 28;
		public  const byte GuildLeaveByMember = 29;
		public  const byte GuildRequestAnswer = 12;
		public  const byte ChangeGuildDetails = 16;
		public  const byte ChangeMemberRank = 22;
		public  const byte GuildChatClientMessage = 115;
		public  const byte GuildNameRequest = 118;
		public  const byte GuildListRequest = 190;
	}
}