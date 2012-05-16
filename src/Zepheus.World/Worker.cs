using System;
using System.Collections.Concurrent;
using System.Threading;
using Zepheus.Util;

namespace Zepheus.World
{
	[ServerModule(Util.InitializationStage.DataStore)]
	public sealed class Worker
	{
		public static Worker Instance { get; private set; }
		private readonly ConcurrentQueue<Action> callbacks = new ConcurrentQueue<Action>();
		private readonly Thread main;
		private int sleep = 1;
		public bool IsRunning { get; set; }

		public Worker()
		{
			main = new Thread(Work);
			IsRunning = true;
			main.Start();
		}

		[InitializerMethod]
		public static bool Load()
		{
			try
			{
				Instance = new Worker();
				Instance.sleep = Settings.Instance.WorkInterval;
				return true;
			}
			catch { return false; }
		}

		public void AddCallback(Action pCallback)
		{
			callbacks.Enqueue(pCallback);
		}

		private void ConnectEntity()
		{
		/*    Program.Entity = EntityFactory.GetWorldEntity(Settings.Instance.Entity);
			// Try to update...
			DatabaseUpdater du = new DatabaseUpdater(Settings.Instance.Entity, DatabaseUpdater.DatabaseTypes.World);
			du.Update();*/
		}


		private void Work()
		{
			try
			{
				//ConnectEntity();
			  //  Program.Entity.Characters.Count(); //test if database is online
			   // Log.WriteLine(LogLevel.Info, "Database Initialized at {0}", Settings.Instance.Entity.DataCatalog);
			}
			catch (Exception ex)
			{
				Log.WriteLine(LogLevel.Exception, "Error initializing database: {0}", ex.ToString());
				return;
			}

			Action action;
			DateTime pingCheckRan = DateTime.Now;
			DateTime lastClientTime = DateTime.Now;
			for (ulong i = 0; ; i++)
			{
				if (!this.IsRunning) break;
				DateTime now = DateTime.Now;

				while (callbacks.TryDequeue(out action))
				{
					try
					{
                       
                        UserWorkItem Work = new UserWorkItem(action);
                        Work.Queue();
						//action();
					}
					catch (Exception ex)
					{
						Log.WriteLine(LogLevel.Exception, ex.ToString());
					}
				}

				if (now.Subtract(pingCheckRan).TotalSeconds >= 15)
				{
			   
						// Just check every minute
						ClientManager.Instance.PingCheck(now);
						pingCheckRan = now;
					
				}
				if (now.Subtract(lastClientTime).TotalSeconds >= 60)
				{
					ClientManager.Instance.UpdateClientTime(now);
					lastClientTime = now;
				}
				if (i % 1500 == 0)
				{
					Thread.Sleep(sleep);
				}
			}
			Log.WriteLine(LogLevel.Info, "Server stopped handling callbacks.");
		}
	}
}
