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
            };
            pMember.AddToDatabase();
            client.Character.Academy = pGuild.GuildAcademy;
            client.Character.Character.AcademyID = pGuild.ID;
            client.Character.UpdateGuildAcademyID();
            pGuild.GuildAcademy.AcademyMembers.Add(pMember);
            AcademyManager.Instance.SendAcademyRequest(client, AcademyRequestCode.Sucess, AcademyName);
            InterServer.InterHandler.AddGuildMemberToZone(false, pMember.GuildID, client.Character.Character.Name, client.Character.Character.ID);
            pGuild.GuildAcademy.MemberJoin(pMember);//Send Join Packet To all
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
               client.Character.Academy.Details.UpdateAcademyDetails(client.Character.Academy.Name, message);

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
