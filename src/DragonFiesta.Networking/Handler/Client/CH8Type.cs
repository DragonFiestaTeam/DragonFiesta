namespace DragonFiesta.Networking.Handler.Client
{
	public class CH8Type : PacketHeader
	{
		public new const byte _Header = 8;
		public const byte By = 29;
		public const byte ByCancel = 11;
		public const byte Whisperto = 12;
		public const byte ChatNormal = 1;
		public const byte ChatParty = 20;
		public const byte BeginInteraction = 10;
		public const byte Stop = 18;
		public const byte Walk = 23;
		public const byte Run = 25;
		public const byte Shout = 30;
		public const byte Emote = 32;
		public const byte Jump = 36;
		public const byte BeginRest = 39;
		public const byte EndRest = 42;
	}
}