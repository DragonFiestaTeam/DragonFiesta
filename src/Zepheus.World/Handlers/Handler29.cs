
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.World.Data;
using Zepheus.World.Networking;

namespace Zepheus.World.Handlers
{
    public sealed class Handler29
    {
        [PacketHandler(CH29Type.GuildNameRequest)]
        public static void GuildNameRequest(WorldClient client, Packet packet)
        {
            int id;
            if (!packet.TryReadInt(out id)) {
                Log.WriteLine(LogLevel.Warn, "Failed reading Guild Name Request packet {0}", client.Character.Character.Name);
                return;
            }
            var guild = WorldGuild.GetGuild(id);
            if (guild != null)
            {
                SendGuildNameResult(client, id, guild.Name);
            }
        }

        public static void SendGuildNameResult(WorldClient client, int pID, string pName)
        {
            using (var packet = new Packet(SH29Type.GuildNameResult))
            {
                packet.WriteInt(pID);
                packet.WriteString(pName, 16);
                client.SendPacket(packet);
            }
        }
        [PacketHandler(CH29Type.CreateGuild)]
        public static void CreateGuildt(WorldClient client, Packet packet)
        {
            int unk = 0;
            bool GuildWar;
            string GuildName = string.Empty;
            string GuildPassword = string.Empty;
            if (!packet.TryReadString(out GuildName, 16))
                return;
            if (!packet.TryReadString(out GuildPassword, 8))
                return;
            if (!packet.TryReadInt(out unk))
                return;
            if (!packet.TryReadBool(out GuildWar))
                return;
            {
                using (var ppacket = new Packet(29, 6))
                {
                    if(client.Character.Character.CharLevel < 20)
                    {
                        ppacket.WriteUShort(3266);
                    }
                    else if(client.Character.Character.Money <= 1000000)
                    {
                        packet.WriteUShort(3228);
                    }
                  
                    ppacket.WriteUInt(0);//unk
                    ppacket.WriteString(GuildName, 16);
                    ppacket.WriteString(GuildPassword, 8);
                    ppacket.WriteBool(GuildWar);
                    client.SendPacket(ppacket);
                }
            }
        }
    }
}
