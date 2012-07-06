using System;
using System.Collections.Generic;
using System.Data;
using Zepheus.FiestaLib;
using Zepheus.Database;
using Zepheus.FiestaLib.Networking;
using Zepheus.Database.DataStore;
using Zepheus.Util;

namespace Zepheus.Zone.Game
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
        public  AcademyMember GetAcademyMemberByName(string CharName)
        {
            return this.AcademyMembers.Find(m => m.pMemberName == CharName);
        }
        public void GiveGuildAcademyReward()
        {
            //Todo
            var packet = new Packet(SH38Type.GuildAcademyReward);
            packet.WriteByte(3);
            packet.WriteByte(1);
            packet.WriteString("Mother", 16);
            packet.WriteUShort(250);//Reward itemID
            packet.WriteByte(0);//unk
            packet.WriteLong(90);//count
            packet.WriteLong(0);//unk

           // character.Client.SendPacket(packet);
        }
        public override void LoadMembers()
        {
            DataTable MemberData = null;
            DataTable GuildExtraData = null;
            using (DatabaseClient dbClient = Program.CharDBManager.GetClient())
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
        #endregion
    }
}
