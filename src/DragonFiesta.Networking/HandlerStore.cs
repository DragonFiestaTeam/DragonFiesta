using System;
using System.Collections.Generic;
using System.Reflection;
using DragonFiesta.Util;

namespace DragonFiesta.Networking
{
	public class HandlerStore	
	{
		public static void Initialize()
		{
			Instance = new HandlerStore
				{
					handler = Reflector.GivePacketMethods(),
				};
		}

		public HandlerStore()
		{
			handler = new Dictionary<byte, Dictionary<byte, MethodInfo>>();
		}
		public bool Handle(ClientBase sender, Packet packet)
		{
			if (!handler.ContainsKey(packet.Header))
				return false;
			if (!handler[packet.Header].ContainsKey(packet.Type))
				return false;

			handler[packet.Header][packet.Type].Invoke (null, new object[]{sender, packet});
			return true;
		}

		public static HandlerStore Instance { get; private set; }
		private Dictionary<byte, Dictionary<byte, MethodInfo>> handler;
	}
}