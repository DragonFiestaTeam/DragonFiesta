namespace DragonFiesta.Networking.Handler.Client
{
	public class CH21Type : PacketHeader
	{
		public new const byte _Header = 21;
		public  const byte FriendInvite = 1;
		public  const byte InviteResponse = 4;
		public  const byte FriendListDelete = 5;
	}
}