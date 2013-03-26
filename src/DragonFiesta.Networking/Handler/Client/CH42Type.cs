namespace DragonFiesta.Networking.Handler.Client
{
	public class CH42Type : PacketHeader
	{
		public new const byte _Header = 42;
		public  const byte AddToBlockList = 3;
		public  const byte RemoveFromBlockList = 7;
		public  const byte ClearBlockList = 11;
	}
}