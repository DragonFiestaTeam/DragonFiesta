using System;
using Zepheus.Database.DataStore;
using System.Collections.Generic;
using System.Data;
using Zepheus.Util;
using Zepheus.Database;

namespace Zepheus.World.Data
{
    public class Guild
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public List<GuildMember> GuildMembers { get; set; }
        public string GuildPassword { get; set; }
        public string GuildMaster { get; set; }
        public bool GuildWar { get; set; }

        public virtual int GuildBuffTime { get; set; }
        public virtual ushort MaxMemberCount { get; set; }
        public DateTime RegisterDate { get; set; }

        public Academy GuildAcademy { get; set; }
        public virtual DetailsMessage Details { get; set; }

        public  static Guild LoadFromDatabase(DataRow row)
        {
            Guild g = new Guild
            {
               ID = GetDataTypes.GetInt(row["ID"]),
               Name = row["Name"].ToString(),
               GuildPassword = row["Password"].ToString(),
               GuildMaster = row["GuildMaster"].ToString(),
               GuildWar = GetDataTypes.GetBool(row["GuildWar"]),
            };
            g.GuildAcademy = new Academy
            {
                Guild = g,
                ID = g.ID,
                Name = g.Name,
                AcademyMembers = new List<AcademyMember>(),
            };
            g.Details = new DetailsMessage
           {
               Message = row["GuildMessage"].ToString(),
               GuildOwner = g.GuildMaster,
           };
            g.GuildAcademy.Details = new DetailsMessage
             {
                 Message = row["GuildAcademyMessage"].ToString(),
                 GuildOwner = g.GuildMaster,
             };
            g.LoadMembers();
            return g;
        }
        public virtual GuildMember GetMemberByName(string CharName)
        {
          return  this.GuildMembers.Find(m => m.pMemberName == CharName);
        }
        public Guild()
        {
            this.GuildMembers = new List<GuildMember>();
        }
        public void AddToDatabase()
        {
        Program.DatabaseManager.GetClient().ExecuteQuery("INSERT INTO Guild (ID,Name,Password,GuildMaster) VALUES ('"+this.ID+"','"+this.Name+"','"+this.GuildPassword+"','"+GuildMaster+"')");
        }
 
       public virtual void LoadMembers()
       {
           DataTable MemberData = null;
           DataTable GuildExtraData = null;
           using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
           {
               MemberData = dbClient.ReadDataTable("SELECT* FROM GuildMembers WHERE GuildID='"+this.ID+"'");
               GuildExtraData = dbClient.ReadDataTable("SELECT* FROM Characters WHERE GuildID='" + this.ID + "'");
           }
           if (MemberData != null)
           {
               foreach (DataRow row in MemberData.Rows)
               {
                   GuildMember pMember = GuildMember.LoadFromDatabase(row);
                   this.GuildMembers.Add(pMember);
               }
           }
           if (GuildExtraData != null)
           {
               foreach (DataRow row in GuildExtraData.Rows)
               {
                   int CharID = GetDataTypes.GetInt(row["CharID"]);
                   GuildMember pMember = this.GuildMembers.Find(m => m.CharID == CharID);
                   if(pMember != null)
                   {
                       pMember.LoadMemberExtraData(row);
                   }
                   else
                   {
                       Log.WriteLine(LogLevel.Warn, "Failed Load Guild ExtraData By Character {0}", CharID);
                   }
               }
           }
       }
    }
}
