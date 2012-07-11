using Zepheus.World.Data;
using System.Collections.Generic;
using Zepheus.World.Networking;
using Zepheus.FiestaLib;
using Zepheus.Database.DataStore;
using System.Data;
using Zepheus.FiestaLib.Networking;

namespace Zepheus.World.Data
{
    public  class GuildMember
    {
        #region Properties
        public virtual WorldClient pClient { get;  set; }
        public virtual int GuildID { get;  set; }
        public virtual bool isOnline { get; set; }
        public GuildRanks GuildRank { get; set; }
        public virtual byte Level { get; set; }
        public virtual string pMemberName { get; set; }
        public virtual byte pMemberJob { get; set; }
        public ushort Korp { get; set; }
        public virtual int CharID { get; set; }
        public virtual string MapName { get; set; }
        #endregion
        #region .ctor
        public GuildMember()
        {
            this.MapName = "Rou";
        }
        #endregion
        #region Methods
        public  static GuildMember LoadFromDatabase(DataRow row)
        {
            GuildMember pMember = new GuildMember
            {
                CharID = GetDataTypes.GetInt(row["CharID"]),
                GuildRank = (GuildRanks)GetDataTypes.GetByte(row["Rank"]),
                Korp = GetDataTypes.GetUshort(row["Korp"]),
                GuildID = GetDataTypes.GetInt(row["GuildID"]),
            };
         
            return pMember;
        }
        public void LoadMemberExtraData(DataRow row)
        {
    
            this.Level = GetDataTypes.GetByte(row["Level"]);
            this.pMemberJob = GetDataTypes.GetByte(row["Job"]);
            this.pMemberName = row["Name"].ToString();
            ushort mapid = GetDataTypes.GetUshort(row["Map"]);
            this.MapName = DataProvider.GetMapname(mapid);
        }

        public virtual void SendMemberStatus(bool Status, string name)
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
        public void WriteGuildUpdateDetails(DetailsMessage details,Packet pPacket)
        {
            pPacket.Fill(4, 0x00);//unk
            pPacket.WriteInt(details.CreateTime.Second);
            pPacket.WriteInt(details.CreateTime.Minute);//hour?
            pPacket.WriteInt(details.CreateTime.Hour);
            pPacket.WriteInt(details.CreateTime.Day);
            pPacket.WriteInt(details.CreateTime.Month-1);//month or yeas
            pPacket.WriteInt(details.CreateTime.ToFiestaYear());//year
            pPacket.WriteInt(0);//unk
            pPacket.WriteLong(0);//unk
            pPacket.WriteString(details.Creater, 16);
            pPacket.WriteUShort((ushort)details.Message.Length);
            pPacket.WriteString(details.Message, details.Message.Length);
        }
        public virtual void WriteInfo(Packet Ppacket)
        {
            Ppacket.WriteString(this.pMemberName, 16);
            Ppacket.WriteByte((byte)this.GuildRank);//rank
            Ppacket.WriteInt(0);

            Ppacket.WriteUShort(this.Korp);//korp
            Ppacket.WriteByte(0);//unk
            Ppacket.WriteUShort(0xffff);//unk
            Ppacket.WriteUShort(0xffff);//unk
            Ppacket.WriteByte(0);//unk
            Ppacket.WriteInt(32);
            Ppacket.WriteInt(32);
            Ppacket.Fill(50, 0x00);//unk
            Ppacket.WriteByte(this.isOnline ? (byte)0xB9 : (byte)0x00);//onlinestatus
            Ppacket.Fill(3, 0x00);//unk
            Ppacket.WriteByte(this.pMemberJob);//job
            Ppacket.WriteByte(this.Level);
            Ppacket.WriteByte(0);//unk
            Ppacket.WriteString(this.MapName, 12);//charmapname
        }

        public virtual void AddToDatabase()
        {
            using(Database.DatabaseClient Client =  Program.DatabaseManager.GetClient())
             {
                 Client.ExecuteQuery("INSERT INTO GuildMembers (CharID,Rank,Korp,GuildID) VALUES ('"+this.CharID+"','"+(byte)this.GuildRank+"','"+this.Korp+"','"+this.GuildID+"')");
                 Client.ExecuteQuery("UPDATE Characters set GuildID =" + this.GuildID + " WHERE CharID=" + this.CharID + "");
             }
        }
        public virtual void RemoveFromDatabase()
        {
            Program.DatabaseManager.GetClient().ExecuteQuery("DELETE FROM GuildMembers WHERE CharID='" + this.CharID + "'");
            Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Characters set GuildID =0 WHERE CharID=" + this.CharID + "");
        }
        #endregion
        #region Packets
        private void SetOffline(string name)
        {
            using (var pack = new Packet(SH29Type.SendMemberGoOffline))
            {
                pack.WriteString(name, 16);
                this.pClient.SendPacket(pack);
            }
        }
        private void SetOnline(string name)
        {
            using (var pack = new Packet(SH29Type.SendMemberGoOnline))
            {
                pack.WriteString(name, 16);
                this.pClient.SendPacket(pack);
            }
        }
        #endregion
    }
}
