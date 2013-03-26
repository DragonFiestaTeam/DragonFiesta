using System;

namespace DragonFiesta.Networking
{
	public class NewClientEventArgs : EventArgs
	{
		public NewClientEventArgs(ClientBase pClient)
		{
			Client = pClient;
		}

		public ClientBase Client { get; private set; }
	}
}