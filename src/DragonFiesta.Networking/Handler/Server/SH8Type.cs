namespace DragonFiesta.Networking.Handler.Server
{
	public class SH8Type : PacketHeader
	{
		public new const byte _Header = 8;
		public const byte ChatNormal = 2;
		public const byte WhisperFrom = 13;
		public const byte WhisperTargetNotFound = 14;
		public const byte WhisperTo = 15;
		public const byte GmNotice = 17;
		public const byte StopTele = 19;	// Stops char but can teleport
		public const byte PartyChat = 21;
		public const byte Walk = 24;
		public const byte Move = 26;
		public const byte Teleport = 27;
		public const byte Interaction = 28;
		public const byte Shout = 31;
		public const byte Emote = 33;
		public const byte Jump = 37;
		public const byte BeginRest = 40;
		public const byte BeginDisplayRest = 41;
		public const byte EndRest = 43;
		public const byte EndDisplayRest = 44;
		public const byte Mounting = 63;
		public const byte MapMount = 64;
		public const byte Unmount = 66;
		public const byte MapUnmount = 67;
		public const byte UpdateMountFound = 70;
		public const byte CastItem = 71;
		public const byte BlockWalk = 74;
	}
}