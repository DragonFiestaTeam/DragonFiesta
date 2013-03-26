namespace DragonFiesta.Networking.Handler.Client
{
	public class CH38Type : PacketHeader
	{
		public new const byte _Header = 38;
		public  const byte GetAcademyListRequest = 7;
		public  const byte AcademyRequestList = 13;
		public  const byte AcademyRequest = 17;
		public  const byte KickMember = 22;
		public  const byte AcademyLeave = 27;
		public  const byte JumpToMember = 31;
		public  const byte BlockAcademyChat = 33;
		public  const byte ChangeDetails = 36;
		public  const byte ChangeFromAcademyToGuild = 41;
		public  const byte ChangeRequestAnswer = 44;
		public  const byte AcademyChatMessage = 104;
	}
}