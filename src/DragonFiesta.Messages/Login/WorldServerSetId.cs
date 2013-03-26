using System;
using DragonFiesta.InterNetwork;

namespace DragonFiesta.Messages.Login
{
    [Serializable]
	public class WorldServerSetId : IMessage
	{
		public Guid Id { get; set; }
		public int YourId { get; set; }
	}
}