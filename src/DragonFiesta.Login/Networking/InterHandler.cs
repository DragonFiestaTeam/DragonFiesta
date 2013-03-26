using System;
using DragonFiesta.InterNetwork;
using DragonFiesta.Messages;
using DragonFiesta.Messages.Login;
using DragonFiesta.Messages.World;
using DragonFiesta.Util;

namespace DragonFiesta.Login.Core
{
	public class InterHandler
	{
        [InternMessageHandler(typeof(NewWorldServerStarted))]
		public static void HandleNewWorldServer(IMessage pMessage)
		{
		    NewWorldServerStarted message = (NewWorldServerStarted) pMessage;
			Logs.InterNetwork.DebugFormat("New WorldServer started under {0}", message.Ip);
		    int id = WorldManager.Instance.RegisterServer(message);

		    WorldServerSetId msg = new WorldServerSetId()
		                               {
		                                   Id = message.Id,
		                                   YourId = id,
		                               };
            InternMessageManager.Instance.Send(msg);
		    Logs.InterNetwork.DebugFormat("Set his id to {0}", id);
		}
	}
}