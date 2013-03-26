namespace DragonFiesta.Networking.Handler.Client
{
	public class CH5Type : PacketHeader
	{
		public new const byte _Header = 5;
		public const byte CreateCharacter = 1;
		public const byte DeleteCharacter = 7;
	}
}