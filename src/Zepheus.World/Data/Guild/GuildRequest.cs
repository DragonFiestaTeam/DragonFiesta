using System;
using Zepheus.World.Networking;
using Zepheus.World.Data;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
namespace Zepheus.World.Data
{
   public class GuildRequest
    {
       public WorldClient pRequester { get;  set; }
       public WorldClient pTarget { get;  set; }
       public Guild Guild { get; set; }
       public DateTime CreationTime { get;  set; }

       public GuildRequest()
       {
       }
       public GuildRequest(WorldClient pRequester, WorldClient pTarget,Guild pGuild)
       {
           this.pRequester = pRequester;
           this.pTarget = pTarget;
           this.Guild = pGuild;
           this.CreationTime = DateTime.Now;
           SendRequest();
           RequestSendedOK();

       }
         private void RequestSendedOK()
       {
             //Todo Send Packet to requester
       }
      public virtual void SendRequest()
       {
           using (var packet = new Packet(SH29Type.SendGuildInvideRequest))
           {
               packet.WriteString(Guild.Name, 16);
               packet.WriteString(pRequester.Character.Character.Name, 16);
               pTarget.SendPacket(packet);
           }
       }
    }
}
