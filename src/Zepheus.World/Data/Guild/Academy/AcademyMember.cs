using System;
using Zepheus.World.Managers;
using Zepheus.Database.DataStore;
using Zepheus.World.Networking;
using Zepheus.FiestaLib.Networking;
using Zepheus.FiestaLib;
using System.Data;

namespace Zepheus.World.Data
{
   public class AcademyMember : GuildMember
    {
        #region Properties

        public GuildAcademyRank  Rank { get; set; }
        public Academy Academy { get; set; }

        #endregion

        public  static  new AcademyMember  LoadFromDatabase(DataRow row)
        {
            AcademyMember pMember = new AcademyMember
            {
                CharID = GetDataTypes.GetInt(row["CharID"]),
                Rank = (GuildAcademyRank)GetDataTypes.GetByte(row["Rank"]),
                GuildID = GetDataTypes.GetInt(row["OwnerGuildID"]),
            };
            return pMember;
        }
        public override void AddToDatabase()
        {
                Program.DatabaseManager.GetClient().ExecuteQuery("INSERT INTO academymembers (OwnerGuildID,CharID,Rank) VALUES ('" + this.GuildID + "','" + this.CharID + "','" + this.Rank + "')");
        }
        public override void SendMemberStatus(bool Status, string name)
        {
            if (Status)
            {
                SetOnline(name);
            }
            else
            {
                SetOffline(name);
            }
        }
        #region Packets
        private void SetOffline(string name)
        {
            //Todo Packet

        }
        private void SetOnline(string name)
        {
           using(var packet = new Packet(SH38Type.GuildAcademyMemberLoggetOn))
           {
               packet.WriteString(name, 16);
               this.pClient.SendPacket(packet);

           }
        }
       public void WriteInfo(Packet pPacket)
        {
            pPacket.WriteString("AddName", 16);
            pPacket.Fill(65, 0x00);//unk
            pPacket.WriteBool(true);
            pPacket.Fill(3, 0x00);//unk
            pPacket.WriteByte(21);//job 
            pPacket.WriteByte(90);//level
            pPacket.WriteByte(0);// unk
            pPacket.WriteString("Rou", 12);//mapName
            pPacket.WriteByte(20);//month
            pPacket.WriteByte(21);//year
            pPacket.WriteByte(30);//day
            pPacket.WriteByte(0);//unk
            pPacket.WriteByte(0);  //unk

        }
       public void ChangeMap(string Name,string MapName)
        {
           if(this.isOnline)
           using(var packet = new Packet(SH38Type.GuildAcademyMemberChangeMap))
           {
               packet.WriteString(Name, 16);
               packet.WriteString(MapName, 12);
               this.pClient.SendPacket(packet);

           }

        }
        #endregion
    }
}
