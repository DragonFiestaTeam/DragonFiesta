namespace DragonFiesta.Networking.Handler.Server
{
	public class SH20Type : PacketHeader
	{
		public new const byte _Header = 20;
		public const byte ChangeHPStones = 3;
		public const byte ChangeSPStones = 4;
		public const byte ErrorBuyStone = 5;
		public const byte ErrorUSeStone = 6;
		public const byte StartHPStoneCooldown = 8;
		public const byte StartSPStoneCooldown = 10;
	}
}