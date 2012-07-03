using System;
using Zepheus.Database.DataStore;
using Zepheus.World.Networking;
using Zepheus.FiestaLib;
using System.Data;

namespace Zepheus.World.Data
{
   public class AcademyMember : GuildMember
    {
        #region Properties

        public GuildAcademyRank  Rank { get; set; }

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
    }
}
