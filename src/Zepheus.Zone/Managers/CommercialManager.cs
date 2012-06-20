using System;
using System.Collections.Generic;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.InterLib.Networking;
using Zepheus.Zone.Game;
using Zepheus.Zone.Data;
using Zepheus.Zone.Networking;
using Zepheus.Util;
using System.Threading;

namespace Zepheus.Zone.Managers
{
   [ServerModule(InitializationStage.Clients)]
   public class CommercialManager
   {
       #region .ctor
       public CommercialManager()
       {
           CommercialReqests = new List<CommercialReqest>();
           CommercialReqestThread = new Thread(new ThreadStart(RequestThread));
           CommercialReqestThread.Start();
       }
        [InitializerMethod]
        public static bool Initialize()
        {
            Instance = new CommercialManager();
            
            return true;
        }
       #endregion
       #region Properties
       public static CommercialManager Instance { get; private set; }

       private readonly List<CommercialReqest> CommercialReqests;
       private readonly Thread CommercialReqestThread;
       #endregion 
       #region Methods
       private void RequestThread()//Todo Remove all Requests when not answer
       {
           List<CommercialReqest> RemovelRequestList = new List<CommercialReqest>();
           while (true)
           {
               foreach(CommercialReqest pR in CommercialReqests)
               {
                   if(DateTime.Now.Subtract(pR.CrationTimeStamp).TotalSeconds >= 60)
                    {
                        RemovelRequestList.Add(pR);
                       
                       //Todo Send not Accept Packet
                    }
               }
               foreach(var RemoveRequest in RemovelRequestList)
               {
                   CommercialReqests.Remove(RemoveRequest);
               }
               Log.WriteLine(LogLevel.Debug, "Remove All CommecialRequest  There Answer was Unkown");
               Thread.Sleep(600000);//10 minutes
           }
       }
       private void AddReqest(CommercialReqest Reqest)
       {
       
           if (Reqest != null)
           {
               if(!CommercialReqests.Contains(Reqest))
               {
                   CommercialReqests.Add(Reqest);
               }
           }
       }
       private void AddComercial(ZoneClient pClient,string pToClient)
       {
           Log.WriteLine(LogLevel.Debug, "{0} AddComercialReqest {1}", pClient.Character.Character.Name, pToClient);
           if (!ClientManager.Instance.GetClientByCharName(pToClient).Authenticated)
               return; // not online
       }
       private void RemoveReqest(CommercialReqest pReqest)
       {
           CommercialReqests.Remove(pReqest);
       }
       private void AddComercial(Commercial Commecial)
       {
         //Todo
       }
       #endregion
   }
}
