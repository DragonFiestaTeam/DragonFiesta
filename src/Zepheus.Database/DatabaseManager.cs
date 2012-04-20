using System;
using System.Data;
using System.Threading;
using MySql.Data.MySqlClient;
using Zepheus.Util;
using Zepheus.Database.Storage;

namespace Zepheus.Database
{
	public class DatabaseManager
	{
		public DatabaseServer Server;
		public Database Database;

		public DatabaseClient[] Clients;
		public Boolean[] AvailableClients;
		public int ClientStarvation;
		public PriorityQueue<MySqlCommand> Commands;
		int CommandCacheCount; 
		public Thread ClientMonitor;

		public string ConnectionString
		{
			get
			{
				var connectionString = new MySqlConnectionStringBuilder
				{
					Server = Server.Hostname,
					Port = Server.Port,
					UserID = Server.Username,
					Password = Server.Password,
					Database = Database.DatabaseName,
					MinimumPoolSize = Database.PoolMinSize,
					MaximumPoolSize = Database.PoolMaxSize
				};

				return connectionString.ToString();
			}
		}

		public DatabaseManager(DatabaseServer server, Database database)
		{
			Server = server;
			Database = database;
			Commands  = new PriorityQueue<MySqlCommand>();
			Clients = new DatabaseClient[0];
			AvailableClients = new Boolean[0];
			ClientStarvation = 0;
			CommandCacheCount = 0;
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
					catch (NullReferenceException)
					{
					}
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
			catch (ThreadAbortException)
			{
			}

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
						foreach (var i in Clients)
						{
							if (i.State != ConnectionState.Closed)
							{
								if (i.InactiveTime >= 45) // Not used in the last %x% seconds
								{
									i.Disconnect();
								}
							}
						}
					}

					Thread.Sleep(10000); // 10 Seconds
				}
				catch (ThreadAbortException)
				{
				}
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

				DatabaseClient anonymous = new DatabaseClient(0, this);
				anonymous.Connect();

				return anonymous;
			}
		}
		public void PushCommand(MySqlCommand command)
		{
			CommandCacheCount++;
			Commands.Enqueue(command,CommandCacheCount);
		}
		public void SetClientAmount(uint amount)
		{
			lock (this.Clients)
			{
				lock (this.AvailableClients)
				{
					if (Clients.Length == amount)
					{
						return;
					}

					if (amount < Clients.Length)
					{
						for (uint i = amount; i < Clients.Length; i++)
						{
							Clients[i].Destroy();
							Clients[i] = null;
						}
					}

					DatabaseClient[] clients = new DatabaseClient[amount];
					bool[] availableClients = new bool[amount];

					for (uint i = 0; i < amount; i++)
					{
						if (i < Clients.Length)
						{
							clients[i] = Clients[i];
							availableClients[i] = AvailableClients[i];
						}
						else
						{
							clients[i] = new DatabaseClient((i + 1), this);
							availableClients[i] = true;
						}
					}

					Clients = clients;
					AvailableClients = availableClients;
				}
			}
		}

		public void ReleaseClient(uint handle)
		{
			lock (this.Clients)
			{
				lock (this.AvailableClients)
				{
					if (Clients.Length >= (handle - 1)) // Ensure client exists
					{
						AvailableClients[handle - 1] = true;
					}
				}
			}
		}
	}
}