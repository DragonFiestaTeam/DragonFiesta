using System.Net.Sockets;
using DragonFiesta.Login.DataTypes;
using DragonFiesta.Networking;

namespace DragonFiesta.Login.Networking
{
	public class LoginClient : ClientBase
	{
		#region .ctor

		public LoginClient(Socket socket) : base(socket)
		{
		}

		#endregion
		#region Methods


		#endregion
		#region Properties

		public bool SentWorldListAlready { get; set; }
		public bool Authed { get; set; }
        public bool HashesAllowed { get; set; }
		public string Username { get; set; }
		public LoginVersion Version { get; set; }

		#endregion
	}
}