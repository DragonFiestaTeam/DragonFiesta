namespace DragonFiesta.Networking.Handler.Server
{
	public class SH15Type : PacketHeader
	{
		public new const byte _Header = 15;
		public const byte Question = 1;
		public const byte MerchWeapon = 9;
		public const byte MerchSkill = 10;
		public const byte MerchStone = 5;
		public const byte MerchTitle = 11;
		public const byte GuildNpcRequest = 12;
	}
}