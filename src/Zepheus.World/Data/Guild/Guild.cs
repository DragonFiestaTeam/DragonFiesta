using System;
using Zepheus.Database.DataStore;
using System.Collections.Generic;
using Zepheus.FiestaLib.Networking;
using Zepheus.FiestaLib;
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
                Creater = row["GuildMessageCreater"].ToString(),
                CreateTime = DateTime.Parse(row["GuildMessageCreateDate"].ToString()),
           };
            g.GuildAcademy.Details = new DetailsMessage
             {
                 Message = row["GuildAcademyMessage"].ToString(),
                 GuildOwner = g.GuildMaster,
             };
            g.LoadMembers();
            return g;
        }
        public virtual void RemoveMember(string Name)
        {
           GuildMember pMember = this.GuildMembers.Find(m => m.pMemberName == Name);
           pMember.RemoveFromDatabase();
              using(var packet = new Packet(SH29Type.RemoveGuildMember))
              {
                 packet.WriteString(Name,16);
                 this.SendPacketToAllOnlineMember(packet);
              }
           this.GuildMembers.Remove(pMember);
        }
        public void SendChatMessage(string Sender, string Message)
        {
             using (var packet = new Packet(SH29Type.GuildChatMessage))
             {
                 packet.WriteInt(this.ID);
                 packet.WriteString(Sender, 16);
                 packet.WriteUShort(0);//unk
                 packet.WriteByte((byte)Message.Length);
                 packet.WriteString(Message, Message.Length);
                 this.SendPacketToAllOnlineMember(packet);
             }
        }
        public virtual void SendPacketToAllOnlineMember(Packet packet)
        {
            foreach (var pMember in this.GuildMembers)
            {
                if (pMember.isOnline)
                    pMember.pClient.SendPacket(packet);
            }
        }
        public void SendRemoveMemberFromGuild(string name)
        {
            using (var packet = new Packet(SH29Type.RemoveGuildMember))
            {
                packet.WriteString(name, 16);
                SendPacketToAllOnlineMember(packet);
            }
        }
        public static Packet MultiMemberList(List<GuildMember> objs, int start, int end, int countGesammt)
        {
            Packet packet = new Packet(SH29Type.GuildList);
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
        Program.DatabaseManager.GetClient().ExecuteQuery("INSERT INTO Guild (ID,Name,Password,GuildMaster,GuildWar) VALUES ('"+this.ID+"','"+this.Name+"','"+this.GuildPassword+"','"+GuildMaster+"','"+Convert.ToByte(this.GuildWar)+"')");
        }
        public void ChangeGuildMaster(string NewMaster)
        {
            Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Guild SET GuildMaster ='" + NewMaster + "' WHERE ID ='" + this.ID + "'");
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
                       this.GuildMembers.Remove(pMember);
                       Log.WriteLine(LogLevel.Warn, "Failed Load Guild ExtraData By Character {0}", CharID);
                   }
               }
           }
       }
    }
}
