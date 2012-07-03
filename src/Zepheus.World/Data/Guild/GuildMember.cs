using Zepheus.World.Data;
using Zepheus.World.Networking;
using Zepheus.Database.DataStore;
using System.Data;
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

        public virtual void AddToDatabase()
        {
            using(Database.DatabaseClient Client =  Program.DatabaseManager.GetClient())
             {
                 Client.ExecuteQuery("INSERT INTO GuildMembers (CharID,Rank,Korp,GuildID) VALUES ('"+this.CharID+"','"+this.GuildRank+"','"+this.Korp+"','"+this.GuildID+"')");
                 Client.ExecuteQuery("UPDATE Characters set GuildID =" + this.GuildID + " WHERE CharID=" + this.CharID + "");
             }
        }
        #endregion
        #region Packets
        private void SetOffline(string name)
        {
            //Todo Packet

        }
        private void SetOnline(string name)
        {
            //TodoPacket
        }
        #endregion
    }
}
