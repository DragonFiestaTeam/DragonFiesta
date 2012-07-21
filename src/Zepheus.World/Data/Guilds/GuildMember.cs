using System;
using System.Data;
using System.Data.SqlClient;
using Fiesta.World.Game.Characters;
using Fiesta.World.Networking;

namespace Fiesta.World.Game.Guilds
{
    public sealed class GuildMember
    {
        public Guild Guild { get; private set; }
        public Character Character { get; private set; }


        public GuildRank Rank { get; set; }
        public ushort Corp { get; set; }



        private object ThreadLocker;


        
        public GuildMember(Guild Guild, Character Character, GuildRank Rank, ushort Corp)
        {
            this.Guild = Guild;
            this.Character = Character;

            this.Rank = Rank;
            this.Corp = Corp;


            ThreadLocker = new object();
        }
        public void Dispose()
        {
            Guild = null;
            Character = null;
        }



        public void Save(SqlConnection con = null)
        {
            lock (ThreadLocker)
            {
                var conCreated = (con == null);
                if (conCreated)
                {
                    con = DatabaseManager.DB_Game.GetConnection();
                }



                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.GuildMember_Save";

                    cmd.Parameters.Add(new SqlParameter("@pGuildID", Guild.ID));
                    cmd.Parameters.Add(new SqlParameter("@pCharacterID", Character.ID));

                    cmd.Parameters.Add(new SqlParameter("@pRank", (byte)Rank));
                    cmd.Parameters.Add(new SqlParameter("@pCorp", (short)Corp));



                    cmd.ExecuteNonQuery();
                }



                if (conCreated)
                {
                    con.Dispose();
                }
            }
        }






        public void WriteInfo(GamePacket Packet)
        {
            Packet.WriteString(Character.Name, 16);
            Packet.WriteByte((byte)Rank);
            Packet.WriteInt32(0); //unk ?

            Packet.WriteUInt16(Corp);
            Packet.WriteByte(0);
            Packet.WriteUInt16(0xFFFF); //unk
            Packet.WriteUInt16(0xFFFF); //unk
            Packet.WriteByte(0);
            Packet.WriteInt32(32);
            Packet.WriteInt32(32);
            Packet.Fill(50, 0x00); // unk
            Packet.WriteByte((byte)(Character.IsOnline ? 0xB9 : 0x00));
            Packet.Fill(3, 0x00); // unk
            Packet.WriteByte(Character.Class.ID);
            Packet.WriteByte(Character.Level);
            Packet.WriteByte(0);
            Packet.WriteString(Character.Map.IndexName, 12);
        }
        public void BroadcastGuildName()
        {
            var packet = new GamePacket(GameOpCode.Server.H29.GuildNameResult);
            packet.WriteInt32(Guild.ID);
            packet.WriteString(Character.Name, 16);

            BroadcastManager.BroadcastInRange(Character, packet, false);
        }
    }
}