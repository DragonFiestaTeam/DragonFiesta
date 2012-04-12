using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragonFiestaDatatbaseConverter.Handlers;

namespace DragonFiestaDatatbaseConverter
{
   public class Program
    {
       public static string folder { get; set; }
    
        static void Main(string[] args)
        {
            if (Settings.Initialize())
            {
                Database.DatabaseHelper.Initialize("User ID=root;Password=1234567;Host=localhost;Port=3306;Database=Data;Protocol=TCP;Compress=false;Pooling=true;Min Pool Size=0;Max Pool Size=2000;Connection Lifetime=0;", "WorkerConn");
                Log.WriteLine(LogLevel.Info, "Welcome To DragonFiestaDatatbase Converter");
                Log.WriteLine(LogLevel.Info, "Please Insert Datanamefolder in the console");
                string Folder = Console.ReadLine();
                folder = Folder;
                while (true)
                {
                    string cmd = Console.ReadLine();
                    CmdHandler.GetcmdCommand(cmd);
                }
            }
            else
            {
                Log.WriteLine(LogLevel.Error, "Settings Initilize Failed");
                Console.ReadLine();
            }
        }
            
    }
}
