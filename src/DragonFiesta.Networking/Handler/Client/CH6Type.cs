namespace DragonFiesta.Networking.Handler.Client
{
	public class CH6Type : PacketHeader
	{
		public new const byte _Header = 6;
		public const byte TransferKey = 1;
		public const byte ClientReady = 3;
		public const byte Teleporter = 26;
	}
}