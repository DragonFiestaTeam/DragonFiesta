using System;
using DragonFiesta.Networking;
using DragonFiesta.Configuration;
using DragonFiesta.Util;
using DragonFiesta.World.Core;
using DragonFiesta.World.Networking;
using DragonFiesta.InterNetwork;
using DragonFiesta.Database;
using DragonFiesta.Configuration.Sections;
using DragonFiesta.Data;
using DragonFiesta.Data.Transfer;

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
                Random = new Random();
                System.Threading.Thread.Sleep(1000);
                InitializeSettings();
                SetupDatabase();
                DataProvider.DataProvider.Initialize(DB.GameDB);
                ZoneManager.Initialize();
                InternMessageManager.Initialize();
                InternMessageManager.StartListening();
                PacketProcessQueue.Initialize();
                HandlerStore.Initialize();
                ClientManager.Initialize();
                ClientListener.Initialize();
                ClientTransferManager.Initialize();
                WorldManager.Initialize();
                EventManager.Initialize();
                WorldClientManager.Initialize();
                new BackgroundWorker();
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

        private static void SetupDatabase()
        {
            DatabaseSettingsSection db = LoginConfiguration.Instance.WorldAndGameSettings;
            if (db.DbOption == DatabaseOption.MySQL)//mysql
            {
                DB.WorldDB = new DatabaseManager(db.Host, db.DatabasePort, db.User, db.Password, db.WorldDb, db.MinPoolSize, db.MaxPoolSize, 1, 1);
                DB.GameDB = new DatabaseManager(db.Host, db.DatabasePort, db.User, db.Password, db.GameDB, db.MinPoolSize, db.MaxPoolSize, 1, 1);
            }
            else if (db.DbOption == DatabaseOption.MSSQL)//mssql
            {
                DB.WorldDB = new DatabaseManager(db.User, db.WorldDb, db.User, db.Password, db.MinPoolSize, db.MaxPoolSize, 1, 1);
                DB.GameDB = new DatabaseManager(db.User, db.GameDB, db.User, db.Password, db.MinPoolSize, db.MaxPoolSize, 1, 1);
            }
        }
	}
}
