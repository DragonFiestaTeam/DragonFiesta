using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Fiesta.Core.Cryptography;
using Fiesta.Core.Networking;
using Fiesta.World.Game.Characters;
using Fiesta.World.Game.Guilds.Academy;
using Fiesta.World.Game.Zones;
using Fiesta.World.Networking;

namespace Fiesta.World.Game.Guilds
{
    public static class GuildManager
    {
        public static object ThreadLocker { get; private set; }




        private static List<Guild> LoadedGuilds;


        [ServiceStartMethod(ServiceStartStep.Logic, 10)]
        public static void OnAppStart()
        {
            ThreadLocker = new object();

            LoadedGuilds = new List<Guild>();


            CharacterManager.OnCharacterLogin += On_CharacterManager_CharacterLogin;
            CharacterManager.OnCharacterLogout += On_CharacterManager_CharacterLogout;
        }
        private static void On_CharacterManager_CharacterLogin(Character Character)
        {
            if (Character.IsInGuild)
            {
                var guild = Character.Guild;

                //send guild info to client
                using (var packet = new GamePacket(GameOpCode.Server.H4.CharacterGuildinfo))
                {
                    guild.WriteGuildInfo(packet);


                    Character.Client.Send(packet);
                }


                //send member list to client
                guild.SendMemberList(Character.Client);



                GuildMember member;
                if (guild.GetMember(Character.Name, out member))
                {
                    //send guild member logged in to other guild members
                    using (var packet = new GamePacket(GameOpCode.Server.H29.GuildMemberLoggedIn))
                    {
                        packet.WriteString(Character.Name, 16);


                        Character.Guild.Broadcast(packet, member);
                    }
                }


                //send packet to zone that guild member logged in
                using (var packet = new InterPacket(InterOpCode.World.H53.ZONE_GuildMemberLogin))
                {
                    packet.WriteInt32(guild.ID);
                    packet.WriteInt32(Character.ID);


                    ZoneManager.Broadcast(packet);
                }
            }
            else
            {
                using (var packet = new GamePacket(GameOpCode.Server.H4.CharacterGuildinfo))
                {
                    packet.WriteInt32(0);


                    Character.Client.Send(packet);
                }
            }



            //academy
            var academy = Character.GuildAcademy;
            if (academy != null)
            {
                if (Character.IsInGuildAcademy)
                {
                    using (var packet = new GamePacket(GameOpCode.Server.H4.CharacterGuildacademyinfo))
                    {
                        academy.WriteInfo(packet);


                        Character.Client.Send(packet);
                    }

                    
                    academy.SendMemberList(Character.Client);
                }
                else
                {
                    using (var packet = new GamePacket(GameOpCode.Server.H4.CharacterGuildacademyinfo))
                    {
                        packet.Fill(5, 0);


                        Character.Client.Send(packet);
                    }
                }
            }
            else
            {
                using (var packet = new GamePacket(GameOpCode.Server.H4.CharacterGuildacademyinfo))
                {
                    packet.Fill(5, 0);


                    Character.Client.Send(packet);
                }
            }
        }
        private static void On_CharacterManager_CharacterLogout(Character Character)
        {
            GuildMember member;
            if (Character.Guild != null
                && Character.Guild.GetMember(Character.Name, out member))
            {
                //send guild member logged out to other guild members
                using (var packet = new GamePacket(GameOpCode.Server.H29.GuildMemberLoggedOut))
                {
                    packet.WriteString(Character.Name, 16);


                    Character.Guild.Broadcast(packet, member);
                }


                //send packet to zone that guild member logged out
                using (var packet = new InterPacket(InterOpCode.World.H53.ZONE_GuildMemberLogout))
                {
                    packet.WriteInt32(Character.Guild.ID);
                    packet.WriteInt32(Character.ID);


                    ZoneManager.Broadcast(packet);
                }
            }
        }





        public static bool GetGuildByID(int ID, out Guild Guild)
        {
            lock (ThreadLocker)
            {
                Guild = LoadedGuilds.Find(g => g.ID.Equals(ID));


                if (Guild == null)
                {
                    //try to load from db
                    using (var con = DatabaseManager.DB_Game.GetConnection())
                    {
                        using (var cmd = con.CreateCommand())
                        {
                            cmd.CommandText = "SELECT * FROM Guilds WHERE ID = @pID";

                            cmd.Parameters.Add(new SqlParameter("@pID", ID));


                            using (var reader = cmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                    return false;

                                //create new guild
                                Guild = new Guild(con, reader);

                                //add to list
                                LoadedGuilds.Add(Guild);
                            }
                        }
                    }
                }
            }

            return (Guild != null);
        }
        public static bool GetGuildByName(string Name, out Guild Guild)
        {
            lock (ThreadLocker)
            {
                Guild = LoadedGuilds.Find(g => g.Name.Equals(Name));


                if (Guild == null)
                {
                    //try to load from db
                    using (var con = DatabaseManager.DB_Game.GetConnection())
                    {
                        using (var cmd = con.CreateCommand())
                        {
                            cmd.CommandText = "SELECT * FROM Guilds WHERE Name = @pName";

                            cmd.Parameters.Add(new SqlParameter("@pName", Name));


                            using (var reader = cmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                    return false;

                                //create new guild
                                Guild = new Guild(con, reader);

                                //add to list
                                LoadedGuilds.Add(Guild);
                            }
                        }
                    }
                }
            }

            return (Guild != null);
        }










        #region Game Client Handlers

        
        [GamePacketHandler(GameOpCode.Client.H29.GetGuildList)]
        public static void On_GameClient_GetGuildList(GameClient Client, FiestaPacket Packet)
        {
            if (Client.Character == null)
            {
                Client.Dispose();
                return;
            }


            var now = WorldService.Instance.Time;
            if (now.Subtract(Client.Character.LastGuildListRefresh).TotalSeconds >= 60)
            {
                Client.Character.LastGuildListRefresh = now;



                const int GuildsPerPacket = 100;
                lock (ThreadLocker)
                {
                    using (var con = DatabaseManager.DB_Game.GetConnection())
                    {
                        //get guild count
                        int guildCount;
                        using (var cmd = con.CreateCommand())
                        {
                            cmd.CommandText = "SELECT COUNT(*) FROM Guilds";


                            guildCount = (int)cmd.ExecuteScalar();
                        }


                        using (var cmd = con.CreateCommand())
                        {
                            cmd.CommandText = "SELECT ID FROM Guilds";


                            GamePacket listPacket = null;
                            var count = 0;
                            var globalCount = 0;

                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (listPacket == null)
                                    {
                                        listPacket = new GamePacket(GameOpCode.Server.H29.SendGuildList);
                                        listPacket.WriteUInt16(3137);
                                        listPacket.WriteByte(1);
                                        listPacket.WriteUInt16((ushort)guildCount);
                                        listPacket.WriteUInt16((ushort)Math.Min(GuildsPerPacket, guildCount - globalCount));
                                    }



                                    Guild guild;
                                    if (GuildManager.GetGuildByID(reader.GetInt32(0), out guild))
                                    {
                                        //write packet
                                        listPacket.WriteInt32(guild.ID);
                                        listPacket.WriteString(guild.Name, 16);
                                        listPacket.WriteString(guild.Master.Character.Name, 16);
                                        listPacket.WriteBool(guild.AllowGuildWar);
                                        listPacket.WriteByte(1);     // unk
                                        listPacket.WriteUInt16((ushort)guild.Members.Count);
                                        listPacket.WriteUInt16(100); // unk
                                    }
                                    else
                                    {
                                        Packet.Fill(42, 0); // guild get error
                                    }



                                    globalCount++;
                                    count++;
                                    if (count >= Math.Min(GuildsPerPacket, guildCount - globalCount))
                                    {
                                        //send packet
                                        Client.Send(listPacket);

                                        listPacket.Dispose();
                                        listPacket = null;


                                        //reset
                                        count = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [GamePacketHandler(GameOpCode.Client.H29.CreateGuild)]
        public static void On_GameClient_CreateGuild(GameClient Client, FiestaPacket Packet)
        {
            string name, password;
            bool allowGuildWar;
            if (!Packet.ReadString(out name, 16)
                || !Packet.ReadString(out password, 8)
                || !Packet.ReadSkip(4) // unk ?
                || !Packet.ReadBool(out allowGuildWar))
            {
                Client.Dispose();
                return;
            }


            GuildCreateResponse response;

            if (Client.Character.Level < 20)
            {
                response = GuildCreateResponse.LevelTooLow;
            }
            else if (Client.Character.Money < Guild.Price)
            {
                response = GuildCreateResponse.MoneyTooLow;
            }
            else
            {
                //encrypt guild pw
                var pwData = Encoding.UTF8.GetBytes(password);
                InterCrypto.Encrypt(ref pwData, 0, pwData.Length);


                Guild guild;

                //try to create guild
                lock (ThreadLocker)
                {
                    int result;
                    int guildID;
                    var createTime = WorldService.Instance.Time;

                    using (var con = DatabaseManager.DB_Game.GetConnection())
                    {
                        //insert guild in db
                        using (var cmd = con.CreateCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "dbo.Guild_Create";

                            cmd.Parameters.Add(new SqlParameter("@pName", name));
                            cmd.Parameters.Add(new SqlParameter("@pPassword", pwData));
                            cmd.Parameters.Add(new SqlParameter("@pAllowGuildWar", allowGuildWar));
                            cmd.Parameters.Add(new SqlParameter("@pCreaterID", Client.Character.ID));
                            cmd.Parameters.Add(new SqlParameter("@pCreateTime", createTime));

                            var idParam = cmd.Parameters.Add(new SqlParameter("@pID", SqlDbType.Int)
                                {
                                    Direction = ParameterDirection.Output
                                });



                            result = (int)cmd.ExecuteScalar();
                            guildID = (int)idParam.Value;
                        }

                        switch (result)
                        {
                            case -1:
                                //guild name already exists (ToDo: get response code)
                                WorldManager.SendYellowNotice(Client, "Guild name already exists (tmp).");
                                return;

                            case -2: //database error @ insert guild (ToDo: get response code)
                            case -3: //database error @ insert guild academy (ToDo: get response code)
                                WorldManager.SendYellowNotice(Client, "Database error (tmp).");
                                return;

                            case 0:

                                //create guild
                                guild = new Guild(con, guildID, name, pwData, allowGuildWar, Client.Character, createTime);

                                //insert guild master (character will get updated)
                                guild.AddMember(Client.Character, GuildRank.Master, con, false, false);

                                
                                //add to loaded guilds
                                LoadedGuilds.Add(guild);

                                break;


                            default:
                                return;
                        }
                    }
                }



                //revoke money
                Client.Character.SetMoney(Client.Character.Money - Guild.Price, true);

                //let character broadcast guild name packet
                using (var packet = new GamePacket(GameOpCode.Server.H29.GuildNameResult))
                {
                    packet.WriteInt32(guild.ID);
                    packet.WriteString(Client.Character.Name, 16);


                    BroadcastManager.BroadcastInRange(Client.Character, packet, true);
                }

                //let zone know that a guild has been loaded
                using (var packet = new InterPacket(InterOpCode.World.H53.ZONE_GuildCreated))
                {
                    packet.WriteInt32(guild.ID);
                    packet.WriteInt32(Client.Character.ID);


                    ZoneManager.Broadcast(packet);
                }

                
                //set response to success
                response = GuildCreateResponse.Success;
            }

            SendGuildCreateResponse(Client, name, password, allowGuildWar, response);
        }
        private static void SendGuildCreateResponse(GameClient Client, string Name, string Password, bool AllowGuildWar, GuildCreateResponse Response)
        {
            using (var packet = new GamePacket(GameOpCode.Server.H29.CreateGuildResponse))
            {

                packet.WriteUInt16((ushort)Response);
                packet.WriteInt32((Response == GuildCreateResponse.Success ? 32 : 0));

                packet.WriteString(Name, 16);
                packet.WriteString(Password, 8);
                packet.WriteBool(AllowGuildWar);


                
                Client.Send(packet);
            }
        }


        [GamePacketHandler(GameOpCode.Client.H29.GuildNameRequest)]
        public static void On_GameClient_GuildNameRequest(GameClient Client, FiestaPacket Packet)
        {
            int guildID;
            if (!Packet.ReadInt32(out guildID))
            {
                Client.Dispose();
                return;
            }


            Guild guild;
            if (GetGuildByID(guildID, out guild))
            {
                using (var packet = new GamePacket(GameOpCode.Server.H29.GuildNameResult))
                {
                    packet.WriteInt32(guildID);
                    packet.WriteString(guild.Name, 16);



                    Client.Send(packet);
                }
            }
        }


        [GamePacketHandler(GameOpCode.Client.H29.GuildMemberListRequest)]
        public static void On_GameClient_GuildMemberListRequest(GameClient Client, FiestaPacket Packet)
        {
            if (Client.Character == null)
            {
                Client.Dispose();
                return;
            }


            if (Client.Character.Guild != null)
            {
                Client.Character.Guild.SendMemberList(Client);
            }
        }

        [GamePacketHandler(GameOpCode.Client.H29.UpdateGuildMessage)]
        public static void On_GameClient_UpdateGuildMessage(GameClient Client, FiestaPacket Packet)
        {
            ushort length;
            string message;
            if (Client.Character.Guild == null
                || !Packet.ReadUInt16(out length)
                || !Packet.ReadString(out message, length))
            {
                Client.Dispose();
                return;
            }

            //response packets
            using (var packet = new GamePacket(GameOpCode.Server.H29.UnkMessageChange))
            {
                packet.WriteHexAsBytes("68 1B 00 92 AD F8 4F 2E 00 00 00 2B 00 00 00 17 00 00 00 07 00 00 00 06 00 00 00 70 00 00 00 06 00 00 00 BC 00 00 00 01 00 00 00 00 00");

                Client.Send(packet);
            }
            using (var packet = new GamePacket(GameOpCode.Server.H29.ClearGuildDetailsMessage))
            {
                packet.WriteUInt16(3137);
                packet.WriteUInt64(0);


                Client.Send(packet);
            }
            using (var packet = new GamePacket(GameOpCode.Server.H29.UpdateGuildMessageResponse))
            {
                packet.WriteUInt16(3137);


                Client.Send(packet);
            }



            //update guild
            lock (Client.Character.Guild.ThreadLocker)
            {
                Client.Character.Guild.Message = message;
                Client.Character.Guild.MessageCreater = Client.Character;
                Client.Character.Guild.MessageCreateTime = WorldService.Instance.Time;

                Client.Character.Guild.Save();



                //broadcast packet to all guild members
                using (var packet = new GamePacket(GameOpCode.Server.H29.SendUpdateGuildDetails))
                {
                    packet.Fill(4, 0x00);
                    packet.WriteInt32(Client.Character.Guild.MessageCreateTime.Second);
                    packet.WriteInt32(Client.Character.Guild.MessageCreateTime.Minute);
                    packet.WriteInt32(Client.Character.Guild.MessageCreateTime.Hour);
                    packet.WriteInt32(Client.Character.Guild.MessageCreateTime.Day);
                    packet.WriteInt32(Client.Character.Guild.MessageCreateTime.Month - 1);
                    packet.WriteInt32(Client.Character.Guild.MessageCreateTime.Year - 1900);
                    packet.WriteInt32(0);
                    packet.WriteUInt64(0);
                    packet.WriteString(Client.Character.Name, 16);
                    packet.WriteUInt16(length);
                    packet.WriteString(message, length);


                    
                    Client.Character.Guild.Broadcast(packet);
                }


                //send packet to zone that guild message changed
                using (var packet = new InterPacket(InterOpCode.World.H53.ZONE_GuildMessageUpdate))
                {
                    packet.WriteInt32(Client.Character.Guild.ID);
                    packet.WriteInt32(Client.Character.ID);
                    packet.WriteDateTime(Client.Character.Guild.MessageCreateTime);

                    packet.WriteUInt16(length);
                    packet.WriteString(message, length);



                    ZoneManager.Broadcast(packet);
                }
            }
        }

        [GamePacketHandler(GameOpCode.Client.H29.LeaveGuild)]
        public static void On_GameClient_LeaveGuild(GameClient Client, FiestaPacket Packet)
        {
            if (Client.Character.Guild == null)
            {
                return;
            }


            GuildMember member;
            if (Client.Character.Guild.GetMember(Client.Character.Name, out member))
            {
                Client.Character.Guild.RemoveMember(member, null, true);

                using (var packet = new GamePacket(GameOpCode.Server.H29.LeaveGuildResponse))
                {
                    packet.WriteUInt16(3137);

                    Client.Send(packet);
                }
            }
        }

        [GamePacketHandler(GameOpCode.Client.H29.GuildInviteRequest)]
        public static void On_GameClient_GuildInviteRequest(GameClient Client, FiestaPacket Packet)
        {
            string targetName;
            if (Client.Character == null
                || Client.Character.Guild == null // cheating ?
                || !Packet.ReadString(out targetName, 16))
            {
                Client.Dispose();
                return;
            }


            //get target
            Character target;
            if (!CharacterManager.GetLoggedInCharacter(targetName, out target)
                || !target.IsOnline)
            {
                return;
            }


            //todo: check for academy, too
            if (target.Guild != null)
            {
                SendGuildInviteError(Client, targetName, GuildInviteError.TargetHasAlreadyGuild);
                return;
            }


            //send invite to target
            using (var packet = new GamePacket(GameOpCode.Server.H29.GuildInviteRequest))
            {
                packet.WriteString(Client.Character.Guild.Name, 16);
                packet.WriteString(Client.Character.Name, 16);


                
                target.Client.Send(packet);
            }
        }
        private static void SendGuildInviteError(GameClient Client, string TargetName, GuildInviteError Error)
        {
            using (var packet = new GamePacket(GameOpCode.Server.H29.GuildInviteError))
            {
                packet.WriteString(TargetName, 16);
                packet.WriteUInt16((ushort)Error);


                Client.Send(packet);
            }
        }

        [GamePacketHandler(GameOpCode.Client.H29.GuildInviteResponse)]
        public static void On_GameClient_GuildInviteResponse(GameClient Client, FiestaPacket Packet)
        {
            string guildName;
            bool joinGuild;
            if (!Packet.ReadString(out guildName, 16)
                || !Packet.ReadBool(out joinGuild))
            {
                Client.Dispose();
                return;
            }



            //get guild
            Guild guild;
            if (GetGuildByName(guildName, out guild))
            {
                guild.AddMember(Client.Character, GuildRank.Member, null, true, true);
            }
        }

        [GamePacketHandler(GameOpCode.Client.H29.GuildChat)]
        public static void On_GameClient_GuildChat(GameClient Client, FiestaPacket Packet)
        {
            byte len;
            string msg;
            if (Client.Character == null
                || !Packet.ReadByte(out len)
                || !Packet.ReadString(out msg, len))
            {
                Client.Dispose();
                return;
            }

            len = (byte)(len + 2);


            if (Client.Character.Guild != null)
            {
                using (var packet = new GamePacket(GameOpCode.Server.H29.GuildChat))
                {
                    packet.WriteInt32(Client.Character.Guild.ID);
                    packet.WriteString(Client.Character.Name, 16);
                    
                    packet.WriteByte(len);
                    packet.WriteString(msg, len);


                    Client.Character.Guild.Broadcast(packet);
                }
            }
        }

        [GamePacketHandler(GameOpCode.Client.H29.UpdateGuildMemberRank)]
        public static void On_GameClient_UpdateGuildMemberRank(GameClient Client, FiestaPacket Packet)
        {
            string targetName;
            byte newRankByte;
            if (!Packet.ReadString(out targetName, 16)
                || !Packet.ReadByte(out newRankByte))
            {
                Client.Dispose();
                return;
            }


            var newRank = (GuildRank)newRankByte;
            GuildMember member;
            GuildMember target;
            if (Client.Character.Guild != null
                && Client.Character.Guild.GetMember(Client.Character.Name, out member)
                && Client.Character.Guild.GetMember(targetName, out target))
            {
                switch (member.Rank)
                {
                    case GuildRank.Master:

                        if (newRank == GuildRank.Master)
                        {
                            Client.Character.Guild.UpdateMemberRank(member, GuildRank.Member);
                        }

                        Client.Character.Guild.UpdateMemberRank(target, newRank);

                        using (var packet = new GamePacket(GameOpCode.Server.H29.UpdateGuildMemberRankResponse))
                        {
                            packet.WriteString(targetName, 16);
                            packet.WriteByte(newRankByte);
                            packet.WriteUInt16(3137); // ok response


                            Client.Send(packet);
                        }

                        break;

                    case GuildRank.Admin:
                    case GuildRank.Advice:
                    case GuildRank.Commander:
                    case GuildRank.Default:
                    case GuildRank.Guard:
                    case GuildRank.Member:
                        return;
                }
            }
        }

        #endregion
    }
}