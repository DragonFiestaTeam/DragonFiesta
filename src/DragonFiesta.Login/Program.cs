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
                SetupEntity();
                InternMessageManager.Initialize();
                InternMessageManager.StartListening();
		        LoginManager.Initialize();
		        WorldManager.Initialize();
		        HandlerStore.Initialize();
		        PacketProcessQueue.Initialize();
		        InitializeExternalIp();
		        ClientListener.Initialize();
		        LoginClientManager.Initialize();
		        Database.DatabaseManager.Initialize();
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
        private static void SetupEntity()
        {
            DatabaseSettingsSection db = LoginConfiguration.Instance.DatabaseSettings;
            if (db.DbOption == DatabaseOption.MySQL)//mysql
            {
                LoginConfiguration.Instance.Entity = new EntitySetting()
                {
                    DatabaseName = db.DbName,
                    Password = db.Password,
                    Username = db.User,
                    host = db.Host,
                    Metadata = "res://*/",//lol? so its works.. NOTE: this means empty metadata btw.
                                            // NOTE - metdata is always neccessary to define but MySQL does not need any.
                    ProviderName = "MySql.Data.MySqlClientFactory",
                    Option = 1,
                };
            }
            else if (db.DbOption == DatabaseOption.MSSQL)//mssql
            {
                LoginConfiguration.Instance.Entity = new EntitySetting()
                {
                    DataCatalog = db.DbName,
                    Username = db.User,
                    Password = db.Password,
                    DataSource = db.Host,
                    ProviderName = "System.Data.SqlClient",
                    Metadata = @"res://DragonFiesta.Database/EF_Models.Login.csdl|res://DragonFiesta.Database/EF_Models.Login.ssdl|res://DragonFiesta.Database/EF_Models.Login.msl",
                };
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