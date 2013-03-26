namespace DragonFiesta.Networking.Handler.Client
{
	public class CH9Type : PacketHeader
	{
		public new const byte _Header = 9;
		public const byte SelectObject = 1;
		public const byte DeselectObject = 8;
		public const byte StartAttackMelee = 43;
		public const byte StopAttackMelee = 50;
		public const byte AttackSkill = 61;
		public const byte UseSkillTarget = 64;
		public const byte UseSkillPosition = 65;
	}
}