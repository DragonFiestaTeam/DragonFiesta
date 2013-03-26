using System;
using DragonFiesta.InterNetwork;

namespace DragonFiesta.Messages.World
{
    [Serializable]
	public struct SetZoneServerId : IMessage
	{
		public Guid Id { get; set; }
		public ushort NewId { get; private set; }
	}
}