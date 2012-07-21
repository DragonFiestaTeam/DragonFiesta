using System;
using System.Data;
using System.Data.SqlClient;
using Fiesta.Zone.Game.Characters;

namespace Fiesta.Zone.Game.Guilds.Academy
{
    public sealed class GuildAcademyMember
    {
        public GuildAcademy Academy { get; private set; }

        public int CharacterID { get; private set; }
        public Character Character { get; set; }
        public bool IsOnline { get; set; }
        public bool IsOnThisZone { get { return (Character != null); } }

        public GuildAcademyRank Rank { get; set; }

        public DateTime RegisterDate { get; private set; }
        public bool IsChatBlocked { get; set; }





        public GuildAcademyMember(GuildAcademy Academy, int CharacterID, GuildAcademyRank Rank, DateTime RegisterTime)
        {
            this.Academy = Academy;
            this.CharacterID = CharacterID;
            this.Rank = Rank;
            this.RegisterDate = RegisterDate;
        }
        public GuildAcademyMember(GuildAcademy Academy, SqlDataReader reader)
        {
            this.Academy = Academy;


            Load(reader);
        }
        public void Dispose()
        {
            Academy = null;
            Character = null;
        }




        private void Load(SqlDataReader reader)
        {
            CharacterID = reader.GetInt32(1);
            RegisterDate = reader.GetDateTime(2);
            IsChatBlocked = reader.GetBoolean(3);
            Rank = (GuildAcademyRank)reader.GetByte(4);
        }
    }
}