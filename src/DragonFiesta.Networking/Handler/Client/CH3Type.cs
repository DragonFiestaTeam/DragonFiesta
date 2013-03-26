namespace DragonFiesta.Networking.Handler.Client
{
	public class CH3Type : PacketHeader
	{
		public new const byte _Header = 3;
		public const byte Version = 1;
		public const byte Login = 56;
		public const byte WorldListRequest = 27;
		public const byte FileHash = 4;
		public const byte WorldSelect = 11;
		public const byte WorldClientKey = 15;
		public const byte BackToCharSelect = 24;
	}
}