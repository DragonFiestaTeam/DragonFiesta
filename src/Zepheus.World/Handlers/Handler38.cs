using System;
using System.Collections.Generic;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.World.Networking;
using Zepheus.World.Data;
using Zepheus.World.Managers;

namespace Zepheus.World.Handlers
{
    public sealed class Handler38
    {
        [PacketHandler(CH38Type.ChangeRequestAnswer)]
        public static void ChangeFromGuildAcademyToResponse(WorldClient client, Packet packet)
        {
            string GuildName;
            string RequestName;
           bool answer;
            if (!packet.TryReadString(out GuildName, 16))
                return;

            if(!packet.TryReadString(out RequestName,16))
            return;

            if(!packet.TryReadBool(out answer))
                return;
            if (client.Character.Academy == null)
                return;

            if (answer)
            {
                AcademyMember AcaMember = client.Character.Academy.AcademyMembers.Find(m => m.pMemberName == client.Character.Character.Name);
                if (AcaMember == null)
                    return;
                Guild gg;
                if (!DataProvider.Instance.GuildsByName.TryGetValue(GuildName, out gg))
                    return;
                client.Character.Guild = gg;
                GuildMember gMember = new GuildMember
                {
                    CharID = AcaMember.CharID,
                    GuildID = AcaMember.GuildID,
                    GuildRank = GuildRanks.Member,
                    pMemberName = AcaMember.pMemberName,
                    isOnline = true,
                    Korp = 0,
                    Level = AcaMember.Level,
                    MapName = AcaMember.MapName,
                    pClient = client,
                    pMemberJob = AcaMember.pMemberJob
                };
                using (var pack = new Packet(SH38Type.JoinGuildFromAcademy))
                {
                    //this packet remove character from academy List and added to GuildList
                    pack.WriteString(RequestName, 16);
                    pack.WriteString(client.Character.Character.Name, 16);
                    pack.WriteByte(6);//rank
                    pack.WriteInt(0);//unk
                    pack.WriteUShort(gMember.Korp);//korp
                    pack.Fill(64, 0x00);//unk
                    pack.WriteByte(gMember.isOnline ? (byte)0x95 : (byte)0x00);// (this.isOnline ? (byte)0x95 : (byte)0x00);
                    pack.Fill(3, 0x00);//unk
                    pack.WriteByte(gMember.pMemberJob);
                    pack.WriteByte(gMember.Level);
                    pack.Fill(13, 0x00);//unk
                    client.Character.Academy.Guild.SendPacketToAllOnlineMember(pack);
                    client.Character.Academy.SendPacketToAllOnlineMember(pack);
                }
                for (int i = 0; i < client.Character.Guild.GuildMembers.Count; i += 20)
                {
                    Packet pack = Guild.MultiMemberList(client.Character.Guild.GuildMembers, i, i + Math.Min(20, client.Character.Guild.GuildMembers.Count - i), client.Character.Guild.GuildMembers.Count);
                    client.SendPacket(pack);
                }
                AcaMember.RemoveFromDatabase();
                client.Character.Academy.AcademyMembers.Remove(AcaMember);
                gMember.AddToDatabase();
                client.Character.Academy.Guild.GuildMembers.Add(gMember);
                using (var p2 = new Packet(SH4Type.CharacterGuildinfo))
                {
                    client.Character.Guild.Details.WriteMessageAsGuildMember(p2, client.Character.Guild);
                }
                for (int i = 0; i < client.Character.Academy.AcademyMembers.Count; i += 20)
                {
                    Packet pack = Academy.MultiMemberList(client.Character.Academy.AcademyMembers, i, i + Math.Min(20, client.Character.Academy.AcademyMembers.Count - i), client.Character.Academy.AcademyMembers.Count);
                    client.SendPacket(pack);
                }
                client.Character.Academy = null;
                using (var pack = new Packet(SH38Type.GuildAcademyMemberLoggetOn))
                {
                    pack.WriteString(client.Character.Character.Name, 16);
                    client.Character.Academy.Guild.SendPacketToAllOnlineMember(pack);
                    client.Character.Academy.SendPacketToAllOnlineMember(pack);

                }
            }
            using(var pack = new Packet(SH29Type.ChangeResponse))
            {
                pack.WriteUShort(3137);//unk
                pack.WriteByte(3);
                pack.Fill(2, 0x00);//unk
                client.SendPacket(pack);
            }
            
        }
        [PacketHandler(CH38Type.ChangeFromGuildAcademyToGuild)]
        public static void ChangeFromGuildAcademyToGuild(WorldClient client, Packet packet)
        {
            if (client.Character.Guild == null)
                return;
            string ChangeName;
            if (!packet.TryReadString(out ChangeName, 16))
                return;
            WorldClient pClient = ClientManager.Instance.GetClientByCharname(ChangeName);
            if(pClient == null)
             return;
            ChangeRequest pRequest = new ChangeRequest(client, pClient, client.Character.Guild);
            GuildManager.Instance.AddReuest(pRequest);
            using (var pack = new Packet(SH38Type.ChangeResponsePacket))
            {
                pack.WriteString(ChangeName, 16);
                pack.WriteUShort(6016);//ok
                client.SendPacket(pack);
            }

        }
        [PacketHandler(CH38Type.KickMember)]
        public static void KickMember(WorldClient client, Packet packet)
        {
            if (client.Character.Academy == null)
                return;
            //todo check rank

            string KickName;
            if (!packet.TryReadString(out KickName, 16))
                return;
            AcademyMember pMember = client.Character.Academy.AcademyMembers.Find(m => m.pMemberName == KickName);
            if (pMember == null)
                return;
            pMember.RemoveFromDatabase();
            if (pMember.pClient != null)
            {
                pMember.pClient.Character.Academy = null;
            }
            using (var pack = new Packet(SH38Type.KickResponse))
            {
                pack.WriteString(KickName, 16);
                pack.WriteUShort(6016);//ok
                client.SendPacket(pack);
            }
            using (var pack = new Packet(SH38Type.KickGuildAcademyMember))
            {
                pack.WriteString(KickName, 16);
                client.Character.Guild.SendPacketToAllOnlineMember(pack);
                client.Character.Guild.GuildAcademy.SendPacketToAllOnlineMember(pack);
            }
        }
        [PacketHandler(CH38Type.BlockAcademyChat)]
        public static void GuildAcademyChatBlock(WorldClient client, Packet packet)
        {
            if (client.Character.Guild == null)
                return;
            string Blockname;
            if (!packet.TryReadString(out Blockname, 16))
                return;
            AcademyMember pMember = client.Character.Guild.GuildAcademy.AcademyMembers.Find(m => m.pMemberName == Blockname);
            if (pMember == null)
                return;
            pMember.HasAcademyChatBlock = true;
            pMember.ChatBlockToDatabase();
            using (var pack = new Packet(SH38Type.BlockMessage))
            {
                pack.WriteString(client.Character.Character.Name,16);
                pack.WriteString(Blockname, 16);
                client.Character.Guild.SendPacketToAllOnlineMember(pack);
                client.Character.Guild.GuildAcademy.SendPacketToAllOnlineMember(pack);
            }
        }
        [PacketHandler(CH38Type.GuildAcademyChatMessage)]
        public static void GuildAcademyChat(WorldClient client, Packet packet)
        {
            if (client.Character.Academy == null)
                return;
            string message;
            byte lenght;
            if(!packet.TryReadByte(out lenght))
                return;
            if(!packet.TryReadString(out message,lenght))
                return;
          AcademyMember pMember = client.Character.Academy.AcademyMembers.Find(m => m.pMemberName == client.Character.Character.Name);
            if (pMember != null)
            {
                if (pMember.HasAcademyChatBlock)
                    using (var pack2 = new Packet(SH38Type.ChatBlock))
                    {
                        pack2.WriteUShort(6140);
                        client.SendPacket(pack2);
                    }//block chat
                    return;
            }

            //Todo Log Message

            foreach(var pAcademyMember in client.Character.Academy.AcademyMembers)
            {
                if(pAcademyMember.isOnline && !pAcademyMember.HasAcademyChatBlock)
                client.Character.Academy.SendChatMessage(pAcademyMember.pClient,client.Character.Character.Name, message);
            }

            foreach(var pGuildMember in client.Character.Academy.Guild.GuildMembers)
            {
                if(pGuildMember.isOnline)
                client.Character.Academy.SendChatMessage(pGuildMember.pClient, client.Character.Character.Name, message);
            }
            
        }
        [PacketHandler(CH38Type.GuildAcademyReuqest)]
        public static void GuildAcademyRequest(WorldClient client, Packet packet)
        {
            string AcademyName;
            if (!packet.TryReadString(out AcademyName, 16))
                return;
            if (client.Character.Character.CharLevel > 60)
                return;

            Guild pGuild = GuildManager.Instance.GetGuildByName(AcademyName);

            if (pGuild == null)
            {
                AcademyManager.Instance.SendAcademyRequest(client, AcademyRequestCode.NotFound, AcademyName);
                return;
            }
            else if (pGuild.GuildAcademy.AcademyMembers.Count >= 50)
            {
                AcademyManager.Instance.SendAcademyRequest(client, AcademyRequestCode.AcademyFull, AcademyName);
            }
            else if (pGuild.GuildAcademy.GetMemberByName(client.Character.Character.Name) != null)
            {
                AcademyManager.Instance.SendAcademyRequest(client, AcademyRequestCode.AlreadyExists, AcademyName);
            }
            AcademyMember pMember = new AcademyMember
            {
                CharID = client.Character.ID,
                isOnline = true,
                Level = client.Character.Character.CharLevel,
                Rank = 0,//rank unkown for member
                pClient = client,
                MapName = DataProvider.Instance.GetMapShortNameFromMapid(client.Character.Character.PositionInfo.Map),
                pMemberName = client.Character.Character.Name,
                pMemberJob = client.Character.Character.Job,
                GuildID = pGuild.ID,
                Academy = pGuild.GuildAcademy,
                RegisterDate = DateTime.Now,
                HasAcademyChatBlock = false,
            };
            pMember.AddToDatabase();
            client.Character.Academy = pGuild.GuildAcademy;
            client.Character.Character.AcademyID = pGuild.ID;
            pGuild.GuildAcademy.AcademyMembers.Add(pMember);
            AcademyManager.Instance.SendAcademyRequest(client,AcademyRequestCode.Sucess, AcademyName);
            InterServer.InterHandler.AddGuildMemberToZone(false, pMember.GuildID, client.Character.Character.Name, client.Character.Character.ID);
            using (var pack = new Packet(SH4Type.CharacterGuildacademyinfo))
            {
                pGuild.GuildAcademy.Details.WriteMessageAsGuildAcadmyler(pack, pGuild.GuildAcademy);
                client.SendPacket(pack);
            }
            client.Character.BroudCastGuildNameResult();
            using (var pack = new Packet(SH38Type.GuildAcademyJoin))
            {
              pMember.WriteInfo(pack);
              pGuild.SendPacketToAllOnlineMember(pack);
              pGuild.GuildAcademy.SendPacketToAllOnlineMember(pack);
            }
          
        }
        [PacketHandler(CH38Type.GuildAcademyLeave)]
        public static void GuildAcademyLeave(WorldClient client, Packet packet)
        {
            if (client.Character.Academy == null)
                return;
            Guild gg;
             if(!DataProvider.Instance.GuildsByID.TryGetValue(client.Character.Academy.ID,out gg))
                 return;
            //Todo Response
            AcademyMember pMember = client.Character.Academy.AcademyMembers.Find(m => m.pMemberName == client.Character.Character.Name);
           /* if (pMember.RegisterDate.Subtract(DateTime.Now).TotalMinutes <= 60)
            {
                Academy.SendAcademyLeaveRequest(AcademyRequestCode.OneHoursLeave, client);
                return;
            }*/
            if (pMember.pClient != null)
            {
                pMember.pClient.Character.Academy = null;
            }
            InterServer.InterHandler.RemoveGuildMemberFromZone(false, pMember.GuildID, pMember.CharID);
            Academy.SendAcademyLeaveRequest(AcademyRequestCode.LeaveSucess, client);
             pMember.RemoveFromDatabase();
             gg.GuildAcademy.AcademyMembers.Remove(pMember);
             using (var pack = new Packet(SH38Type.GuildAcademyMemberLeave))
             {
                 packet.WriteString(client.Character.Character.Name, 16);
                 client.Character.Academy.SendPacketToAllOnlineMember(packet);
                 gg.SendPacketToAllOnlineMember(pack);

             }

        }
           [PacketHandler(CH38Type.ChangeDetails)]
        public static void GuildAcademyDetailsChange(WorldClient client, Packet packet)
        {
            ushort lenght;
            string message;
            if (!packet.TryReadUShort(out lenght))
                return;

            if (!packet.TryReadString(out message, lenght))
                return;
               using(var pack = new Packet(SH38Type.GuildAcademyChangeDetailsResponse))
               {
                   pack.WriteUShort(6016);//code for ok
                   client.SendPacket(pack);
               }
               if (client.Character.Academy == null)
                   return;
               client.Character.Academy.Details.UpdateAcademyDetails(client.Character.Academy.Name, message,client.Character.Character.Name);

        }
        [PacketHandler(CH38Type.JumpToMember)]
        public static void JumpToMember(WorldClient client, Packet packet)
        {
            string pMemberName;
            if (!packet.TryReadString(out pMemberName, 16))
                return;

            if (client.Character.Academy == null)
                return;
            AcademyMember pMember = client.Character.Academy.AcademyMembers.Find(m => m.pClient.Character.Character.Name == pMemberName);
            if(pMember != null)
            {
                AcademyMember mMember = client.Character.Academy.AcademyMembers.Find(m => m.pMemberName == client.Character.Character.Name);
                mMember.ChangeMap(mMember.MapName, pMember.MapName);
                client.Character.ChangeMap(pMember.MapName);
            }
        }
        [PacketHandler(CH38Type.GuildAcademyRequestList)]
        public static void GuildAcademyRequestList(WorldClient client, Packet packet)
        {
            if (client.Character.Academy == null)
                return;
            for (int i = 0; i < client.Character.Academy.AcademyMembers.Count; i += 20)
            {
                Packet pack = Academy.MultiMemberList(client.Character.Academy.AcademyMembers, i, i + Math.Min(20, client.Character.Academy.AcademyMembers.Count - i), client.Character.Academy.AcademyMembers.Count);
                client.SendPacket(pack);
            }
        }
    }
}
