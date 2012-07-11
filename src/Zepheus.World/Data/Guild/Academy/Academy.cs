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
        public void SendChatMessage(Networking.WorldClient pClient, string Sender, string Message)
        {
            using (var packet = new Packet(SH38Type.GuildAcademyChatessage))
            {
                packet.WriteInt(this.ID);
                packet.WriteString(Sender, 16);
                packet.WriteUShort(0);//unk
                packet.WriteByte((byte)Message.Length);
                packet.WriteString(Message, Message.Length);
                pClient.SendPacket(packet);
            }
        }
        public override  void SendPacketToAllOnlineMember(Packet packet)
        {
            foreach (var pMember in this.AcademyMembers)
            {
                if (pMember.isOnline)
                    pMember.pClient.SendPacket(packet);
            }
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
        public void UpdateAcademyMessage(DetailsMessage message, Packet pack)
        {
            pack.WriteUShort((ushort)message.Message.Length);
            pack.WriteString(message.Message, message.lenght);
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
