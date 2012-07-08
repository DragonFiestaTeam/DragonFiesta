using System;
using Zepheus.World.Networking;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;

namespace Zepheus.World.Data
{
   public class GuildRequesterResponse
    {
       public WorldClient pClient { get; private set; }
       public WorldClient TargetClient { get; private set; }

       public GuildRequesterResponse(WorldClient pClient,WorldClient pTarget)
       {
           this.pClient = pClient;
           this.TargetClient = pTarget;
       }
       public bool GetReponse()
       {
         if(this.TargetClient.Character.Guild != null)
         {
             SendRequesterResponse(GuildRequstCode.MemberHasAlredyAcademy);
             return false;
         }
         return true;
       }
       private void SendRequesterResponse(GuildRequstCode pCode)
       {
           using (var packet = new Packet(SH29Type.SendRequesterResponse))
           {
               packet.WriteString(this.TargetClient.Character.Character.Name, 16);
               packet.WriteUShort((ushort)pCode);
               pClient.SendPacket(packet);
           }
       }
    }
}
