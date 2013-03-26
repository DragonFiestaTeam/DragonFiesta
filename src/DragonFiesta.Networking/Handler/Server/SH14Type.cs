namespace DragonFiesta.Networking.Handler.Server
{
	public class SH14Type : PacketHeader
	{
		public new const byte _Header = 14;
		public const byte InviteDeclined = 7;
		public const byte UpdateMemberLocation = 73;
		public const byte UpdateMemberStats = 50;
		public const byte SetMemeberStats = 51;
		public const byte PartyInvite = 3;
		public const byte PartyAccept = 4;
		public const byte PartyDropStat = 76;
		public const byte PartyList = 9;
		public const byte PartyLeave = 11;
		public const byte GroupList = 85;
		public const byte ChangePartyMaster = 41;
		public const byte ChangePartyDrop = 75;
		public const byte KickPartyMember = 21;
		public const byte BreakUp = 30;
	}
}