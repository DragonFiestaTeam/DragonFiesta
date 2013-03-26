using System;
using DragonFiesta.Networking;
using DragonFiesta.Configuration;
using DragonFiesta.Util;
using DragonFiesta.World.Core;
using DragonFiesta.InterNetwork;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace DragonFiesta.World
{
	public class Program
	{
		static void Main()
		{
            
			Initialize();
		    Console.ReadLine();
		}

        public static Random Random { get; private set; }

        protected static void Initialize()
        {
            try
            {
                InitializeSettings();
                InternMessageManager.Initialize();
                InternMessageManager.StartListening();
                HandlerStore.Initialize();
                ClientManager.Initialize();
                ClientListener.Initialize();
                WorldManager.Initialize();

                Logs.Main.Info("Successfully started server");
            }
            catch (Exception e)
            {
                Logs.Main.Fatal("Could not inialize server", e);
                throw;
            }
        }

        private static void InitializeSettings()
        {
            Configuration.Configuration.Instance = new WorldConfiguration();

            if (Configuration.Configuration.Instance.ReadXml())
            {
                Logs.Main.Info("Successfully read config.");
            }
            else
            {
                Configuration.Configuration.Instance.WriteXml();
                Logs.Main.Info("Successfully created config");
            }
        }
	}
}
