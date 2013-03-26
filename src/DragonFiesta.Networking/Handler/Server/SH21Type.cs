namespace DragonFiesta.Networking.Handler.Server
{
	public class SH21Type : PacketHeader
	{
		public new const byte _Header = 21;
		public const byte FriendListDelete = 6;
		public const byte FriendInviteResponse = 2;
		public const byte FriendInviteRequest = 3;
		public const byte FriendExtraInformation = 8;
		public const byte FriendOnline = 9;
		public const byte FriendOffline = 10;
		public const byte FriendInviteReject = 11;
		public const byte FriendDeleteSend = 12;
		public const byte FriendChangeMap = 13;
	}
}