namespace DragonFiesta.Networking.Handler.Server
{
	public class SH5Type : PacketHeader
	{
		public new const byte _Header = 5;
		public const byte CharCreationError = 4;
		public const byte CharCreationOk = 6;
		public const byte CharDeleteOk = 12;
	}
}