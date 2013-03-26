namespace DragonFiesta.Networking.Handler.Server
{
	public class SH6Type : PacketHeader
	{
		public new const byte _Header = 6;
		public const byte DetailedCharacterInfo = 2;
		public const byte Error = 3;
		public const byte RemoveDrop = 5;
		public const byte ChangeMap = 9;
		public const byte ChangeZone = 10;
		public const byte Teleporter = 27;
	}
}