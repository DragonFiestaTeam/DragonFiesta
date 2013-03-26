namespace DragonFiesta.Networking.Handler.Server
{
	public class SH42Type : PacketHeader
	{
		public new const byte _Header = 42;
		public const byte BlockList = 2;
		public const byte AddToBlockList = 6;
		public const byte RemoveFromBlockList = 10;
		public const byte ClearBlockList = 14;
	}
}