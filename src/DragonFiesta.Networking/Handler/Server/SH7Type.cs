namespace DragonFiesta.Networking.Handler.Server
{
	public class SH7Type : PacketHeader
	{
		public new const byte _Header = 7;
		public const byte ShowUnequip = 4;
		public const byte ShowEquip = 5;
		public const byte SpawnSinglePlayer = 6;
		public const byte SpawnMutliPlayer = 7;
		public const byte SpawnSingleObject = 9;
		public const byte SpawnMutliObject = 9;
		public const byte ShowDrop = 10;
		public const byte ShowDrops = 11;
		public const byte RemoveObject = 14;
	}
}