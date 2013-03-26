namespace DragonFiesta.Networking.Handler.Client
{
	public class CH12Type : PacketHeader
	{
		public new const byte _Header = 12;
		public const byte BuyItem = 3;
		public const byte SellItem = 6;
		public const byte DropItem = 7;
		public const byte LootItem = 9;
		public const byte MoveItem = 11;
		public const byte Equip = 15;
		public const byte Unequip = 18;
		public const byte UseItem = 21;
		public const byte ItemEnhance = 23;
		public const byte GetPremiumItemList = 32;
		public const byte GetRewardItemList = 44;
	}
}