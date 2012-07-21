using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Fiesta.Core.Networking;
using Fiesta.World.Game.Characters;
using Fiesta.World.Game.Zones;
using Fiesta.World.Networking;
using Fiesta.World.Networking.Helpers;

namespace Fiesta.World.Game.Guilds.Academy
{
    public sealed class GuildAcademy
    {
        public Guild Guild { get; private set; }

        public string Message { get; set; }
        public ushort Points { get; set; }

        public List<GuildAcademyMember> Members { get; private set; }
        public const ushort MaxMembers = 60; // Yes, its up to the server. Max is: 65535


        public TimeSpan GuildBuffKeepTime { get; set; }







        public GuildAcademy(Guild Guild, SqlConnection con)
        {
            this.Guild = Guild;

            Members = new List<GuildAcademyMember>();

            Load(con);
        }
        public void Dispose()
        {
            Guild = null;

            Message = null;

            Members.ForEach(m => m.Dispose());
            Members.Clear();
            Members = null;
        }

        private void Load(SqlConnection con)
        {
            //load academy info
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM GuildAcademy WHERE GuildID = @pGuildID";

                cmd.Parameters.Add(new SqlParameter("@pGuildID", Guild.ID));


                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        throw new InvalidOperationException("Error getting guild academy info from database for guild: " + Guild.Name);

                    Message = reader.GetString(1);
                    Points = (ushort)reader.GetInt16(2);
                }
            }



            //members
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM GuildAcademyMembers WHERE GuildID = @pGuildID";

                cmd.Parameters.Add(new SqlParameter("@pGuildID", Guild.ID));


                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Character character;
                        if (!CharacterManager.GetCharacterByID(reader.GetInt32(1), out character, con))
                            continue; // maybe deleted

                        var member = new GuildAcademyMember(this, character, reader);

                        Members.Add(member);
                    }
                }
            }
        }
        public void Save(SqlConnection con = null)
        {
            lock (Guild.ThreadLocker)
            {
                var conCreated = (con == null);
                if (conCreated)
                {
                    con = DatabaseManager.DB_Game.GetConnection();
                }



                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.GuildAcademy_Save";


                    cmd.Parameters.Add(new SqlParameter("@pGuildID", Guild.ID));
                    cmd.Parameters.Add(new SqlParameter("@pMessage", Message));
                    cmd.Parameters.Add(new SqlParameter("@pPoints", (short)Points));


                    cmd.ExecuteNonQuery();
                }


                foreach (var member in Members)
                {
                    member.Save(con);
                }




                if (conCreated)
                {
                    con.Dispose();
                }
            }
        }






        public void Broadcast(GamePacket Packet, GuildAcademyMember Exclude = null)
        {
            lock (Guild.ThreadLocker)
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
        public void BroadcastInfo()
        {
            using (var packet = new GamePacket(GameOpCode.Server.H4.CharacterGuildacademyinfo))
            {
                WriteInfo(packet);


                Broadcast(packet);
            }
        }
        public void WriteInfo(GamePacket Packet)
        {
            Packet.WriteInt32(Guild.ID);
            Packet.WriteByte(1);//unk
            Packet.WriteString(Guild.Master.Character.Name, 16);
            Packet.WriteUInt16((ushort)Members.Count);//membercount
            Packet.WriteUInt16(MaxMembers);//maxmembercount
            Packet.WriteInt32(Guild.ID);//academyid
            Packet.WriteInt32((int)Guild.CreateTime.DayOfWeek);//weeks //Todo Calculate Weeks
            Packet.WriteInt32((int)GuildBuffKeepTime.TotalSeconds);  //time in sek (buff?)
            Packet.Fill(128, 1);//GuildAcademyBUff
            Packet.WriteString(Message, 512);
        }
        public void SendMemberList(GameClient Client)
        {
            for (int i = 0; i < Members.Count; i += 20)
            {
                using (var packet = GetMemberListPacket(i, (i + Math.Min(20, Members.Count - i))))
                {
                    Client.Send(packet);
                }
            }
        }
        private GamePacket GetMemberListPacket(int Start, int End)
        {
            var packet = new GamePacket(GameOpCode.Server.H38.SendAcademyMemberList);

            packet.WriteUInt16((ushort)Members.Count);
            packet.WriteUInt16((ushort)(Members.Count - End));
            packet.WriteUInt16((ushort)End);

            for (int i = Start; i < End; i++)
            {
                Members[i].WriteInfo(packet);
            }

            return packet;
        }










        public void AddMember(Character Character, GuildAcademyRank Rank)
        {
            if (Character.Level < 10
                || Character.Level > 60)
                return;


            if (Character.IsInGuild
                || Character.IsInGuildAcademy)
            {
                SH38Helpers.SendAcademyResponse(Character.Client, Guild.Name, GuildAcademyResponse.AlreadyInAcademy);
                return;
            }

            lock (Guild.ThreadLocker)
            {
                if (Members.Count >= MaxMembers)
                {
                    SH38Helpers.SendAcademyResponse(Character.Client, Guild.Name, GuildAcademyResponse.AcademyFull);
                    return;
                }


                var registerDate = WorldService.Instance.Time;

                //add to sql
                using (var con = DatabaseManager.DB_Game.GetConnection())
                {
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "dbo.GuildAcademyMember_Create";

                        cmd.Parameters.Add(new SqlParameter("@pGuildID", Guild.ID));
                        cmd.Parameters.Add(new SqlParameter("@pCharacterID", Character.ID));
                        cmd.Parameters.Add(new SqlParameter("@pRegisterDate", registerDate));
                        cmd.Parameters.Add(new SqlParameter("@pRank", (byte)Rank));



                        switch ((int)cmd.ExecuteScalar())
                        {
                            case 0:
                                
                                var member = new GuildAcademyMember(this, Character, registerDate, Rank);

                                //Add to list
                                Members.Add(member);

                                //Update character
                                Character.Guild = Guild;
                                Character.GuildAcademy = this;
                                Character.GuildAcademyMember = member;


                                //send packets to client
                                SH38Helpers.SendAcademyResponse(Character.Client, Guild.Name, GuildAcademyResponse.JoinSuccess);
                                using (var packet = new GamePacket(GameOpCode.Server.H4.CharacterGuildacademyinfo))
                                {
                                    WriteInfo(packet);

                                    Character.Client.Send(packet);
                                }

                                member.BroadcastGuildName();
                                using (var packet = new GamePacket(GameOpCode.Server.H38.AcademyMemberJoined))
                                {
                                    member.WriteInfo(packet);

                                    Broadcast(packet);
                                    Guild.Broadcast(packet);
                                }


                                //send packet to zones
                                using (var packet = new InterPacket(InterOpCode.World.H55.ZONE_AcademyMemberJoined))
                                {
                                    packet.WriteInt32(Guild.ID);
                                    packet.WriteInt32(Character.ID);
                                    packet.WriteDateTime(registerDate);


                                    
                                    ZoneManager.Broadcast(packet);
                                }


                                break;

                            case -1:
                                SH38Helpers.SendAcademyResponse(Character.Client, Guild.Name, GuildAcademyResponse.AlreadyInAcademy);
                                return;
                            case -2:
                            default:
                                SH38Helpers.SendAcademyResponse(Character.Client, Guild.Name, GuildAcademyResponse.DatabaseError);
                                return;
                        }
                    }
                }
            }
        }
        public void RemoveMember(GuildAcademyMember Member)
        {
            lock (Guild.ThreadLocker)
            {
                //remove from db
                using (var con = DatabaseManager.DB_Game.GetConnection())
                {
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "dbo.GuildAcademyMember_Remove";

                        cmd.Parameters.Add(new SqlParameter("@pGuildID", Guild.ID));
                        cmd.Parameters.Add(new SqlParameter("@pCharacterID", Member.Character.ID));


                        cmd.ExecuteNonQuery();
                    }
                }




                //remove from list
                Members.Remove(Member);

                //clean character
                Member.Character.Guild = null;
                Member.Character.GuildAcademy = null;
                Member.Character.GuildAcademyMember = null;



                //send packets
                using (var packet = new GamePacket(GameOpCode.Server.H38.LeaveAcademyResponse))
                {
                    packet.WriteUInt16((ushort)GuildAcademyResponse.LeaveSuccess);

                    
                    Member.Character.Client.Send(packet);
                }
                using (var packet = new GamePacket(GameOpCode.Server.H38.AcademyMemberLeft))
                {
                    packet.WriteString(Member.Character.Name, 16);


                    Broadcast(packet);
                    Guild.Broadcast(packet);
                }

                //send packet to zones
                using (var packet = new InterPacket(InterOpCode.World.H55.ZONE_AcademyMemberLeft))
                {
                    packet.WriteInt32(Guild.ID);
                    packet.WriteInt32(Member.Character.ID);



                    ZoneManager.Broadcast(packet);
                }


                //clean up
                Member.Dispose();
            }
        }
    }
}