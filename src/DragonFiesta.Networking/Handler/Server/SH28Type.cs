namespace DragonFiesta.Networking.Handler.Server
{
	public class SH28Type : PacketHeader
	{
		public new const byte _Header = 28;
		public const byte LoadQuickBar = 3;
		public const byte LoadQuickBarState = 5;
		public const byte LoadGameSettings = 11;
		public const byte LoadClientSettings = 13;
		public const byte LoadShortCuts = 15;
	}
}