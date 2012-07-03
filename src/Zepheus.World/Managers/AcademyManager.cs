using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.FiestaLib;
using Zepheus.World.Data;
using Zepheus.World.Networking;

namespace Zepheus.World.Managers
{
     [ServerModule(InitializationStage.Clients)] 
   public class AcademyManager : GuildManager
    {
       public static AcademyManager Instance { get; private set; }
       public AcademyManager()
        {
      
        }
        [InitializerMethod]
        public static bool Initialize()
        {
            Instance = new AcademyManager();
            return true;
        }
         public void SendAcademyRequest(WorldClient pclient, AcademyRequestCode ErorCode,string Name)
        {
             using(var packet = new Packet(SH38Type.GuildAcademyRequest))
             {
                 packet.WriteString(Name, 16);
                 packet.WriteUShort((ushort)ErorCode);
             }
        }
         public Academy GetAcademyByName(string AcademyName)
        {
            Guild gg;
            if (!DataProvider.Instance.GuildsByName.TryGetValue(AcademyName,out gg))
                return null;

            return gg.GuildAcademy;
        }
    }
}
