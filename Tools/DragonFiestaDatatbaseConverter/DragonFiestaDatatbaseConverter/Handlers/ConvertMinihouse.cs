using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragonFiestaDatatbaseConverter.Data;
using DragonFiestaDatatbaseConverter.SHN;

namespace DragonFiestaDatatbaseConverter.Handlers
{
   public class ConvertMinihouse
    {
       public static void ConvertMinihouses()
       {
           using (var file = new SHNFile(Program.folder + @"\MiniHouse.shn"))
           {
               if (Database.DatabaseHelper.Instance.runSQL("CREATE TABLE `minihouse` (`ItemID` int(7) NOT NULL DEFAULT '0',`KeepTime_Hour` smallint(5) unsigned NOT NULL,`HPTick` smallint(5) unsigned NOT NULL,`SPTick` smallint(5) unsigned NOT NULL,`HPRecovery` smallint(5) unsigned NOT NULL,`SPRecovery` smallint(5) unsigned NOT NULL,PRIMARY KEY (`ItemID`))"))
               {
                   using (DataTableReaderEx reader = new DataTableReaderEx(file))
                   {
                       while (reader.Read())
                       {
                           MiniHouseInfo mhi = new MiniHouseInfo(reader);
                           Database.DatabaseHelper.Instance.runSQL("INSERT INTO minihouse (ItemID,KeepTime_Hour,HPTick,SPTick,HPRecovery,SPRecovery) VALUES ('" + mhi.ID + "','" + mhi.KeepTime_Hour + "','" + mhi.HPTick + "','" + mhi.SPTick + "','" + mhi.HPRecovery + "','" + mhi.SPRecovery + "');");
                       }
                   }
               }
           }
           Log.WriteLine(LogLevel.Info, "Mini Houses. Convert Complett");
       }
    }
}
