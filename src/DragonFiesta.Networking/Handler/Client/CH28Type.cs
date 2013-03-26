namespace DragonFiesta.Networking.Handler.Client
{
	public class CH28Type : PacketHeader
	{
		public new const byte _Header = 28;
		public  const byte GetQuickBar = 2;
		public  const byte GetQuickBarState = 4;
		public  const byte GetGameSettings = 10;
		public  const byte GetClientSettings = 12;
		public  const byte GetShortCuts = 14;

		public  const byte SaveQuickBar = 16;
		public  const byte SaveQuickBarState = 4;
		public  const byte SaveGameSettings = 20;
		public  const byte SaveClientSettings = 21;
		public  const byte SaveShortcuts = 22;
	}
}