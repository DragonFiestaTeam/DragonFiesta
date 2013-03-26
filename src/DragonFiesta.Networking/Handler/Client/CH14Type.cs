namespace DragonFiesta.Networking.Handler.Client
{
	public class CH14Type :PacketHeader
	{
		public new const byte _Header = 14;
		public const byte PartyAccept = 4;
		public const byte PartyDecline = 5;
		public const byte PartyRequest = 2;
		public const byte PartyLeave = 10;
		public const byte PartyMaster = 84;
		public const byte PartyInviteGame = 72;
		public const byte ChangePartyMaster = 40;
		public const byte ChangePartyDrop = 75;
		public const byte KickPartyMember = 20;
	}
}