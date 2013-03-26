namespace DragonFiesta.Networking.Handler.Server
{
	public class SH4Type : PacketHeader
	{
		public const byte Money = 51;
		public const byte UpdateStats = 53;
		public const byte ConnectError = 2;
		public const byte Unk = 222;
		public const byte ServerIP = 3;
		public const byte CharacterGuildInfo = 18;
		public const byte CharacterInfo = 56;
		public const byte CharacterLook = 57;
		public const byte CharacterQuestBusy = 58;
		public const byte CharacterActiveSkillList = 61;
		public const byte CharacterPassiveSkillList = 62;
		public const byte CharacterItemList = 71;
		public const byte CharacterInfoEnd = 72;
		public const byte CharacterTimedItemList = 74;
		public const byte ReviveWindow = 77;
		public const byte Revive = 79;
		public const byte CharacterPoints = 91;
		public const byte SetPointOnStat = 95;
		public const byte CharacterAcademyInfo = 151;
	}
}