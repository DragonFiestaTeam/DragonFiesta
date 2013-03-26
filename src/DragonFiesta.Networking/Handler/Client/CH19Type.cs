namespace DragonFiesta.Networking.Handler.Client
{
	public class CH19Type : PacketHeader
	{
		public new const byte _Header = 19;
		public  const byte TradeRequest = 1;
		public  const byte TradeDeclined = 3;
		public  const byte TradeAccept = 6;
		public  const byte TradeBreak = 10;
		public  const byte TradeAddItem = 13;
		public  const byte TradeRemoveItem = 17;
		public  const byte TradeChangeMoney = 21;
		public  const byte TradeLock = 25;
		public  const byte TradeAgree = 31;
	}
}