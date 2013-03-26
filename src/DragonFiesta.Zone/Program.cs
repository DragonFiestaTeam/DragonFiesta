using System;
using DragonFiesta.Configuration;
using DragonFiesta.InterNetwork;
using DragonFiesta.Networking;
using DragonFiesta.Util;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace DragonFiesta.Zone
{
    class Program
    {
        static void Main()
        {
            Initialize();
        }
		public static void Initialize()
		{
		    try
		    {
		        InitializeConfig(0);
                InternMessageManager.Initialize();
                HandlerStore.Initialize();
		        PacketProcessQueue.Initialize();
		    }
		    catch (Exception e)
		    {
		        Logs.Main.Fatal("Couldnot initialize server.", e);
		        throw;
		    }
		}

        private static void InitializeConfig(byte pZoneId)
        {
            ZoneConfiguration.Instance = new ZoneConfiguration(pZoneId);

            if (ZoneConfiguration.Instance.ReadXml())
            {
                Logs.Main.Info("Successfully read config.");
            }
            else
            {
                ZoneConfiguration.Instance.WriteXml();
                Logs.Main.Info("Successfully created config.");
            }
        }
    }
}
