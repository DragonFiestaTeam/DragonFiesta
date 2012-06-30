using Zepheus.World.Data;
using Zepheus.World.Networking;
using Zepheus.Database.DataStore;
using System.Data;
namespace Zepheus.World.Data
{
    public sealed class GuildMember
    {
        #region Properties
        public WorldClient pClient { get;  set; }
        public int GuildID { get;  set; }
        public bool isOnline { get; set; }
        public byte GuildRank { get; set; }
        public byte Level { get; set; }
        public string pMemberName { get; set; }
        public byte pMemberJob { get; set; }
        public ushort Korp { get; set; }
        public int CharID { get; set; }
        #endregion
        #region .ctor
        public GuildMember()
        {
        }
        #endregion
        #region Methods
        public static GuildMember LoadFromDatabase(DataRow row)
        {
            GuildMember pMember = new GuildMember
            {
                CharID = GetDataTypes.GetInt(row["CharID"]),
                GuildRank = GetDataTypes.GetByte(row["Rank"]),
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
        }
        public void SetMemberStatus(bool Status, string name)
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

        public void AddToDatabase()
        {
        }
        #endregion
        #region Packets
        private void SetOffline(string name)
        {
            this.isOnline= false;
            //Todo Packet

        }
        private void SetOnline(string name)
        {
            this.isOnline = true;
            //TodoPacket
        }
        #endregion
    }
}
