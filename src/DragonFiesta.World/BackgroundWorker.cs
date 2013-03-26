using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DragonFiesta.World.Networking;

namespace DragonFiesta.World
{
    public class BackgroundWorker
    {
        private readonly Thread main;
        private int sleep = 1;
        private ulong TicksToSleep = 200;
        public ulong TicksPerSecond = 2000;

        public DateTime Current = DateTime.Now;

        public BackgroundWorker()
        {

            main = new Thread(Work);
            main.Start();
        }


        private void Work()
        {

            DateTime pingCheckRan = DateTime.Now;
            DateTime lastClientTime = DateTime.Now;
            DateTime lastCheck = DateTime.Now;
            for (ulong i = 0; ; i++)
            {
                DateTime now = DateTime.Now;


                if (now.Subtract(pingCheckRan).TotalSeconds >= 15)
                {

                    // Just check every minute
                    WorldClientManager.Instance.PingCheck();
                    pingCheckRan = now;

                }
                if (now.Subtract(lastClientTime).TotalSeconds >= 60)
                {
           
                    lastClientTime = now;
                }
                if (i % TicksToSleep == 0)
                {
                    this.Current = DateTime.Now;
                    Thread.Sleep(sleep);
                }
            }
            Console.WriteLine("bagroundworker leved");
        }
    }
}
