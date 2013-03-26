using System;
using DragonFiesta.InterNetwork;

namespace DragonFiesta.Messages.Login
{
    [Serializable]
	public class UserSelectedServer : IMessage
	{
	    public Guid Id { get; set; }
		public Guid AuthKey { get; set; }
		public ushort ServerId { get; set; }
	}
}