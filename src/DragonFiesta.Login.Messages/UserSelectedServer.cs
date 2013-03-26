using System;
using DragonFiesta.InterNetwork;

namespace DragonFiesta.Login.Messages
{
	public class UserSelectedServer : IMessage
	{
	    public Guid Id { get; set; }
		public Guid AuthKey { get; set; }
		public ushort ServerId { get; set; }
	}
}