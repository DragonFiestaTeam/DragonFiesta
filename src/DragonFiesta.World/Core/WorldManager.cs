using System;
using System.Collections.Generic;
using System.Threading;
using DragonFiesta.Configuration;
using DragonFiesta.InterNetwork;
using DragonFiesta.Messages;
using DragonFiesta.Messages.Login;
using DragonFiesta.Messages.World;
using DragonFiesta.Util;

namespace DragonFiesta.World.Core
{
	public class WorldManager
	{
		#region .ctor
		public WorldManager()
		{
		}
		#endregion
		#region Properties
		public static WorldManager Instance { get; private set; }

		public int Id { get; private set; }
		public string Ip { get; private set; }
		public ushort Port { get; private set; }

        #endregion
		#region Methods
		public static bool Initialize()
		{
			try
			{
				Instance = new WorldManager();

			    Instance.Port = WorldConfiguration.Instance.ServerSettings.Port;
                if(WorldConfiguration.Instance.ServerSettings.AutoGetExternIP)
                {
                    Instance.Ip = HttpHelper.GetExternIp();
                }
                else
                {
                    Instance.Ip = WorldConfiguration.Instance.ServerSettings.ExternalIP;
                }

			    Instance.RegisterToLoginServer();
                Logs.Main.Info("Successfully initialized WorldManager");

			}
			catch(Exception e)
			{
				Logs.Main.Fatal("Could not initialize WorldManager", e);
				return false;
			}
			return true;
		}
		public void RegisterToLoginServer()
		{
		    Thread.Sleep(1000);         // test, might be neccesarry for waiting to register listeners
			var message = new NewWorldServerStarted();
		    var id = Guid.NewGuid();
			message.Id = id;
			message.Ip = this.Ip;
			message.Port = this.Port;
		    message.Callback = SetId;
		    InternMessageManager.Instance.Send(message);
		}
		public void SetId(IMessage pMessage)
		{
		    this.Id = ((WorldServerSetId) pMessage).YourId;
		    Logs.Main.DebugFormat("My ID is {0}", Id);
		}
		#endregion
	}
}
