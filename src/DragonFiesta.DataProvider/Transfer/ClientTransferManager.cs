using System;
using System.Timers;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using DragonFiesta.Util;

namespace DragonFiesta.Data.Transfer
{
    public class ClientTransferManager
    {
        private ConcurrentDictionary<string, ClientTransfer> Transfers = new ConcurrentDictionary<string, ClientTransfer>();

        public static ClientTransferManager Instance { get; private set; }
        
        private Mutex TransferMutex = new Mutex();

        private readonly System.Timers.Timer expirator;
        private int transferTimeout = 10;

        
		public ClientTransferManager()
		{
			expirator = new System.Timers.Timer(10000);
			expirator.Elapsed += new ElapsedEventHandler(ExpiratorElapsed);
			expirator.Start();
		}
        public static bool Initialize()
        {
            try
            {
                Instance = new ClientTransferManager();

                Logs.Main.Info("Successfully initialized ClientTransferManager");

            }
            catch (Exception e)
            {
                Logs.Main.Fatal("Could not initialize ClientTransferManager", e);
                return false;
            }
            return true;
        }

        public void AddTransfer(ClientTransfer transfer)
        {
            TransferMutex.WaitOne();
            try
            {
       
                switch (transfer.Type)
                {
                    case TransferType.LoginToWorld:
                        AddTransfer(transfer.AuthHash, transfer);
                        break;
                    case TransferType.WorldToZone:
                        AddTransfer(transfer.UserName,transfer);
                        break;
                    case TransferType.MapToMap:
                        AddTransfer(transfer.UserName, transfer);
                        break;
                    default: 
                        Logs.Main.Warn(String.Format("Unkown TransferType {0}", transfer.Type));
                        break;
                }
            }
            finally
            {
                TransferMutex.ReleaseMutex();
            }
        }
        private void AddTransfer(string key, ClientTransfer pTransfer)
        {
            if (this.Transfers.ContainsKey(key))
            {
                ClientTransfer trans;
                if (this.Transfers.TryRemove(key,out trans))
                {
                    Logs.Main.Warn(String.Format("Duplicate client transfer (Char={0}) attempt from {1}.", key, pTransfer.IP));
                }
            }
            if (!this.Transfers.TryAdd(key, pTransfer))
            {
                 Logs.Main.Warn(String.Format("Error registering client transfer for {0}.", key));
            }
            else
            {
                  Logs.Main.Debug(String.Format("Transfering {0}.", key));
            }
        }
        public bool RemoveTransfer(string key)
        {
            TransferMutex.WaitOne();
            try
            {
                ClientTransfer trans;
                return  Transfers.TryRemove(key,out trans);
            }
            finally
            {
                TransferMutex.ReleaseMutex();
            }
        }

        public bool GetTransfer(string key, out ClientTransfer Transfer)
        {
            return this.Transfers.TryGetValue(key, out Transfer);
        }
        private readonly List<string> toExpire = new List<string>();
        void ExpiratorElapsed(object sender, ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;
            //this is actually executed in the main thread! (ctor is in STAThread)
            foreach (var transfer in this.Transfers)
            {
                if (now.Subtract(transfer.Value.TransferStartTime).TotalSeconds >= transferTimeout)
                {
                    toExpire.Add(transfer.Key);
                    Logs.Main.Debug(String.Format("Transfer timeout for {0}", transfer.Key));
                }
            }

            if (toExpire.Count > 0)
            {
                ClientTransfer trans;
                foreach (var expired in toExpire)
                {
                    Transfers.TryRemove(expired, out trans);
                }
                toExpire.Clear();
            }
        }

    }
}
