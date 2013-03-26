namespace DragonFiesta.Networking.Handler.Server
{
	public class SH19Type : PacketHeader
	{
		public new const byte _Header = 19;
		public const byte SendTradeRequest = 2;
		public const byte DeclineRequest = 4;
		public const byte SendTradeAccept = 9;
		public const byte SendTradeBreak = 12;
		public const byte SendAddItemSuccesfull = 15;
		public const byte SendAddItem = 16;
		public const byte SendItemRemove = 19;
		public const byte SendChangeMoney = 24;
		public const byte SendTradeRdy = 27;
		public const byte SendTradeLock = 28;
		public const byte SendRemoveItem = 20;
		public const byte SendTradeAgreeMe = 33;
		public const byte SendTradeAgreeTo = 34;
		public const byte SendTradeComplete = 36;
	}
}