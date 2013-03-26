namespace DragonFiesta.Networking.Handler.Client
{
	public class CH4Type : PacketHeader
	{
		public new const byte _Header = 4;
		public const byte CharSelect = 1;
		public const byte ReviveToTown = 78;
		public const byte SetPointOnStat = 92;
	}
}