using DragonFiesta.FiestaLib;

namespace DragonFiesta.Login.DataTypes
{
	public class WorldServerInfo
	{
		public WorldServerInfo(int id, string name, string ip, ushort port, WorldStatus status)
		{
			ID = id;
			Name = name;
			Ip = ip;
			Port = port;
			Status = status;
		}

		public void UpdateStatus(WorldStatus pStatus)
		{
			Status = pStatus;
		}

		public int ID { get; private set; }
		public string Name { get; private set; }
		public string Ip { get; private set; }
		public ushort Port { get; private set; }
		public WorldStatus Status { get; private set; }
	}
}