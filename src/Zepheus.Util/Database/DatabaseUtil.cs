using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;

namespace DragonFiesta.Util
{
    public class DatabaseUtil
    {
        public static bool TestDatabaseConnection(string ConnectionString)
        {
            try
            {
                using (EntityConnection Conn = new EntityConnection(ConnectionString))
                {
                    Conn.Open();
                    Console.WriteLine("Testing  DatabaseConnection");
                    Conn.Close();
                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Connection Failure wirh string : " + ConnectionString + "");
                return true;
            }
        }
    }
}
