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
        public static void LeaveGuild(WorldClient client, Packet packet)
        {
            if (client.Character.Guild == null)
                return;
            //todo check rank

            string KickName;
            if (!packet.TryReadString(out KickName, 16))
                return;
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
            };
            pMember.AddToDatabase();
            client.Character.Academy = pGuild.GuildAcademy;
            client.Character.Character.AcademyID = pGuild.ID;
            client.Character.UpdateGuildAcademyID();
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
              pMember.WriteInfo(packet);
              pGuild.SendPacketToAllOnlineMember(pack);
              pGuild.GuildAcademy.SendPacketToAllOnlineMember(pack);
            }
          
        }
        [PacketHandler(CH38Type.GuildAcademyLeave)]
        public static void GuildAcademyLeave(WorldClient client, Packet packet)
        {
            if (client.Character.Academy == null)
                return;
            //Todo Response
            AcademyMember pMember = client.Character.Academy.AcademyMembers.Find(m => m.pMemberName == client.Character.Character.Name);
            if (pMember.RegisterDate.Subtract(DateTime.Now).TotalMinutes <= 60)
            {
                Academy.SendAcademyLeaveRequest(AcademyRequestCode.OneHoursLeave, client);
                return;
            }
            InterServer.InterHandler.RemoveGuildMemberFromZone(false, pMember.GuildID, pMember.CharID);
            Academy.SendAcademyLeaveRequest(AcademyRequestCode.LeaveSucess, client);
            client.Character.Academy.MemberLeave(pMember);
            
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
