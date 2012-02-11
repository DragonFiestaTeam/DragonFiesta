using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;
using Zepheus.Util;

namespace Zepheus.Database
{
  public  class DatabaseManager
    {
        public DatabaseServer Server;
        public Database Database;

        public DatabaseClient[] Clients;
        public Boolean[] AvailableClients;
        public int ClientStarvation;

        public Thread ClientMonitor;

        public string ConnectionString
        {
            get
            {
                MySqlConnectionStringBuilder ConnString = new MySqlConnectionStringBuilder();

                ConnString.Server = Server.Hostname;
                ConnString.Port = Server.Port;
                ConnString.UserID = Server.Username;
                ConnString.Password = Server.Password;
                ConnString.Database = Database.DatabaseName;
                ConnString.MinimumPoolSize = Database.PoolMinSize;
                ConnString.MaximumPoolSize = Database.PoolMaxSize;

                return ConnString.ToString();
            }
        }

        public DatabaseManager(DatabaseServer _Server, Database _Database)
        {
            Server = _Server;
            Database = _Database;

            Clients = new DatabaseClient[0];
            AvailableClients = new Boolean[0];
            ClientStarvation = 0;

            StartClientMonitor();
        }

        public void DestroyClients()
        {
            lock (this.Clients)
            {
                for (int i = 0; i < Clients.Length; i++)
                {
                    Clients[i].Destroy();
                    Clients[i] = null;
                }
            }
        }

        public void DestroyDatabaseManager()
        {
            StopClientMonitor();

            lock (this.Clients)
            {
                for (int i = 0; i < Clients.Length; i++)
                {
                    try
                    {
                        Clients[i].Destroy();
                        Clients[i] = null;
                    }
                    catch (NullReferenceException) { }
                }
            }

            Server = null;
            Database = null;
            Clients = null;
            AvailableClients = null;
        }

        public void StartClientMonitor()
        {
            if (ClientMonitor != null)
            {
                return;
            }

            ClientMonitor = new Thread(MonitorClients);
            ClientMonitor.Name = "DB Monitor";
            ClientMonitor.Priority = ThreadPriority.Lowest;
            ClientMonitor.Start();
        }

        public void StopClientMonitor()
        {
            if (ClientMonitor == null)
            {
                return;
            }

            try
            {
                ClientMonitor.Abort();
            }

            catch (ThreadAbortException) { }

            ClientMonitor = null;
        }

        public void MonitorClients()
        {
            while (true)
            {
                try
                {
                    lock (this.Clients)
                    {
                        DateTime DT = DateTime.Now;

                        for (int i = 0; i < Clients.Length; i++)
                        {
                            if (Clients[i].State != ConnectionState.Closed)
                            {
                                if (Clients[i].InactiveTime >= 45) // Not used in the last %x% seconds
                                {
                                    Clients[i].Disconnect();
                                }
                            }
                        }
                    }

                    Thread.Sleep(10000); // 10 Seconds
                }

                catch (ThreadAbortException) { }

                catch (Exception e)
                {
                Console.WriteLine("An error occured in database manager client monitor: " + e.Message);
                }
            }
        }

        public DatabaseClient GetClient()
        {
            lock (this.Clients)
            {
                lock (this.AvailableClients)
                {
                    for (uint i = 0; i < Clients.Length; i++)
                    {
                        if (AvailableClients[i] == true)
                        {
                            ClientStarvation = 0;

                            if (Clients[i].State == ConnectionState.Closed)
                            {
                                try
                                {
                                    Clients[i].Connect();
                                }

                                catch (Exception e)
                                {
                                    Log.WriteLine(LogLevel.Error, "Could not get database client: " + e.Message);
                                }
                            }

                            if (Clients[i].State == ConnectionState.Open)
                            {
                                AvailableClients[i] = false;

                                Clients[i].UpdateLastActivity();
                                return Clients[i];
                            }
                        }
                    }
                }

                ClientStarvation++;

                if (ClientStarvation >= ((Clients.Length + 1) / 2))
                {
                    ClientStarvation = 0;
                    SetClientAmount((uint)(Clients.Length + 1 * 1.3f));
                    return GetClient();
                }

                DatabaseClient Anonymous = new DatabaseClient(0, this);
                Anonymous.Connect();

                return Anonymous;
            }
        }

        public void SetClientAmount(uint Amount)
        {
            lock (this.Clients)
            {
                lock (this.AvailableClients)
                {
                    if (Clients.Length == Amount)
                    {
                        return;
                    }

                    if (Amount < Clients.Length)
                    {
                        for (uint i = Amount; i < Clients.Length; i++)
                        {
                            Clients[i].Destroy();
                            Clients[i] = null;
                        }
                    }

                    DatabaseClient[] _Clients = new DatabaseClient[Amount];
                    bool[] _AvailableClients = new bool[Amount];

                    for (uint i = 0; i < Amount; i++)
                    {
                        if (i < Clients.Length)
                        {
                            _Clients[i] = Clients[i];
                            _AvailableClients[i] = AvailableClients[i];
                        }
                        else
                        {
                            _Clients[i] = new DatabaseClient((i + 1), this);
                            _AvailableClients[i] = true;
                        }
                    }

                    Clients = _Clients;
                    AvailableClients = _AvailableClients;
                }
            }
        }

        public void ReleaseClient(uint Handle)
        {
            lock (this.Clients)
            {
                lock (this.AvailableClients)
                {
                    if (Clients.Length >= (Handle - 1)) // Ensure client exists
                    {
                        AvailableClients[Handle - 1] = true;
                    }
                }
            }
        }
    }

 public   class Database
    {
        public string DatabaseName;
        public uint PoolMinSize;
        public uint PoolMaxSize;

        public Database(string _DatabaseName, uint _PoolMinSize, uint _PoolMaxSize)
        {
            DatabaseName = _DatabaseName;

            PoolMinSize = _PoolMinSize;
            PoolMaxSize = _PoolMaxSize;
        }
    }

   public class DatabaseServer
    {
        public string Hostname;
        public uint Port;

        public string Username;
        public string Password;

        public DatabaseServer(string _Hostname, uint _Port, string _Username, string _Password)
        {
            Hostname = _Hostname;
            Port = _Port;
            Username = _Username;
            Password = _Password;
        }
    }

   public class DatabaseClient : IDisposable
    {
        public uint Handle;

        public DateTime LastActivity;

        public DatabaseManager Manager;

        public MySqlConnection Connection;
        public MySqlCommand Command;

        public Boolean IsAnonymous
        {
            get
            {
                return (Handle == 0);
            }
        }

        public int InactiveTime
        {
            get
            {
                return (int)(DateTime.Now - LastActivity).TotalSeconds;
            }
        }

        public ConnectionState State
        {
            get
            {
                return (Connection != null) ? Connection.State : ConnectionState.Broken;
            }
        }

        public DatabaseClient(uint _Handle, DatabaseManager _Manager)
        {
            if (_Manager == null)
            {
                throw new ArgumentNullException("[DBClient.Connect]: Invalid database handle");
            }

            Handle = _Handle;
            Manager = _Manager;

            Connection = new MySqlConnection(Manager.ConnectionString);
            Command = Connection.CreateCommand();

            UpdateLastActivity();
        }

        public void Connect()
        {
            if (Connection == null)
                throw new DatabaseException("[DBClient.Connect]: Connection instance of database client " + Handle + " holds no value.");
            if (Connection.State != ConnectionState.Closed)
                throw new DatabaseException("[DBClient.Connect]: Connection instance of database client " + Handle + " requires to be closed before it can open again.");


            try
            {
                Connection.Open();
            }
            catch (MySqlException e)
            {
                throw new DatabaseException("[DBClient.Connect]: Could not open MySQL Connection - " + e.Message);
            }
        }

        public void Disconnect()
        {
            try
            {
                Connection.Close();
            }
            catch { }
        }

        public void Dispose()
        {
            if (this.IsAnonymous)
            {
                Destroy();
                return;
            }

            Command.CommandText = null;
            Command.Parameters.Clear();

            Manager.ReleaseClient(Handle);
        }

        public void Destroy()
        {
            Disconnect();

            Connection.Dispose();
            Connection = null;

            Command.Dispose();
            Command = null;

            Manager = null;
        }

        public void UpdateLastActivity()
        {
            LastActivity = DateTime.Now;
        }

        public void AddParamWithValue(string sParam, object val)
        {
            Command.Parameters.AddWithValue(sParam, val);
        }

        public void ExecuteQuery(string sQuery)
        {
            try
            {
                Command.CommandText = sQuery;
                Command.ExecuteScalar();
                Command.CommandText = null;
            }
            catch (Exception e) { Console.WriteLine(e + "\n (" + sQuery + ")"); }
        }

        public bool findsResult(string sQuery)
        {
            bool Found = false;
            try
            {
                Command.CommandText = sQuery;
                MySqlDataReader dReader = Command.ExecuteReader();
                Found = dReader.HasRows;
                dReader.Close();

            }
            catch (Exception e) { Console.WriteLine(e + "\n (" + sQuery + ")"); }
            return Found;
        }

        public DataSet ReadDataSet(string Query)
        {
            try
            {
                DataSet DataSet = new DataSet();
                Command.CommandText = Query;

                using (MySqlDataAdapter Adapter = new MySqlDataAdapter(Command))
                {
                    Adapter.Fill(DataSet);
                }

                Command.CommandText = null;
                return DataSet;
            }
            catch (DatabaseException ex)
            {
                Log.WriteLine(LogLevel.Error, ex.ToString());
                return null;
            }
        }

        public DataTable ReadDataTable(string Query)
        {
            try
            {
                DataTable DataTable = new DataTable();
                Command.CommandText = Query;

                using (MySqlDataAdapter Adapter = new MySqlDataAdapter(Command))
                {
                    Adapter.Fill(DataTable);
                }

                Command.CommandText = null;
                return DataTable;
            }
            catch (DatabaseException ex)
            {
                Log.WriteLine(LogLevel.Error, ex.ToString());
                return null;
            }
        }

        public DataRow ReadDataRow(string Query)
        {
            try
            {
                DataTable DataTable = ReadDataTable(Query);

                if (DataTable != null && DataTable.Rows.Count > 0)
                {
                    return DataTable.Rows[0];
                }
            }
            catch (DatabaseException ex)
            {
                Log.WriteLine(LogLevel.Error,ex.ToString());
                return null;
            }
            return null;
        }

        public string ReadString(string Query)
        {
            try
            {
                Command.CommandText = Query;
                string result = Command.ExecuteScalar().ToString();
                Command.CommandText = null;
                return result;
            }
            catch (DatabaseException ex)
            {
                Log.WriteLine(LogLevel.Error, ex.ToString());
                return null;
            }
        }

        public Int32 ReadInt32(string Query)
        {
            Command.CommandText = Query;
            Int32 result = Int32.Parse(Command.ExecuteScalar().ToString());
            Command.CommandText = null;
            return result;
        }
        public byte[] GetBlob(MySqlCommand pCommand)
        {
            byte[] retvalue;
            try
            {

                retvalue = (byte[])pCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
              Console.WriteLine("Error reading BLOB: {0} && {1}", ex.Message, ex.StackTrace);
                return null;
            }
            return retvalue;
        }

    }

    public class DatabaseException : Exception
    {
        public DatabaseException(string sMessage) : base(sMessage) { }
    }
}
