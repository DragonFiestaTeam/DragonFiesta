namespace DragonFiesta.Networking.Handler.Server
{
	public class SH37Type : PacketHeader
	{
		public new const byte _Header = 37;
		public const byte SendMasterRequestAccept = 3;
		public const byte SendMasterRequestResponse = 2;
		public const byte SendMasterRequest = 4;
		public const byte SendMasterResponseRemove = 7;
		public const byte SendRemoveMember = 11;
		public const byte SendMasterList = 20;
		public const byte SendRegisterApprentice = 21;
		public const byte SendMasterMemberOnline = 22;
		public const byte SendMasterMemberOffline = 23;
		public const byte SendApprenticeRemoveMaster = 24;
		public const byte SendApprenticeLevelUp = 25;
		public const byte SendApprenticeReward = 26;
		public const byte SendReceiveCopper = 61;
		public const byte SendGiveMasterReward = 65;
		public const byte MasterReceiveCopperDecline = 69;
	}
}