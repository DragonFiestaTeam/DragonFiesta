﻿using System;
using System.Collections.Generic;
using System.Data;
using System;
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
                        Log.WriteLine(LogLevel.Warn, "Failed Load GuildAcademy ExtraData By Character {0}", CharID);
                    }
                }
            }
        }
        #region Packets
        public void ChangeDekan(string name)
        {
            using (var packet = new Packet(SH38Type.GuildAcademyDekanChange))
            {
                packet.WriteString(name);

            }
        }
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
        public void SendAcademyJoin(AcademyMember pMember )
        {
            using(var packet = new Packet(SH38Type.GuildAcademyJoin))
            {
                pMember.WriteInfo(packet);

            }
        }
        #endregion
    }
}
