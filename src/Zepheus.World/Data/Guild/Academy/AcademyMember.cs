using System;
using Zepheus.Database.DataStore;
using Zepheus.World.Networking;
using Zepheus.FiestaLib;
using System.Data;

namespace Zepheus.World.Data
{
   public class AcademyMember
    {
        #region Properties
        public WorldClient pClient { get; set; }
        public int GuildAcademyID { get; set; }
        public bool isOnline { get; set; }
        public GuildAcademyRank  Rank { get; set; }
        public byte Level { get; set; }
        public string pMemberName { get; set; }
        public byte pMemberJob { get; set; }
        public int CharID { get; set; }
        public int OwnerGuild { get; set; }

        public string MapName { get; set; }
        #endregion

        public static AcademyMember LoadFromDatabase(DataRow row)
        {
            AcademyMember pMember = new AcademyMember
            {
                CharID = GetDataTypes.GetInt(row["CharID"]),
                Rank = (GuildAcademyRank)GetDataTypes.GetByte(row["Rank"]),
                OwnerGuild = GetDataTypes.GetInt(row["OwnerGuildID"]),
                GuildAcademyID = GetDataTypes.GetInt(row["GuildAcademyID"]),
            };
            return pMember;
        }
    }
}
