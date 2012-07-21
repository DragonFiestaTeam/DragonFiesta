/*File for this file Basic Copyright 2012 no0dl */
using System;
using System.Data;
using Zepheus.World.Networking;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.InterLib.Networking;
using Zepheus.InterLib;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Zepheus.World.Data;
using Zepheus.World.Managers;

namespace Zepheus.World.Data.Guilds.Academy
{
    public sealed class GuildAcademy
    {
        public Guild Guild { get; private set; }

        public string Message { get; set; }
        public ushort Points { get; set; }

        public List<GuildAcademyMember> Members { get; private set; }
        public const ushort MaxMembers = 60; // Yes, its up to the server. Max is: 65535


        public TimeSpan GuildBuffKeepTime { get; set; }







        public GuildAcademy(Guild Guild, MySqlConnection con)
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

        private void Load(MySqlConnection con)
        {
            //load academy info
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM GuildAcademy WHERE GuildID = @pGuildID";

                cmd.Parameters.Add(new MySqlParameter("@pGuildID", Guild.ID));


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

                cmd.Parameters.Add(new MySqlParameter("@pGuildID", Guild.ID));


                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                        WorldCharacter character;
                        if (!CharacterManager.Instance.GetCharacterByID(reader.GetInt32(1), out character, con))
                            continue; // maybe deleted

                        var member = new GuildAcademyMember(this, character, reader);

                        Members.Add(member);
                    }
                }
            }
        }
        public void Save(MySqlConnection con = null)
        {
            lock (Guild.ThreadLocker)
            {
                var conCreated = (con == null);
                if (conCreated)
                {
                    con = Program.DatabaseManager.GetClient().GetConnection();
                }



                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.GuildAcademy_Save";


                    cmd.Parameters.Add(new MySqlParameter("@pGuildID", Guild.ID));
                    cmd.Parameters.Add(new MySqlParameter("@pMessage", Message));
                    cmd.Parameters.Add(new MySqlParameter("@pPoints", (short)Points));


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






        public void Broadcast(Packet Packet, GuildAcademyMember Exclude = null)
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
                            member.Character.Client.SendPacket(Packet);
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
            using (var packet = new Packet(SH4Type.CharacterGuildacademyinfo))
            {
                WriteInfo(packet);


                Broadcast(packet);
            }
        }
        public void WriteInfo(Packet Packet)
        {
            Packet.WriteInt(Guild.ID);
            Packet.WriteByte(1);//unk
            Packet.WriteString(Guild.Master.Character.Character.Name, 16);
            Packet.WriteUShort((ushort)Members.Count);//membercount
            Packet.WriteUShort(MaxMembers);//maxmembercount
            Packet.WriteInt(Guild.ID);//academyid
            Packet.WriteInt((int)Guild.CreateTime.DayOfWeek);//weeks //Todo Calculate Weeks
            Packet.WriteInt((int)GuildBuffKeepTime.TotalSeconds);  //time in sek (buff?)
            Packet.Fill(128, 1);//GuildAcademyBUff
            Packet.WriteString(Message, 512);
        }
        public void SendMemberList(WorldClient Client)
        {
            for (int i = 0; i < Members.Count; i += 20)
            {
                using (var packet = GetMemberListPacket(i, (i + Math.Min(20, Members.Count - i))))
                {
                    Client.SendPacket(packet);
                }
            }
        }
        private Packet GetMemberListPacket(int Start, int End)
        {
            var packet = new Packet(SH38Type.GuildAcademyList);

            packet.WriteUShort((ushort)Members.Count);
            packet.WriteUShort((ushort)(Members.Count - End));
            packet.WriteUShort((ushort)End);

            for (int i = Start; i < End; i++)
            {
                Members[i].WriteInfo(packet);
            }

            return packet;
        }










        public void AddMember(WorldCharacter Character, GuildAcademyRank Rank)
        {
            if (Character.Character.CharLevel < 10
                || Character.Character.CharLevel > 60)
                return;


            if (Character.IsInGuild
                || Character.IsInGuildAcademy)
            {
               Handlers.Handler38.SendAcademyResponse(Character.Client, Guild.Name, GuildAcademyResponse.AlreadyInAcademy);
                return;
            }

            lock (Guild.ThreadLocker)
            {
                if (Members.Count >= MaxMembers)
                {
                    Handlers.Handler38.SendAcademyResponse(Character.Client, Guild.Name, GuildAcademyResponse.AcademyFull);
                    return;
                }


                var registerDate = Program.CurrentTime;

                //add to sql
                using (var con = Program.DatabaseManager.GetClient().GetConnection())
                {
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "dbo.GuildAcademyMember_Create";

                        cmd.Parameters.Add(new MySqlParameter("@pGuildID", Guild.ID));
                        cmd.Parameters.Add(new MySqlParameter("@pCharacterID", Character.ID));
                        cmd.Parameters.Add(new MySqlParameter("@pRegisterDate", registerDate));
                        cmd.Parameters.Add(new MySqlParameter("@pRank", (byte)Rank));



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
                                Handlers.Handler38.SendAcademyResponse(Character.Client, Guild.Name, GuildAcademyResponse.JoinSuccess);
                                using (var packet = new Packet(SH4Type.CharacterGuildacademyinfo))
                                {
                                    WriteInfo(packet);

                                    Character.Client.SendPacket(packet);
                                }

                                member.BroadcastGuildName();
                                using (var packet = new Packet(SH38Type.GuildAcademyJoin))
                                {
                                    member.WriteInfo(packet);

                                    Broadcast(packet);
                                    Guild.Broadcast(packet);
                                }


                                //send packet to zones
                                using (var packet = new InterPacket(InterHeader.ZONE_AcademyMemberJoined))
                                {
                                    packet.WriteInt(Guild.ID);
                                    packet.WriteInt(Character.ID);
                                    packet.WriteDateTime(registerDate);


                                    
                                    ZoneManager.Instance.Broadcast(packet);
                                }


                                break;

                            case -1:
                                Handlers.Handler38.SendAcademyResponse(Character.Client, Guild.Name, GuildAcademyResponse.AlreadyInAcademy);
                                return;
                            case -2:
                            default:
                                Handlers.Handler38.SendAcademyResponse(Character.Client, Guild.Name, GuildAcademyResponse.DatabaseError);
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
                using (var con = Program.DatabaseManager.GetClient().GetConnection())
                {
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "dbo.GuildAcademyMember_Remove";

                        cmd.Parameters.Add(new MySqlParameter("@pGuildID", Guild.ID));
                        cmd.Parameters.Add(new MySqlParameter("@pCharacterID", Member.Character.ID));

                       
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
                using (var packet = new Packet(SH38Type.GuildAcademyLeaveResponse))
                {
                    packet.WriteUShort((ushort)GuildAcademyResponse.LeaveSuccess);

                    
                    Member.Character.Client.SendPacket(packet);
                }
                using (var packet = new Packet(SH38Type.GuildAcademyMemberLeave))
                {
                    packet.WriteString(Member.Character.Character.Name, 16);


                    Broadcast(packet);
                    Guild.Broadcast(packet);
                }

                //send packet to zones
                using (var packet = new InterPacket(InterHeader.ZONE_AcademyMemberLeft))
                {
                    packet.WriteInt(Guild.ID);
                    packet.WriteInt(Member.Character.ID);



                    ZoneManager.Instance.Broadcast(packet);
                }


                //clean up
                Member.Dispose();
            }
        }
    }
}