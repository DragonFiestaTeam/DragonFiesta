using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHNtoMySQLConverter
{
    public class CmdHandler
    {
        public static void GetcmdCommand(string cmd)
        {
            switch (cmd)
            {
                case "Help":
                    break;
                case "exp":
                    //ConvertExpHandler.ConvertExpTable();
                    break;
                case "Recall":
                    //RecallConvert.ConvertRecallCoordinates();
                    break;
                case "Minihouse":
                    //ConvertMinihouse.ConvertMinihouses();
                    break;
                case "Map":
                    //ConvertMaps.LoadMaps();
                    break;
                case "mysql":

                    int count = Database.DatabaseHelper.Instance.KillSleepingConnections(100);
                    Console.WriteLine(count);
                    break;
                default:
                    Console.WriteLine("Help for CommandLine");
                    break;
            }
        }
    }
}
