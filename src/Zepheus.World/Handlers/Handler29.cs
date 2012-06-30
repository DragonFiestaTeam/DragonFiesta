
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.World.Data;
using Zepheus.World.Networking;
using Zepheus.World.Managers;

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
            if (client.Character.Guild != null)
            {
                SendGuildNameResult(client, id, client.Character.Guild.Name);
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
        [PacketHandler(CH29Type.GuildListReqest)]
        public static void GuildListReqest(WorldClient client, Packet packet)
        {
            if (client.Character.Guild == null)
                return;

            using (var Ppacket = new Packet(SH29Type.GuildList))
            {
                
                Ppacket.WriteInt((ushort)client.Character.Guild.GuildMembers.Count);
                Ppacket.WriteUShort((ushort)client.Character.Guild.GuildMembers.Count);
          foreach(var GuildMember in client.Character.Guild.GuildMembers)
          {
                Ppacket.WriteString(GuildMember.pMemberName, 16);
                Ppacket.WriteByte((byte)GuildMember.GuildRank);//rank
                Ppacket.WriteInt(0);

                Ppacket.WriteUShort(GuildMember.Korp);//korp
                Ppacket.WriteByte(0);//unk
                Ppacket.WriteUShort(0xffff);//unk
                Ppacket.WriteUShort(0xffff);//unk
                Ppacket.WriteByte(0);//unk
                Ppacket.WriteInt(32);
                Ppacket.WriteInt(32);
                Ppacket.Fill(50, 0x00);//unk
                Ppacket.WriteByte(GuildMember.isOnline ? (byte)0xB9 : (byte)0x00);//onlinestatus
                Ppacket.Fill(3, 0x00);//unk
                Ppacket.WriteByte(GuildMember.pMemberJob);//job
                Ppacket.WriteByte(GuildMember.Level);
                Ppacket.WriteByte(0);//unk
                Ppacket.WriteString(GuildMember.MapName,12);//charmapname
                }
                client.SendPacket(Ppacket);
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
                        ppacket.WriteUInt(0);//unk
                    }
                    else if (client.Character.Character.Money < 1000000)
                    {
                        ppacket.WriteUShort(3228);
                        ppacket.WriteUInt(0);//unk
                    }
                    else
                    {
                        ppacket.WriteUShort(3137);
                        ppacket.WriteUInt(32);//unk
                        GuildManager.Instance.CreateGuild(client.Character, GuildName, GuildPassword, GuildWar);
                    }
                    ppacket.WriteString(GuildName, 16);
                    ppacket.WriteString(GuildPassword, 8);
                    ppacket.WriteBool(GuildWar);
                    client.SendPacket(ppacket);
                }
            }
        }
    }
}
