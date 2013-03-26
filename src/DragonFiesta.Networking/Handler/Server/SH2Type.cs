namespace DragonFiesta.Networking.Handler.Server
{
    // Named as SHXType , where X = header ID
	 
	public class SH2Type : PacketHeader
	{
		public new const byte _Header = 2;
		public const byte Ping = 4;
		public const byte SetXorKeyPosition = 7;
		public const byte Chatblock = 72;
		public const byte UpdateClientTime = 73;
		public const byte UnkTimePacket = 69;
		public const byte Unk1 = 14;
	}
}
