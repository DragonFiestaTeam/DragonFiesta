using System;
using DragonFiesta.InterNetwork;

namespace DragonFiesta.Login.Messages
{
	public class NewWorldServerStarted : IMessage, IExpectAnAnswer
	{
		public Guid Id { get; set; }
		public string Ip { get; set; }
		public ushort Port { get; set; }
	    public Action<IMessage> Callback { get; set; }
	}
}