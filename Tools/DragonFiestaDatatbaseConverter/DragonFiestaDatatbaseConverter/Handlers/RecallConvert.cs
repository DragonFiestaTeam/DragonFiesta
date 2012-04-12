using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragonFiestaDatatbaseConverter.Data;
using System.IO;
using DragonFiestaDatatbaseConverter.ShineTable;

namespace DragonFiestaDatatbaseConverter.Handlers
{
    public class RecallConvert
    {
        public static void ConvertRecallCoordinates()
        {
            if (!File.Exists(Program.folder + @"\RecallCoordinates.txt"))
            {
                Log.WriteLine(LogLevel.Warn, "Could not find RecallCoordinates.txt, return scrolls won't work.");
                return;
            }

            using (var data = new ShineReader(Program.folder + @"\RecallCoordinates.txt"))
            {

                var recallData = data["RecallPoint"];

                using (var reader = new DataTableReaderEx(recallData))
                {
                    if (Database.DatabaseHelper.Instance.runSQL("CREATE TABLE `recall` (`ItemIndex` varchar(255) NOT NULL,`MapName` varchar(255) NOT NULL,`LinkX` int(11) NOT NULL,`LinkY` int(11) NOT NULL, PRIMARY KEY (`ItemIndex`));"))
                    {
                        while (reader.Read())
                        {
                            var rc = RecallCoordinate.Load(reader);
                            Database.DatabaseHelper.Instance.runSQL("INSERT INTO Recall (ItemIndex,MapName,LinkX,LinkY) VALUES ('" + rc.ItemIndex + "','" + rc.MapName + "','" + rc.LinkX + "','" + rc.LinkY + "');");
                        }
                    }
                    Log.WriteLine(LogLevel.Info, "Recall Convert Complett");
                }
            }
        }

    }
}
