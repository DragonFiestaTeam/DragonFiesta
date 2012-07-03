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

        public static new  AcademyManager Instance { get; private set; }

        [InitializerMethod]
        public static new bool Initialize()
        {
            Instance = new AcademyManager();
            return true;
        }
       public AcademyManager()
       {

       }
        public void SendAcademyRequest(WorldClient pclient, AcademyRequestCode ErorCode,string Name)
        {
             using(var packet = new Packet(SH38Type.GuildAcademyRequest))
             {
                 packet.WriteString(Name, 16);
                 packet.WriteUShort((ushort)ErorCode);
                 pclient.SendPacket(packet);
             }
        }
    }
}
