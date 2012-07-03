using System;
using System.Collections.Generic;
using System.Data;
using Zepheus.Database;
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

        public  Academy()
        {
        }
        public virtual AcademyMember GetMemberByName(string CharName)
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
    }
}
