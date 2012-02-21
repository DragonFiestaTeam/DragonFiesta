using System;
using System.Data;
using MySql.Data.MySqlClient;
using Zepheus.Util;

namespace Zepheus.Database
{
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

		public DatabaseClient(uint handle, DatabaseManager manager)
		{
			if (manager == null)
				throw new ArgumentNullException("[DBClient.Connect]: Invalid database handle");

			Handle = handle;
			Manager = manager;

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
			catch
			{
			}
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
				this.Dispose();
				Command.CommandText = sQuery;
				Command.ExecuteScalar();
				Command.CommandText = null;
			}
			catch (Exception e)
			{
				Console.WriteLine(e + "\n (" + sQuery + ")");
			}
		}

		public bool FindsResult(string sQuery)
		{
			bool found = false;
			try
			{
				Command.CommandText = sQuery;
				MySqlDataReader dReader = Command.ExecuteReader();
				found = dReader.HasRows;
				dReader.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine(e + "\n (" + sQuery + ")");
			}
			return found;
		}

		public DataSet ReadDataSet(string query)
		{
			try
			{
				DataSet dataSet = new DataSet();
				Command.CommandText = query;

				using (MySqlDataAdapter adapter = new MySqlDataAdapter(Command))
				{
					adapter.Fill(dataSet);
				}

				Command.CommandText = null;
				return dataSet;
			}
			catch (DatabaseException ex)
			{
				Log.WriteLine(LogLevel.Error, ex.ToString());
				return null;
			}
		}

		public DataTable ReadDataTable(string query)
		{
			try
			{
				DataTable dataTable = new DataTable();
				Command.CommandText = query;

				using (MySqlDataAdapter adapter = new MySqlDataAdapter(Command))
				{
					adapter.Fill(dataTable);
				}

				Command.CommandText = null;
				return dataTable;
			}
			catch (DatabaseException ex)
			{
				Log.WriteLine(LogLevel.Error, ex.ToString());
				return null;
			}
		}

		public DataRow ReadDataRow(string query)
		{
			try
			{
				DataTable dataTable = ReadDataTable(query);

				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					return dataTable.Rows[0];
				}
			}
			catch (DatabaseException ex)
			{
				Log.WriteLine(LogLevel.Error, ex.ToString());
				return null;
			}
			return null;
		}

		public string ReadString(string query)
		{
			try
			{
				Command.CommandText = query;
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

		public uint InsertAndIdentify(string query)
		{
			MySqlCommand command = this.Connection.CreateCommand();
			command.CommandText = query;
			return InsertAndIdentifyInternal(command);
		}

		private uint InsertAndIdentifyInternal(MySqlCommand pCommand)
		{
			pCommand.Prepare();
			pCommand.ExecuteNonQuery();
			pCommand.CommandText = "SELECT LAST_INSERT_ID()";
			pCommand.Parameters.Clear();
			return (uint)(long)pCommand.ExecuteScalar();
		}

		public uint ReadUInt(string query)
		{
			Command.CommandText = query;
			uint result = uint.Parse(Command.ExecuteScalar().ToString());
			Command.CommandText = null;
			return result;
		}

		public Int32 ReadInt32(string query)
		{
			Command.CommandText = query;
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
}