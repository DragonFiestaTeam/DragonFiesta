using System;
using DragonFiesta.Login.Networking.ServerHandler;
using DragonFiesta.Networking;
using DragonFiesta.Util;

namespace DragonFiesta.Login.Networking
{
    public class LoginClientManager : ClientManager
    {
        #region .ctor

        public LoginClientManager() : base()
        {
            // Note: add any logic here
        }

        #endregion
        #region Properties

        public new static LoginClientManager Instance { get; private set; }

        #endregion
        #region Methods

        public new static void Initialize()
        {
            Instance = new LoginClientManager();
            ClientListener.Instance.NewClientAccepted += Instance.NewClient;
        }


		public override void RegisterClient(ClientBase pClient)
		{
			base.RegisterClient(pClient);
			short pos = (short) Program.Random.Next(0, 480);
		    pClient.XorPosition = (ushort) pos;
			SH2Methods.SetXorPosition(pClient, pos);
		} 

        private void NewClient(object sender, NewClientAcceptedEventArgs newClientAcceptedEventArgs)
        {
            var client = new LoginClient(newClientAcceptedEventArgs.Client);

            this.RegisterClient(client);
        }

        #endregion
    }
}