using System;
using DragonFiesta.InterNetwork;

namespace DragonFiesta.World.Messages
{
	public struct SetZoneServerId : IMessage
	{
		public Guid Id { get; set; }
		public ushort NewId { get; private set; }
	}
}