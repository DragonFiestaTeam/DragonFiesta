using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Collections;

namespace SHNtoMySQLConverter.Connection
{
    public class DatabaseHelper
    {
        #region .ctor
        public DatabaseHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }
        ~DatabaseHelper()
        {
        }
        #endregion

        #region Methods

        public MySqlConnection GetConnection()
        {

            MySqlConnection conn = new MySqlConnection(connectionString);
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

        #endregion

        #region Propertys
        #endregion

        #region Variables

        private string connectionString;

        #endregion
    }

}
