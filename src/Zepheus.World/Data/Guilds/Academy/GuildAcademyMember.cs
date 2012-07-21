using System;
using System.Data;
using System.Data.SqlClient;
using Fiesta.World.Game.Characters;
using Fiesta.World.Networking;

namespace Fiesta.World.Game.Guilds.Academy
{
    public sealed class GuildAcademyMember
    {
        public GuildAcademy Academy { get; private set; }

        public Character Character { get; private set; }
        public GuildAcademyRank Rank { get; set; }

        public DateTime RegisterDate { get; private set; }
        public bool IsChatBlocked { get; set; }




        public GuildAcademyMember(GuildAcademy Academy, Character Character, SqlDataReader reader)
        {
            this.Academy = Academy;
            this.Character = Character;

            Load(reader);
        }
        public GuildAcademyMember(GuildAcademy Academy, Character Character, DateTime RegisterDate, GuildAcademyRank Rank)
        {
            this.Academy = Academy;
            this.Character = Character;
            this.RegisterDate = RegisterDate;
            this.Rank = Rank;
        }
        public void Dispose()
        {
            Academy = null;
            Character = null;
        }



        private void Load(SqlDataReader reader)
        {
            RegisterDate = reader.GetDateTime(2);
            IsChatBlocked = reader.GetBoolean(3);
            Rank = (GuildAcademyRank)reader.GetByte(4);
        }
        public void Save(SqlConnection con)
        {
            var conCreated = (con == null);
            if (conCreated)
            {
                con = DatabaseManager.DB_Game.GetConnection();
            }


            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = "dbo.GuildAcademyMember_Save";
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(new SqlParameter("@pGuildID", Academy.Guild.ID));
                cmd.Parameters.Add(new SqlParameter("@pCharacterID", Character.ID));
                cmd.Parameters.Add(new SqlParameter("@pIsChatBlocked", IsChatBlocked));
                cmd.Parameters.Add(new SqlParameter("@pRank", (byte)Rank));


                
                cmd.ExecuteNonQuery();
            }


            if (conCreated)
            {
                con.Dispose();
            }
        }









        public void WriteInfo(GamePacket Packet)
        {
            Packet.WriteString(Character.Name, 16);
            Packet.Fill(65, 0x00);//unk
            Packet.WriteBool(Character.IsOnline);
            Packet.Fill(3, 0x00);//unk
            Packet.WriteByte(Character.Class.ID);//job 
            Packet.WriteByte(Character.Level);//level
            Packet.WriteByte(0);// unk
            Packet.WriteString(Character.Map.IndexName, 12);//mapName
            Packet.WriteByte((byte)RegisterDate.Month);//month
            Packet.WriteByte(184);//year fortmat unkown
            Packet.WriteByte((byte)RegisterDate.Day);//day
            Packet.WriteByte(0);//unk
            Packet.WriteByte(0);  //unk
        }
        public void BroadcastGuildName()
        {
            var packet = new GamePacket(GameOpCode.Server.H29.GuildNameResult);
            packet.WriteInt32(Academy.Guild.ID);
            packet.WriteString(Character.Name, 16);

            BroadcastManager.BroadcastInRange(Character, packet, false);
        }
    }
}