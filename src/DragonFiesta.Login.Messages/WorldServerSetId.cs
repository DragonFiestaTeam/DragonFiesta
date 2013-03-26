using System;
using DragonFiesta.InterNetwork;

namespace DragonFiesta.Login.Messages
{
	public class WorldServerSetId : IMessage
	{
		public Guid Id { get; set; }
		public int YourId { get; set; }
	}
}