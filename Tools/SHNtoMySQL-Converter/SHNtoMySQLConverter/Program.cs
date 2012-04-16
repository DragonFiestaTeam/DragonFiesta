using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SHNtoMySQLConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            string version = "v0.9";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "SHN to MySQL Database Converter for DragonProject Fiesta "+version+" by CoolyT";
            Console.WriteLine("SHN to MySQL Database Converter for DragonProject Fiesta v0.1 by CoolyT");
            Console.ForegroundColor = ConsoleColor.White;
            Converter convert = new Converter();
            Settings.Initialize();
            Console.WriteLine(); Console.WriteLine("Using Settings :");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Server [{0}] \nPort [{1}] \nUser [{2}] \nDatabase [{3}]", Settings.GetString("MySql.Server"), Settings.GetInt16("MySql.Port"), Settings.GetString("MySql.User"), Settings.GetString("MySql.Database"));
            Console.ForegroundColor = ConsoleColor.White;
            Database.DatabaseHelper.Initialize("User ID="+Settings.GetString("MySql.User")+";Password="+Settings.GetString("MySql.Password")+";Host="+Settings.GetString("MySql.Server")+";Port="+Settings.GetInt16("MySql.Port")+";Database="+Settings.GetString("MySql.Database")+";Protocol=TCP;Compress=false;Pooling=true;Min Pool Size=0;Max Pool Size=2000;Connection Lifetime=0;", "WorkerConn");
            Console.WriteLine();
            convert.MainConvert();
            while (true)
            {
                string cmd = Console.ReadLine();
                CmdHandler.GetcmdCommand(cmd);
            }
        }

    }
}

