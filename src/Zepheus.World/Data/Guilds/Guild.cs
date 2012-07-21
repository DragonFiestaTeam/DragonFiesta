using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Fiesta.Core.Cryptography;
using Fiesta.Core.Networking;
using Fiesta.World.Game.Characters;
using Fiesta.World.Game.Guilds.Academy;
using Fiesta.World.Game.Zones;
using Fiesta.World.Networking;

namespace Fiesta.World.Game.Guilds
{
    public sealed class Guild
    {
        public int ID { get; private set; }

        public string Name { get; set; }
        public string Password
        {
            get
            {
                var data = _Password;
                InterCrypto.Decrypt(ref data, 0, data.Length);

                return Encoding.UTF8.GetString(data);
            }
            set
            {
                var data = Encoding.UTF8.GetBytes(value);
                InterCrypto.Encrypt(ref data, 0, data.Length);

                _Password = data;
            }
        }
        private byte[] _Password;


        public bool AllowGuildWar { get; set; }
        public string Message { get; set; }
        public DateTime MessageCreateTime { get; set; }
        public Character MessageCreater { get; set; }

        public DateTime CreateTime { get; private set; }


        public List<GuildMember> Members { get; private set; }
        public GuildMember Master { get { return Members.Find(m => m.Rank == GuildRank.Master); } }

        public GuildAcademy Academy { get; private set; }




        public object ThreadLocker { get; private set; }
        public const int Price = 1000000;





        public Guild(SqlConnection con, int ID, string Name, byte[] Password, bool AllowGuildWar, Character Creater, DateTime CreateTime)
            : this()
        {
            this.ID = ID;

            this.Name = Name;
            _Password = Password;

            this.AllowGuildWar = AllowGuildWar;
            this.CreateTime = CreateTime;

            Message = "";
            MessageCreateTime = WorldService.Instance.Time;
            MessageCreater = Creater;


            Load(con);
        }
        public Guild(SqlConnection con, SqlDataReader reader)
            : this()
        {
            ID = reader.GetInt32(0);

            Name = reader.GetString(1);
            _Password = (byte[])reader.GetValue(2);

            AllowGuildWar = reader.GetBoolean(3);

            Message = reader.GetString(4);
            MessageCreateTime = reader.GetDateTime(5);


            Character creater;
            if (!CharacterManager.GetCharacterByID(reader.GetInt32(6), out creater))
                throw new InvalidOperationException("Can't find character which created guild message. Character ID: " + reader.GetInt32(6));

            MessageCreater = creater;

            CreateTime = reader.GetDateTime(7);

            
            Load(con);
        }
        private Guild()
        {
            ThreadLocker = new object();

            Members = new List<GuildMember>();
        }
        public void Dispose()
        {
            Name = null;
            _Password = null;
            Message = null;
            MessageCreater = null;

            ThreadLocker = null;


            Members.ForEach(m => m.Dispose());
            Members.Clear();
            Members = null;


            Academy.Dispose();
            Academy = null;
        }




        private void Load(SqlConnection con)
        {
            //members
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM GuildMembers WHERE GuildID = @pGuildID";

                cmd.Parameters.Add(new SqlParameter("@pGuildID", ID));


                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //get character
                        Character character;
                        if (!CharacterManager.GetCharacterByID(reader.GetInt32(1), out character, con))
                            continue;

                        var member = new GuildMember(this,
                                                     character,
                                                     (GuildRank)reader.GetByte(2),
                                                     (ushort)reader.GetInt16(3));

                        Members.Add(member);
                    }
                }
            }


            //academy
            Academy = new GuildAcademy(this, con);
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

                //save the guild itself
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.Guild_Save";

                    cmd.Parameters.Add(new SqlParameter("@pID", ID));
                    cmd.Parameters.Add(new SqlParameter("@pName", Name));
                    cmd.Parameters.Add(new SqlParameter("@pPassword", _Password));
                    cmd.Parameters.Add(new SqlParameter("@pAllowGuildWar", AllowGuildWar));
                    cmd.Parameters.Add(new SqlParameter("@pMessage", Message));
                    cmd.Parameters.Add(new SqlParameter("@pMessageCreateTime", MessageCreateTime));
                    cmd.Parameters.Add(new SqlParameter("@pMessageCreaterID", MessageCreater.ID));



                    cmd.ExecuteNonQuery();
                }

                //save members
                foreach (var member in Members)
                {
                    member.Save(con);
                }


                //save aka
                Academy.Save(con);



                if (conCreated)
                {
                    con.Dispose();
                }
            }
        }


        public void Broadcast(GamePacket Packet, GuildMember Exclude = null)
        {
            lock (ThreadLocker)
            {
                foreach (var member in Members)
                {
                    if (Exclude != null
                        && member == Exclude)
                        continue;


                    if (member.Character.IsOnline)
                    {
                        try
                        {
                            member.Character.Client.Send(Packet);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }
        }
        public void WriteGuildInfo(GamePacket Packet)
        {
            Packet.WriteInt32(ID);
            Packet.WriteInt32(ID); // academy id?
            Packet.WriteString(Name, 16);

            Packet.Fill(24, 0x00); //unk
            Packet.WriteUInt16(38);
            Packet.WriteInt32(100);
            Packet.Fill(233, 0x00);//unk
            Packet.WriteUInt16(11779);
            Packet.WriteUInt16(20082);
            Packet.WriteInt32(31);
            Packet.WriteInt32(55);
            Packet.WriteInt32(18);//unk
            Packet.WriteInt32(15);
            Packet.WriteInt32(8);//unk
            Packet.WriteInt32(111);//unk
            Packet.WriteInt32(4);
            Packet.Fill(136, 0);//buff or string
            Packet.WriteUInt16(1824);
            Packet.WriteUInt16(20152);
            Packet.WriteInt32(16);
            Packet.WriteInt32(28);
            Packet.WriteInt32(MessageCreateTime.Minute);//createDetails Guild Minutes Date
            Packet.WriteInt32(MessageCreateTime.Hour); //create Details Guild Hours Date
            Packet.WriteInt32(MessageCreateTime.Day);//create details Guild Day Date
            Packet.WriteInt32(MessageCreateTime.Month);//create details Month
            Packet.WriteInt32(MessageCreateTime.Year - 1900);//creae details year 1900- 2012
            Packet.WriteInt32(10);//unk
            Packet.WriteUInt16(2);
            Packet.Fill(6, 0);//unk
            Packet.WriteString(MessageCreater.Name, 16);
            Packet.WriteString(Message, 512);//details message
        }
        public void SendMemberList(GameClient Client)
        {
            lock (ThreadLocker)
            {
                for (int i = 0; i < Members.Count; i += 20)
                {
                    using (var packet = GetMemberListPacket(i, i + Math.Min(20, Members.Count - i)))
                    {
                        Client.Send(packet);
                    }
                }
            }
        }
        private GamePacket GetMemberListPacket(int Start, int End)
        {
            var left = (Members.Count - End);


            var packet = new GamePacket(GameOpCode.Server.H29.GuildMemberList);

            packet.WriteUInt16((ushort)Members.Count);
            packet.WriteUInt16((ushort)left);
            packet.WriteUInt16((ushort)End);
            for (int i = Start; i < End; i++)
            {
                Members[i].WriteInfo(packet);
            }

            return packet;
        }


        public bool GetMember(string Name, out GuildMember Member)
        {
            lock (ThreadLocker)
            {
                Member = Members.Find(m => m.Character.Name.Equals(Name));
            }

            return (Member != null);
        }
        public void AddMember(Character Character, GuildRank Rank, SqlConnection con = null, bool BroadcastAdd = true, bool SendGuildInfoToClient = true)
        {
            lock (ThreadLocker)
            {
                var conCreated = (con == null);
                if (conCreated)
                {
                    con = DatabaseManager.DB_Game.GetConnection();
                }

                //add to db
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.GuildMember_Create";

                    cmd.Parameters.Add(new SqlParameter("@pGuildID", ID));
                    cmd.Parameters.Add(new SqlParameter("@pCharacterID", Character.ID));
                    cmd.Parameters.Add(new SqlParameter("@pRank", (byte)Rank));
                    cmd.Parameters.Add(new SqlParameter("@pCorp", Convert.ToInt16("0")));



                    cmd.ExecuteNonQuery();
                }

                //create object
                var newMember = new GuildMember(this, Character, Rank, 0);

                //update character
                Character.Guild = this;
                Character.GuildMember = newMember;
                Character.GuildAcademy = Academy;

                //add to list
                Members.Add(newMember);


                if (BroadcastAdd)
                {
                    newMember.BroadcastGuildName();

                    //broadcast that guild member joined
                    using (var packet = new GamePacket(GameOpCode.Server.H29.GuildMemberJoined))
                    {
                        newMember.WriteInfo(packet);


                        Broadcast(packet, newMember);
                    }
                    using (var packet = new GamePacket(GameOpCode.Server.H29.GuildMemberLoggedIn))
                    {
                        packet.WriteString(newMember.Character.Name, 16);


                        Broadcast(packet, newMember);
                    }


                    //let zone know that a new member has been added to guild
                    using (var packet = new InterPacket(InterOpCode.World.H53.ZONE_GuildMemberAdd))
                    {
                        packet.WriteInt32(ID);
                        packet.WriteInt32(Character.ID);
                        packet.WriteByte((byte)newMember.Rank);
                        packet.WriteUInt16(newMember.Corp);



                        ZoneManager.Broadcast(packet);
                    }
                }

                //send guild info to new member
                if (SendGuildInfoToClient)
                {
                    SendMemberList(newMember.Character.Client);

                    using (var packet = new GamePacket(GameOpCode.Server.H4.CharacterGuildinfo))
                    {
                        WriteGuildInfo(packet);


                        newMember.Character.Client.Send(packet);
                    }
                }



                if (conCreated)
                {
                    con.Dispose();
                }
            }
        }
        public void RemoveMember(GuildMember Member, SqlConnection con = null, bool BroadcastRemove = true)
        {
            lock (ThreadLocker)
            {
                var conCreated = (con == null);
                if (conCreated)
                {
                    con = DatabaseManager.DB_Game.GetConnection();
                }


                //remove from db
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "dbo.GuildMember_Remove";

                    cmd.Parameters.Add(new SqlParameter("@pGuildID", ID));
                    cmd.Parameters.Add(new SqlParameter("@pCharacterID", Member.Character.ID));


                    
                    cmd.ExecuteNonQuery();
                }


                //remove from list
                Members.Remove(Member);

                //update character
                Member.Character.Guild = null;
                Member.Character.GuildMember = null;
                Member.Character.GuildAcademy = null;


                //broadcast member left packet
                if (BroadcastRemove)
                {
                    using (var packet = new GamePacket(GameOpCode.Server.H29.GuildMemberLeft))
                    {
                        packet.WriteString(Member.Character.Name);



                        Broadcast(packet);
                    }

                    //send packet to zones that a member has been removed
                    using (var packet = new InterPacket(InterOpCode.World.H53.ZONE_GuildMemberRemove))
                    {
                        packet.WriteInt32(ID);
                        packet.WriteInt32(Member.Character.ID);


                        ZoneManager.Broadcast(packet);
                    }
                }


                //clean up
                Member.Dispose();



                if (conCreated)
                {
                    con.Dispose();
                }
            }
        }
        public void UpdateMemberRank(GuildMember Member, GuildRank NewRank)
        {
            Member.Rank = NewRank;
            Member.Save();


            //broadcast to members
            using (var packet = new GamePacket(GameOpCode.Server.H29.UpdateGuildMemberRank))
            {
                packet.WriteString(Member.Character.Name, 16);
                packet.WriteByte((byte)NewRank);


                Broadcast(packet);
            }


            //broadcast to zones
            using (var packet = new InterPacket(InterOpCode.World.H53.ZONE_GuildMemberRankUpdate))
            {
                packet.WriteInt32(ID);
                packet.WriteInt32(Member.Character.ID);
                packet.WriteByte((byte)NewRank);


                ZoneManager.Broadcast(packet);
            }
        }
    }
}