using System;
using Zepheus.World.Networking;
using Zepheus.World.Data;

namespace Zepheus.World.Data
{
   public class GuildRequest
    {
       public WorldClient pRequester { get; private set; }
       public WorldClient pTarget { get; private set; }
       public DateTime CreationTime { get; private set; }
       public GuildRequest(WorldClient pRequester, WorldClient pTarget)
       {
           this.pRequester = pRequester;
           this.pTarget = pTarget;
           this.CreationTime = DateTime.Now;
           SendRequest();
       }
       private void SendRequest()
       {
       //Todo Send Request packet
       }
    }
}
