namespace DragonFiesta.Networking.Handler.Server
{
	public class SH12Type : PacketHeader
	{
		public new const byte _Header = 12;
		public const byte ModifyItemSlot = 1;
		public const byte ModifyEquipSlot = 2;
		public const byte InventoryFull = 4;
		public const byte ObtainedItem = 10;
		public const byte FailedEquip = 17;
		public const byte FailedUnequip = 19;
		public const byte ItemUseEffect = 22;
		public const byte ItemUpgrade = 24;
		public const byte ItemUsedOk = 26;
		public const byte SendPremiumItemList = 33;
		public const byte SendRewawrdList = 45;
	}
}