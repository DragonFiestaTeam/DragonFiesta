using System;
using Zepheus.FiestaLib.Networking;

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
        public void UpdateGuildDetails(Guild gGuild, string Name,string Message,ushort lenght)
        {
            this.CreateTime = DateTime.Now;
            this.Creater = Name;
            this.Message = Message;
            this.lenght = lenght;
            Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Guild SET GuildMessage='" + this.Message + "',GuildMessageCreateDate='" + this.CreateTime + "',GuildMessageCreater='" + this.Creater + "' WHERE GuildID='"+gGuild.ID+"'");
            foreach(var pMember in gGuild.GuildMembers)
            {
                pMember.WriteGuildUpdateDetails(this);
            }
        }
        public void WriteMessageAsGuildAcadmyler(Packet pPacket,Academy pAcademy)
        {
            pPacket.WriteUShort(8433);
            pPacket.WriteUShort(0);//unk
            pPacket.WriteByte(1);//unk
            pPacket.WriteString(this.GuildOwner, 16);
            pPacket.WriteUShort(3);//membercount
            pPacket.WriteUShort(50);//maxmembercount
            pPacket.WriteInt(pAcademy.Guild.ID);//academyid
            pPacket.WriteInt(9);//weeks //Todo Calculate Weeks
            pPacket.WriteInt(pAcademy.GuildBuffTime);//time in sek
            pPacket.Fill(128, 0x00);//GuildAcademyBUff
            pPacket.WriteString(this.Message, 512);
        }
        public void WriteMessageAsGuildMember(Packet pPacket,Guild pGuild)
        {
            pPacket.WriteInt(pGuild.ID);//GuildID?
            pPacket.WriteInt(pGuild.ID);//AcademyID?
            pPacket.WriteString(pGuild.GuildMaster, 16);
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
            pPacket.WriteInt(this.CreateTime.Year);//creae details year 1900- 2012
            pPacket.WriteInt(1);//unk
            pPacket.WriteUShort(2);
            pPacket.Fill(6, 0);//unk
            pPacket.WriteString(this.Creater, 16);
            pPacket.WriteString(this.Message, 512);//details message
        }
    }
}
