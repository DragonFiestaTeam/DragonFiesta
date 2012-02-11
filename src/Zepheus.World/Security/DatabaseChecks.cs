using System.Linq;
using MySql.Data.MySqlClient;
using Zepheus.Database.Storage;
using Zepheus.Database;
using System.Data;

namespace Zepheus.World.Security
{
    public sealed class DatabaseChecks
    {
        public static bool IsCharNameUsed(string name)
        {
            DataTable Data = null;
            using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
            {
                Data = dbClient.ReadDataTable("Select CharID from characters  WHERE binary Name='" + name + "'");
            }
            if (Data != null)
            {
                if (Data.Rows.Count == 1)
                {
                    return true;
                }
                else
                {
                    if (Data.Rows.Count == 0)
                        return false;
                }
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}

