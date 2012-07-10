using System;
using Zepheus.FiestaLib.Networking;
using Zepheus.FiestaLib;

namespace Zepheus.World.Data
{
    public class DetailsMessage
    {
        public string GuildOwner { get; set; }
        public string Message { get; set; }
        public ushort lenght { get; set; }
        public DateTime CreateTime { get; set; }
        public string Creater { get; set; }

        public void AddToDatabase(int GuildID, bool Type)
        {
            if(Type)
            {
                //Save GuildMEssage
            }
            else
            {
                //Save AcademyMessage
            }
        }
        public void UpdateGuildDetails(Guild gGuild, string Name,string Message)
        {
            this.CreateTime = DateTime.Now;
            this.Creater = Name;
            this.Message = Message;
            this.lenght = (ushort)Message.Length;
            Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Guild SET GuildMessage='" + this.Message + "',GuildMessageCreateDate='" + this.CreateTime.ToDBString() + "',GuildMessageCreater='" + this.Creater + "' WHERE ID='"+gGuild.ID+"'");
            foreach(var pMember in gGuild.GuildMembers)
            {
                if(pMember.isOnline)
                using (var pack = new Packet(SH29Type.SendUpdateDetails))
                {
                    pMember.WriteGuildUpdateDetails(this, pack);
                    pMember.pClient.SendPacket(pack);
                }
            }
        }
         public void UpdateAcademyDetails(string GuildName ,string Message)
        {
             Guild g;
             if (!DataProvider.Instance.GuildsByName.TryGetValue(GuildName, out g))
                 return;

            this.CreateTime = DateTime.Now;
            this.Message = Message;
            this.lenght = (ushort)Message.Length;
            Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Guild SET GuildAcademyMessage='" + this.Message + "' WHERE ID='" + g.ID + "'");
            foreach (var pMember in g.GuildMembers)
            {
                if (pMember.isOnline)
                    using (var pack = new Packet(SH29Type.SendUpdateDetails))
                    {
                        pMember.WriteGuildUpdateDetails(this, pack);
                        pMember.pClient.SendPacket(pack);
                    }
            }
            foreach (var pMember in g.GuildAcademy.AcademyMembers)//send to members
            {
                if (pMember.isOnline)
                    using (var pack = new Packet(SH29Type.SendUpdateDetails))
                    {
                        pMember.WriteGuildUpdateDetails(this, pack);
                        pMember.pClient.SendPacket(pack);
                    }
            }
        }
        public void WriteMessageAsGuildAcadmyler(Packet pPacket,Academy pAcademy)
        {
            pPacket.WriteInt(pAcademy.ID);
            pPacket.WriteByte(1);//unk
            pPacket.WriteString(this.GuildOwner, 16);
            pPacket.WriteUShort((ushort)pAcademy.AcademyMembers.Count);//membercount
            pPacket.WriteUShort(50);//maxmembercount
            pPacket.WriteInt(pAcademy.Guild.ID);//academyid
            pPacket.WriteInt((int)pAcademy.RegisterDate.DayOfWeek);//weeks //Todo Calculate Weeks
            pPacket.WriteInt(pAcademy.GuildBuffTime);//time in sek
            pPacket.Fill(128, 0x00);//GuildAcademyBUff
            pPacket.WriteString(this.Message, 512);
        }
        public void WriteMessageAsGuildMember(Packet pPacket,Guild pGuild)
        {
            pPacket.WriteInt(pGuild.ID);//GuildID?
            pPacket.WriteInt(pGuild.ID);//AcademyID?
            pPacket.WriteString(pGuild.Name, 16);
            //this shit later
            pPacket.Fill(24, 0x00);//unk
            pPacket.WriteUShort(38);
            pPacket.WriteInt(100);
            pPacket.Fill(233, 0x00);//unk
            pPacket.WriteUShort(11779);
            pPacket.WriteUShort(20082);
            pPacket.WriteInt(31);
            pPacket.WriteInt(55);
            pPacket.WriteInt(18);//unk
            pPacket.WriteInt(15);
            pPacket.WriteInt(8);//unk
            pPacket.WriteInt(111);//unk
            pPacket.WriteInt(4);
            pPacket.Fill(136, 0);//buff or string
            pPacket.WriteUShort(1824);
            pPacket.WriteUShort(20152);
            pPacket.WriteInt(16);
            pPacket.WriteInt(28);
            pPacket.WriteInt(this.CreateTime.Minute);//createDetails Guild Minutes Date
            pPacket.WriteInt(this.CreateTime.Hour); //create Details Guild Hours Date
            pPacket.WriteInt(this.CreateTime.Day);//create details Guild Day Date
            pPacket.WriteInt(this.CreateTime.Month);//create details Month
            pPacket.WriteInt(this.CreateTime.ToFiestaYear());//creae details year 1900- 2012
            pPacket.WriteInt(1);//unk
            pPacket.WriteUShort(2);
            pPacket.Fill(6, 0);//unk
            pPacket.WriteString(this.Creater, 16);
            pPacket.WriteString(this.Message, 512);//details message
        }
    }
}
