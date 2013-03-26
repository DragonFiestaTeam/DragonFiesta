namespace DragonFiesta.Networking.Handler.Server
{
	public class SH29Type : PacketHeader
	{
		public new const byte _Header = 29;
		public const byte SendRequesterResponse = 10;
		public const byte SendGuildInviteRequest = 11;
		public const byte GuildKickResponse = 15;
		public const byte ChangeGuildMessageResponse = 17;
		public const byte KickFromGuildForMember = 20;	// got kicked
		public const byte RankChangeResponse = 23;
		public const byte LeaveResponse = 29;
		public const byte ClearGuildDetailMessage = 191;
		public const byte UnkMessageChange = 196;
		public const byte GuildList = 27;
		public const byte RemoveFromGuild = 29; // Note: doubled OP code!!
		public const byte ChangeResponse = 39;
		public const byte SendUpdateDetils = 45;
		public const byte AddGuildMember = 54;
		public const byte GuildMemberKick = 55;
		public const byte RemoveGuildMember = 56;
		public const byte ChangeRank = 57;
		public const byte SendMemberGotOnline = 61;
		public const byte SendMemberGotOffline = 62;
		public const byte GuildChatMessage = 116;
		public const byte GuildNameResult = 119;
	}
}