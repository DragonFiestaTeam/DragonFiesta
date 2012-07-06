using System.Collections.Generic;
using Zepheus.FiestaLib;
using Zepheus.Database.DataStore;
using System.Data;
using Zepheus.Zone.Networking;
using Zepheus.FiestaLib.Networking;

namespace Zepheus.Zone.Game
{
    public  class GuildMember
    {
        #region Properties
        public virtual ZoneClient pClient { get;  set; }
        public virtual int GuildID { get;  set; }
        public virtual bool isOnline { get; set; }
        public virtual string pMemberName { get; set; }
        public virtual int CharID { get; set; }

        #endregion
        #region .ctor
        public GuildMember()
        {
        }
        #endregion
        #region Methods
        public  static GuildMember LoadFromDatabase(DataRow row)
        {
            GuildMember pMember = new GuildMember
            {
                CharID = GetDataTypes.GetInt(row["CharID"]),
                GuildID = GetDataTypes.GetInt(row["GuildID"]),
            };
            return pMember;
        }
        public void LoadMemberExtraData(DataRow row)
        {
            this.pMemberName = row["Name"].ToString();
        }

        #endregion
        #region Packets
        #endregion
    }
}
