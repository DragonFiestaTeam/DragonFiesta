using System;
using System.Collections.Generic;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.World.Networking;
using Zepheus.World.Data;
using Zepheus.World.Managers;
using Zepheus.World.Data.Guilds.Academy;
namespace Zepheus.World.Handlers
{
    public sealed class Handler38
    {
        public static void SendAcademyResponse(WorldClient pClient,string GuildName, GuildAcademyResponse Response)
        {

            using (var packet = new Packet(SH38Type.AcademyResponse))
            {
                packet.WriteString(GuildName, 16);
                packet.WriteUShort((ushort)Response);
                pClient.SendPacket(packet);
            }
        }
    }
        
}
