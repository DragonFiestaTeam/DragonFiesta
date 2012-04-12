using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragonFiestaDatatbaseConverter.ShineTable;

namespace DragonFiestaDatatbaseConverter.Handlers
{
  public  class ConvertExpHandler
    {
      public static void ConvertExpTable()
      {
          try
          {

              using (var tables = new ShineReader(Program.folder + @"\ChrCommon.txt"))
              {
                 bool  CreateTableExp = Database.DatabaseHelper.Instance.runSQL("CREATE TABLE `exptable` (`Level` tinyint(3) unsigned NOT NULL AUTO_INCREMENT,`Exp` bigint(20) unsigned DEFAULT NULL,PRIMARY KEY (`Level`));");
                 if (CreateTableExp)
                 {
                     using (DataTableReaderEx reader = new DataTableReaderEx(tables["StatTable"]))
                     {
                         while (reader.Read())
                         {
                             ushort lvl = reader.GetByte("level");
                             ulong exp = reader.GetUInt64("NextExp");
                             Database.DatabaseHelper.Instance.runSQL("INSERT INTO ExpTable (Level,Exp) VALUES ('" + lvl + "','" + exp + "');");
                         }
                     }
                     Log.WriteLine(LogLevel.Info, "ExpTable Convert Complit");
                 }
              }
          
          }
          catch (Exception ex)
          {
              Log.WriteLine(LogLevel.Exception, "Couldn't load EXP table: {0}", ex.ToString());
          }
      }

    }
}
