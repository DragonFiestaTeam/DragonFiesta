// #define NO_BUS
using System;
using DragonFiesta.Configuration;
using DragonFiesta.Configuration.Sections;
using DragonFiesta.InterNetwork;
using DragonFiesta.Login.Core;
using DragonFiesta.Login.Networking;
using DragonFiesta.Networking;
using DragonFiesta.Util;
using DragonFiesta.Database;
using System.Xml.Serialization;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace DragonFiesta.Login
{
	public class Program
	{

		static void Main()
		{
			AppDomain.CurrentDomain.UnhandledException += UnhandledException;
			Logs.Main.Info("Server started.");
			Initialize();
			Logs.Main.Info("Server initialized.");
            while (true)
            {
                string inputString = Console.ReadLine();
                string[] cmdString = inputString.Split(' ');
                CmdCommandHandler.GetCommand(cmdString);
            }
		}

	    private static void UnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
		{
			Logs.Main.Fatal("Unhanled exception", unhandledExceptionEventArgs.ExceptionObject as Exception);
		}

		static void Initialize()
		{
		    try
		    {
                InitializeExceptionManager();
		        Random = new Random();
		        InitializeConfig();
                SetupDatabase();
                InternMessageManager.Initialize();
                InternMessageManager.StartListening();
		        LoginManager.Initialize();
		        WorldManager.Initialize();
		        HandlerStore.Initialize();
		        PacketProcessQueue.Initialize();
		        InitializeExternalIp();
		        ClientListener.Initialize();
		        LoginClientManager.Initialize();
                Console.WriteLine("loginserver sucess");
		    }
		    catch (Exception e)
		    {
		        Logs.Main.Fatal("Could not start server", e);
		        throw;
		    }
            //if(InitializeConfig())
            //    SetupEntity();
            //Random = new Random();
            //if(InitializeExceptionManager()
            //&& InitializeInterMessaging()
            //&& InitializeExternalIp()
            //&& LoginManager.Initialize()
            //&& WorldManager.Initialize()
            //&& HandlerStore.Initialize()
            //&& ClientManager.Initialize()
            //&& PacketProcessQueue.Initialize()
            //&& ClientListener.Initialize()
            //&& LoginClientManager.Initialize()
            //&& Database.DatabaseManager.Initialize())
            //{
            //    InternMessageManager.StartListening();
            //    Logs.Main.Info("Initialized successfully.");
            //}
            //else
            //{
            //    Logs.Main.Fatal("Failed to initialize.");
            //    Console.ReadLine();
            //    Environment.Exit(-1);
            //}
		}
        private static void SetupDatabase()
        {
            DatabaseSettingsSection db = LoginConfiguration.Instance.DatabaseSettings;
            if (db.DbOption == DatabaseOption.MySQL)//mysql
            {
                DB.LoginDB = new DatabaseManager(db.Host,db.DatabasePort, db.User, db.Password, db.DbName, db.MinPoolSize, db.MaxPoolSize, 1,1); 
            }
            else if (db.DbOption == DatabaseOption.MSSQL)//mssql
            {
                DB.LoginDB = new DatabaseManager(db.User,db.DbName, db.User, db.Password, db.MinPoolSize, db.MaxPoolSize, 1,1); 
            }
        }
        private static void InitializeConfig()
        {
            LoginConfiguration.Instance = new LoginConfiguration();

            if(LoginConfiguration.Instance.ReadXml())
            {
                Logs.Main.Info("Successfully read config.");
            }
            else
            {
                LoginConfiguration.Instance.WriteXml();
                Logs.Main.Info("Successfully created config");
            }
        }

        private static bool InitializeExceptionManager()
        {
            try
            {
                if(!ExceptionManager.Initialize())
                    return false;
                ExceptionHandlers.RegisterHandlers();
            }
            catch(Exception e)
            {
                Logs.Main.Fatal("Error initializing ExceptionManager", e);
                return false;
            }
            return true;
        }
        private static bool InitializeExternalIp()
        {
            try
            {
                var settings = Configuration.Configuration.Instance.ServerSettings;
                if (settings.AutoGetExternIP)
                    ExternalIp = HttpHelper.GetExternIp();
                else
                    ExternalIp = settings.ExternalIP;

                Logs.Main.InfoFormat("External IP is '{0}'", ExternalIp);
                return true;
            }
            catch (Exception e)
            {
                Logs.Main.Fatal("Could not get external IP", e);
                return false;
            }
        }
		public static Random Random { get; private set; }
        public static string ExternalIp { get; private set; }
	}
}