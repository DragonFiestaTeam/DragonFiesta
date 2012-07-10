using System;
using System.Collections.Generic;
using System.Data;
using Zepheus.FiestaLib;
using Zepheus.Database;
using Zepheus.FiestaLib.Networking;
using Zepheus.Database.DataStore;
using Zepheus.Util;

namespace Zepheus.World.Data
{
    public class Academy : Guild
    {
        public List<AcademyMember> AcademyMembers { get; set; }
        public Guild Guild { get; set; }
        public override int ID { get; set; }
        public override string Name { get; set; }
        public override DetailsMessage Details { get; set; }
    
        public  Academy()
        {
        }
        public  AcademyMember GetAcademyMemberByName(string CharName)
        {
            return this.AcademyMembers.Find(m => m.pMemberName == CharName);
        }
        public override void LoadMembers()
        {
            DataTable MemberData = null;
            DataTable GuildExtraData = null;
            using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
            {
                MemberData = dbClient.ReadDataTable("SELECT* FROM AcademyMembers WHERE OwnerGuildID='"+this.ID+"'");
                GuildExtraData = dbClient.ReadDataTable("SELECT* FROM Characters WHERE AcademyID='" + this.ID+ "'");
            }
            if (MemberData != null)
            {
                foreach (DataRow row in MemberData.Rows)
                {
                    AcademyMember pMember = AcademyMember.LoadFromDatabase(row);
                    this.AcademyMembers.Add(pMember);
                }
            }
            if (GuildExtraData != null)
            {
                foreach (DataRow row in GuildExtraData.Rows)
                {
                    int CharID = GetDataTypes.GetInt(row["CharID"]);
                    AcademyMember pMember = this.AcademyMembers.Find(m => m.CharID == CharID);
                    if (pMember != null)
                    {
                        pMember.LoadMemberExtraData(row);
                    }
                    else
                    {
                        this.AcademyMembers.Remove(pMember);
                        Log.WriteLine(LogLevel.Warn, "Failed Load GuildAcademy ExtraData By Character {0}", CharID);
                    }
                }
            }
        }
        #region Packets
  
        public static Packet MultiMemberList(List<AcademyMember> objs, int start, int end,int countGesammt)
        {
           Packet packet = new Packet(SH38Type.GuildAcademyList);
           packet.WriteUShort((ushort)countGesammt);//GuildMembercount
           int Rest = countGesammt - end;
           packet.WriteUShort((ushort)Rest);// GuildMemberCount - ForeachCount = RestCount
           packet.WriteUShort((ushort)end);//foreachcount
            for (int i = start; i < end; i++)
            {
                objs[i].WriteInfo(packet);
            }
            return packet;
        }
        public void ChangeMemberLevel(string name, byte level)
        {
            foreach (var pMember in this.AcademyMembers)
            {
                pMember.SendMemberLevel(level, name);
            }

        }
        public void MemberLeave(AcademyMember LeavepMember)
        {
            //Todo More Leave Logic
            LeavepMember.RemoveFromDatabase();
            this.AcademyMembers.Remove(LeavepMember);
            foreach (var pMember in this.AcademyMembers)
            {
                pMember.SendMemberLeave(LeavepMember.pMemberName);
            }
        }
        public void MemberJoin(AcademyMember JoinMember)
        {
            foreach (var pMember in this.AcademyMembers)
            {
                if (pMember.isOnline)
                {
                    this.SendAcademyJoin(pMember);
                }
            }
        }
        private void SendAcademyJoin(AcademyMember pMember)
        {
            using (var packet = new Packet(SH38Type.GuildAcademyJoin))
            {
                pMember.WriteInfo(packet);
                pMember.pClient.SendPacket(packet);
            }
        }
        public static void SendAcademyLeaveRequest(AcademyRequestCode pCode,Networking.WorldClient pClient)
        {
            using(var packet = new Packet(SH38Type.GuildAcademyLeaveResponse))
            {
               packet.WriteUShort((ushort)pCode);
               pClient.SendPacket(packet);
            }
        }
        #endregion
    }
}
