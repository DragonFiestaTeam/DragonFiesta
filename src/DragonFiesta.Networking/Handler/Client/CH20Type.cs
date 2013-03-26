namespace DragonFiesta.Networking.Handler.Client
{
	public class CH20Type : PacketHeader
	{
		public new const byte _Header = 20;
		public  const byte ByHpStone = 1;
		public  const byte BySpStone = 2;
		public  const byte UseHpStone = 7;
		public  const byte UseSpStone = 9;
	}
}