using DragonFiesta.Messages;
using DragonFiesta.Messages.Login;
using DragonFiesta.Util;
using DragonFiesta.World.Core;
using DragonFiesta.InterNetwork;

namespace DragonFiesta.World.Networking
{
	public static class InterHandler 
	{
        [InternMessageHandler(typeof(WorldServerSetId))]
		public static void HandleSeverSetId(IMessage pMessage)
        {
            /* HANDLED BY CALLBACK */
		}
        [InternMessageHandler(typeof(UserSelectedServer))]
        public static void HandleUserSelectedServer(IMessage pMessage)
        {
            var message = (UserSelectedServer) pMessage;
	        if(message.ServerId != WorldManager.Instance.Id)
	            return;          // message not for me
	        Logs.InterNetwork.Debug("User selected me. lol");
	        // TODO: what now?
	    }

	}
}