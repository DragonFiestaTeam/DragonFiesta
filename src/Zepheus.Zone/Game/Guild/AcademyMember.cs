using System;
using Zepheus.Database.DataStore;
using Zepheus.FiestaLib.Networking;
using Zepheus.FiestaLib;
using System.Data;

namespace Zepheus.Zone.Game
{
   public class AcademyMember : GuildMember
    {
        #region Properties
        public Academy Academy { get; set; }

        #endregion

        public  static  new AcademyMember  LoadFromDatabase(DataRow row)
        {
            AcademyMember pMember = new AcademyMember
            {
                CharID = GetDataTypes.GetInt(row["CharID"]),
                GuildID = GetDataTypes.GetInt(row["OwnerGuildID"]),
            };
            return pMember;
        }
        
        #region Packets
        #endregion
    }
}
