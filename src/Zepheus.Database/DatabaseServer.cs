namespace Zepheus.Database
{
	public class DatabaseServer
	{
		public string Hostname;
		public uint Port;

		public string Username;
		public string Password;

		public DatabaseServer(string hostname, uint port, string username, string password)
		{
			Hostname = hostname;
			Port = port;
			Username = username;
			Password = password;
		}
	}
}