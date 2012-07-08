
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.World.Data;
using Zepheus.World.Networking;
using Zepheus.World.Managers;
using System;

namespace Zepheus.World.Handlers
{
    public sealed class Handler29
    {
              [PacketHandler(CH29Type.ChangeMemberRank)]
        public static void ChangeRank(WorldClient client, Packet packet)
        {
                  string targetName;
                  byte Rank;

            if (client.Character.Guild == null)
                return;
            if (client.Character.Guild.GuildMaster != client.Character.Character.Name)
                return;
            if (!packet.TryReadString(out targetName, 16))
                return;
            if (!packet.TryReadByte(out Rank))
                return;
            GuildMember pMember = client.Character.Guild.GuildMembers.Find(m => m.pMemberName == targetName);
            pMember.GuildRank = (GuildRanks)Rank;
        }
        [PacketHandler(CH29Type.GuildRquestAnswer)]
        public static void GuildRquestAnswer(WorldClient client, Packet packet)
        {
            bool Answer;
            string GuildName;
            if(!packet.TryReadString(out GuildName,16))
                return;

            if (!packet.TryReadBool(out Answer))
                return;
            GuildRequest pRequest = GuildManager.Instance.pRequests.Find(G => G.Guild.Name == GuildName);
            if (Answer)
            {
                GuildManager.Instance.AddMember(pRequest);
                GuildManager.Instance.RemoveGuildRequest(pRequest);
            }
            else
            {
                GuildManager.Instance.pRequests.Remove(pRequest);
                GuildManager.Instance.RemoveGuildRequest(pRequest);
            }

        }
        [PacketHandler(CH29Type.ChangeGuildDetails)]
        public static void ChangeGuildDetails(WorldClient client, Packet packet)
        {
            string Message;
            ushort MessageLenght;
            if (!packet.TryReadUShort(out MessageLenght))
                return;
            if (!packet.TryReadString(out Message, MessageLenght))
                return;
            if (client.Character.Guild == null)
                return;

            client.Character.Guild.Details.UpdateGuildDetails(client.Character.Guild, client.Character.Character, Message, MessageLenght);
        }
          [PacketHandler(CH29Type.GuildInvideRequest)]
        public static void GuildInvideRequest(WorldClient client, Packet packet)
        {
            string targetName;
              if(!packet.TryReadString(out targetName,16))
                  return;
              if (client.Character.Guild == null)
                  return;

              GuildManager.Instance.CreateGuildInvideRequest(targetName,client.Character);
        }
        [PacketHandler(CH29Type.GuildNameRequest)]
        public static void GuildNameRequest(WorldClient client, Packet packet)
        {
            int id;
            if (!packet.TryReadInt(out id)) {
                Log.WriteLine(LogLevel.Warn, "Failed reading Guild Name Request packet {0}", client.Character.Character.Name);
                return;
            }
            Guild g = GuildManager.Instance.GetGuildByID(id);
            if (g != null)
            {
                SendGuildNameResult(client, id, g.Name);
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

            for (int i = 0; i < client.Character.Guild.GuildMembers.Count; i += 20)
            {
                Packet pack = Guild.MultiMemberList(client.Character.Guild.GuildMembers, i, i + Math.Min(20, client.Character.Guild.GuildMembers.Count - i), client.Character.Guild.GuildMembers.Count);
                client.SendPacket(pack);
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
