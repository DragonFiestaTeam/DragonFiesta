namespace DragonFiesta.Networking.Handler.Server
{
	public class SH3Type : PacketHeader
	{
		public new const byte _Header = 3;
		public const byte IncorrectVersion = 2;
		public const byte VersionAllowed = 3;
		public const byte FilecheckAllow = 5;
		public const byte Error = 9;
		public const byte WorldListNew = 10;
		public const byte WorldServerIP = 12;
		public const byte WorldListResend = 28;

		public const byte CharacterList = 20;
	}
}