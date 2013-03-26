namespace DragonFiesta.Networking.Handler.Server
{
	public class SH38Type : PacketHeader
	{
		public new const byte _Header = 38;
		public const byte AcademyDetails = 8;
		public const byte AcademyList = 14;
		public const byte AcademyRequest = 18;
		public const byte AcademyJoin = 19;
		public const byte AcademyChangeDetailsRespnse = 37;
		public const byte AcademyChangeDetails = 38;
		public const byte KickResponse = 23;
		public const byte KickAcademyMember = 24;
		public const byte AcademyLeaveResponse = 28;	// 6052 = failed coz hour 6016 Sucessfull
		public const byte BlockMessage = 35;
		public const byte ChangeResponse = 52;
		public const byte ChangeRequest = 43;
		public const byte JoinGuildFromAcademy = 46;
		public const byte AcademyDekanChange = 91;
		public const byte AcademyMemberLeave = 96;
		public const byte AcademyMemberOnline = 97;
		public const byte AcademyMemberOffline = 98;
		public const byte AcademyMemberChangeMap = 99;
		public const byte AcademyMemberLevelUp = 102;
		public const byte AcademyMemberChangeJob = 103;
		public const byte AcademyChatMessage = 105;
		public const byte AcademyChatBlock = 106;
		public const byte AcademyReward = 115;
	}
}