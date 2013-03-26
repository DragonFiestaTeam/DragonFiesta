using System;

namespace DragonFiesta.Util
{
	[AttributeUsage(AttributeTargets.Class)]
	public class PacketHandlerClassAttribute : Attribute
	{
		public PacketHandlerClassAttribute(byte header)
		{
			this.Header = header;
		}

		public byte Header { get; private set; }
	}
	[AttributeUsage(AttributeTargets.Method)]
	public class PacketHandlerAttribute : Attribute
	{
		public PacketHandlerAttribute(byte opcode)
		{
			OpCode = opcode;
		}

		public byte OpCode { get; private set; }
	}
}
