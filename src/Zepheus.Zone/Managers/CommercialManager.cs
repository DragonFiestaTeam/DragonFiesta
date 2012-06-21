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
   
       #endregion 
       #region Methods

       private void SendCommecialRequest(CommercialReqest pRequest)
       {
           using (var pPacket = new Packet(SH19Type.SendCommercialReqest))
           {
               pPacket.WriteUShort(pRequest.pFromCommercialClient.MapObjectID);
               pRequest.pToCommercialClient.Client.SendPacket(pPacket);
           }
       }
       public void AddComercialRequest(ZoneClient pClient,ushort  MapObjectIDto)
       {
           Log.WriteLine(LogLevel.Debug, "{0} AddComercialReqest {1}", pClient.Character.Character.Name, MapObjectIDto);
           CommercialReqest pRequest = new CommercialReqest(pClient.Character, MapObjectIDto);
           this.CommercialReqests.Add(pRequest);
           SendCommecialRequest(pRequest);
       }
       public void RemoveReqest(ZoneClient pClient)
       {
           CommercialReqest Request = CommercialReqests.Find(r => r.MapID == pClient.Character.MapID && r.pToCommercialClient.MapObjectID== pClient.Character.MapObjectID);
           if (CommercialReqests.Contains(Request))
           {
               CommercialReqests.Remove(Request);
           }
       }
       private void AddComercial(Commercial Commecial)
       {
         //Todo
       }
       #endregion
   }
}
