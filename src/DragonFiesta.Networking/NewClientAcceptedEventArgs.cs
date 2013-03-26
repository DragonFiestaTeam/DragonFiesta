using System;
using System.Net.Sockets;

namespace DragonFiesta.Networking
{
	public class NewClientAcceptedEventArgs : EventArgs
	{
		public NewClientAcceptedEventArgs(Socket client)
		{
			Client = client;
		}
		public Socket Client { get; private set; }
	}
}