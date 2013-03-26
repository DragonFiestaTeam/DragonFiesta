namespace DragonFiesta.Networking.Handler.Server
{
	public class SH9Type : PacketHeader
	{
		public new const byte _Header = 9;
		public const byte StatUpdate = 2;
		public const byte GainExp = 11;
		public const byte LevelUp = 12;
		public const byte LevelUpAnimation = 13;
		public const byte HealHP = 14;
		public const byte HealSP = 15;
		public const byte SkilLAck = 53;
		public const byte ResetStance = 61;
		public const byte AttackAnimation = 71;
		public const byte AttackDamage = 72;
		public const byte DieAnimation = 74;
		public const byte SkillUsePrepareSelf = 78;
		public const byte SkillUsePrepareOthers = 79;
		public const byte SkillAnimationPosition = 81;
		public const byte SkillAnimationTarget = 82;
		public const byte SkillAnimation = 87;
	}
}