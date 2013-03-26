namespace DragonFiesta.Networking.Handler.Client
{
	public class CH37Type : PacketHeader
	{
		public new const byte _Header = 37;
		public  const byte MasterRequest = 1;
		public  const byte MasterRequestResponse = 5;
		public  const byte RemoveMasterByApprentice = 6;
		public  const byte MasterRemove = 10;
		public  const byte MasterRewardCoperRequest = 60;
		public  const byte SendReceiveCoperAccept = 64;
	}
}