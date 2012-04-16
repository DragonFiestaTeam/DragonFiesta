using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Collections;

namespace SHNtoMySQLConverter.Database
{
    public class DatabaseHelper : SHNtoMySQLConverter.Connection.DatabaseHelper
    {
        public string conns { get; set; }
        #region Querys
        #endregion

        #region .ctor

        public DatabaseHelper(string connectionString)
            : base(connectionString)
        {
            conns = connectionString;
        }

        #endregion

        public static void Initialize(string connstring, string connectionname)
        {
            string connectionString = connstring;
            //Log.WriteLine(LogLevel.Info, connectionname + " Connection Initialize");
            Instance = new DatabaseHelper(connectionString);
        }
        public int KillSleepingConnections(int iMinSecondsToExpire)
        {
            string strSQL = "show processlist";

            System.Collections.ArrayList m_ProcessesToKill = new ArrayList();
            MySqlConnection myConn = new MySqlConnection(conns);

            MySqlCommand myCmd = new MySqlCommand(strSQL, myConn);
            MySqlDataReader MyReader = null;

            try
            {
                myConn.Open();

                // Get a list of processes to kill.
                MyReader = myCmd.ExecuteReader();
                while (MyReader.Read())
                {
                    // Find all processes sleeping with a timeout value higher than our threshold.
                    int iPID = Convert.ToInt32(MyReader["Id"].ToString());
                    string strState = MyReader["Command"].ToString();
                    int iTime = Convert.ToInt32(MyReader["Time"].ToString());

                    if (strState == "Sleep" && iTime >= iMinSecondsToExpire && iPID > 0)
                    {
                        // This connection is sitting around doing nothing. Kill it.
                        m_ProcessesToKill.Add(iPID);
                    }
                }

                MyReader.Close();

                foreach (int aPID in m_ProcessesToKill)
                {
                    strSQL = "kill " + aPID;
                    myCmd.CommandText = strSQL;
                    myCmd.ExecuteNonQuery();
                }
            }
            catch (Exception excep)
            {
            }
            finally
            {
                if (MyReader != null && !MyReader.IsClosed)
                {
                    MyReader.Close();
                }

                if (myConn != null && myConn.State == System.Data.ConnectionState.Closed)
                {
                    myConn.Close();
                }
            }

            return m_ProcessesToKill.Count;
        }
        public bool runSQL(string Syntax)
        {
            lock (Syntax)
            {
                // Log.WriteLine(LogLevel.Info, Syntax);
                try
                {
                    var conn = this.GetConnection();

                    MySqlCommand SqlCmd = new MySqlCommand(Syntax, conn);
                    SqlCmd.ExecuteNonQuery();

                    return true;

                }
                catch (MySqlException ex)
                {
                    Log.WriteLine(LogLevel.Exception, ex.Message.ToString());
                    //KillSleepingConnections(5);
                    return false;
                }
            }
        }
        public byte[] GetBlob(MySqlCommand pCommand)
        {
            byte[] retvalue;
            try
            {

                pCommand.Connection = this.GetConnection();
                pCommand.Prepare();
                retvalue = (byte[])pCommand.ExecuteScalar();


            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, "Error reading BLOB: {0} && {1}", ex.Message, ex.StackTrace);
                return null;
            }
            return retvalue;
        }

        public MySqlDataReader CreateReader(string query)
        {

            Log.WriteLine(LogLevel.Info, query);
            try
            {
                var conn = this.GetConnection();

                MySqlCommand mysqlCmd = new MySqlCommand(query, conn);
                MySqlDataReader mysqlReader = mysqlCmd.ExecuteReader();
                return mysqlReader;
            }
            catch (MySqlException ex)
            {

                Log.WriteLine(LogLevel.Exception, ex.Message.ToString());
                return null;

            }

        }

        public static DatabaseHelper Instance { get; private set; }
    }
}
