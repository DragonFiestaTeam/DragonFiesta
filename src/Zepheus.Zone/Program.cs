﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using Zepheus.Database;
using Zepheus.FiestaLib.Data;
using Zepheus.Util;
using Zepheus.Zone.InterServer;
using Zepheus.Zone.Networking;
using System.Security.Permissions;
using System.IO;
namespace Zepheus.Zone
{
    class Program
    {
        public static ZoneData serviceInfo { get { return Zones[0]; } set { Zones[0] = value; } }
        public static ConcurrentDictionary<byte, ZoneData> Zones { get; set; }
        //public static WorldEntity Entity { get; set; }
        public static Random Randomizer { get; set; }
        public static DateTime CurrentTime { get; set; }
        public static bool Shutdown { get; private set; }
        public static DatabaseManager DatabaseManager;
        public static DatabaseManager CharDBManager;
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
            Console.Title = "Zepheus.Zone[Registering]";
            // Lets wait a sec
            System.Threading.Thread.Sleep(10000);

            
            Zones = new ConcurrentDictionary<byte, ZoneData>();
            Zones.TryAdd(0, new ZoneData());

            if (Load())
            {
                // Start Worker thread.
                Worker.Load();

                while (true)
                {
                    string cmd = Console.ReadLine();
                    string[] arguments = cmd.Split(' ');
                    switch (arguments[0])
                    {
                        case "shutdown": 
                            Shutdown = true;
                            Log.WriteLine(LogLevel.Info, "Disconnecting from world.");
                            WorldConnector.Instance.Disconnect();
                            Log.WriteLine(LogLevel.Info, "Stopping client acceptor");
                            ZoneAcceptor.Instance.Stop();
                            Log.WriteLine(LogLevel.Info, "Stopping worker thread");
                            Worker.Instance.Stop();
                            Log.WriteLine(LogLevel.Info, "Disconnecting all clients");
                            ClientManager.Instance.DisconnectAll();
                            Log.WriteLine(LogLevel.Info, "Saving everything a last time");
                           // Entity.SaveChanges();
                            Log.WriteLine(LogLevel.Info, "Bay.");
                            Environment.Exit(1);
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("There was an error during load. Please press RETURN to exit.");
                Console.ReadLine();
            }
        }

        private static bool Load()
        {

            Zepheus.InterLib.Settings.Initialize();
            Settings.Load();
            DatabaseServer dbServer = new DatabaseServer(Settings.Instance.zoneMysqlServer, (uint)Settings.Instance.zoneMysqlPort, Settings.Instance.zoneMysqlUser, Settings.Instance.zoneMysqlPassword);
            Database.Database db = new Database.Database(Settings.Instance.zoneMysqlDatabase, Settings.Instance.ZoneDBMinPoolSize, Settings.Instance.ZoneDBMinPoolSize);
            DatabaseManager = new DatabaseManager(dbServer, db);
            DatabaseManager.GetClient(); //testclient
            Database.Database charDB = new Database.Database(Settings.Instance.WorldMysqlDatabase, Settings.Instance.ZoneDBMinPoolSize, Settings.Instance.ZoneDBMinPoolSize);
            CharDBManager = new DatabaseManager(dbServer, charDB);
            CharDBManager.GetClient();
            CharDBManager.StartClientMonitor();
            DatabaseManager.StartClientMonitor();
            Log.SetLogToFile(string.Format(@"Logs\Zone\{0}.log", DateTime.Now.ToString("yyyy-MM-dd HHmmss")));
            Randomizer = new Random();
            Log.IsDebug = Settings.Instance.Debug;
      
            try
            {
                if (Reflector.GetInitializerMethods().Any(method => !method.Invoke()))
                {
                    Log.WriteLine(LogLevel.Error, "Server could not be started. Errors occured.");
                    return false;
                }
                else return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, "Error loading Initializer methods: {0}", ex.ToString());
                return false;
            }
        }
        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;

            #region Logging
            #region Write Errors to a log file
            // Create a writer and open the file:
            StreamWriter log;

            if (!File.Exists("errorlog.txt"))
            {
                log = new StreamWriter("errorlog.txt");
            }
            else
            {
                log = File.AppendText("errorlog.txt");
            }

            // Write to the file:
            log.WriteLine(DateTime.Now);
            log.WriteLine(e.ToString());
            log.WriteLine();

            // Close the stream:
            log.Close();
            #endregion
            #endregion

          Log.WriteLine(LogLevel.Exception,"Unhandled Exception : " + e.ToString());
            Console.ReadKey(true);
        }
        public static ZoneData GetZoneForMap(ushort mapid)
        {
            foreach (var v in Zones.Values)
            {
                if (v.MapsToLoad.Count(m => m.ID == mapid) > 0) return v;
            }
            return null;
        }

        public static MapInfo GetMapInfo(ushort mapid)
        {
            foreach (var v in Zones.Values)
            {
                MapInfo mi = v.MapsToLoad.Find(m => m.ID == mapid);
                if (mi != null)
                {
                    return mi;
                }
            }
            return null;
        }

        public static bool IsLoaded(ushort mapid)
        {
            try
            {
                return serviceInfo.MapsToLoad.Count(m => m.ID == mapid) > 0;
            }
            catch { return false; }
        }
    }
}
