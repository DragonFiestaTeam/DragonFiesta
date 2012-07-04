using System;
using Zepheus.FiestaLib.Networking;

namespace Zepheus.World.Data
{
    public class DetailsMessage
    {
        public string GuildOwner { get; set; }
        public string Message { get; set; }

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
        public void WriteMessageAsGuildMember(Packet pPacket)
        {
            //Save AcademyMessage
        }
    }
}
