using System.Net.Sockets;
using DragonFiesta.Login.DataTypes;
using DragonFiesta.Networking;
using System;
using DragonFiesta.Util;

namespace DragonFiesta.Login.Networking
{
	public class LoginClient : ClientBase
	{
		#region .ctor

		public LoginClient(Socket socket) : base(socket)
		{
            this.OnDisconnect += new EventHandler<EventArgs>(pOnDisconnect);
		}

		#endregion
		#region Methods


        void pOnDisconnect(object sender, EventArgs e)
        {
            Socket sock = sender as Socket;
            Logs.Main.Debug("disconnect " + base.IP+base.Port + "");
            base.Dispose();
     
        }
		#endregion
		#region Properties

		public bool SentWorldListAlready { get; set; }
		public bool Authed { get; set; }
        public bool HashesAllowed { get; set; }
		public string Username { get; set; }
        public int AccountID { get; set; }
        public byte Access_level { get; set; }
		public LoginVersion Version { get; set; }

		#endregion
	}
}