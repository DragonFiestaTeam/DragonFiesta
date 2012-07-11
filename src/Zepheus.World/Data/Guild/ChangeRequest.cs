using System;
using Zepheus.World.Networking;
using Zepheus.World.Data;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;

namespace Zepheus.World.Data
{
    public class ChangeRequest : GuildRequest
    {
        public ChangeRequest(WorldClient pRequester, WorldClient pTarget, Guild pGuild)
        {
            this.pRequester = pRequester;
            this.pTarget = pTarget;
            this.Guild = pGuild;
            this.CreationTime = DateTime.Now;
            SendRequest();
          
        }
        public override void SendRequest()
        {
            using (var packet = new Packet(SH38Type.ChangeRequest))
            {
                packet.WriteString(Guild.Name, 16);
                packet.WriteString(pRequester.Character.Character.Name, 16);
                pTarget.SendPacket(packet);
            }
        }
    }
}
