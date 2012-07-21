using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Fiesta.Core.Networking;
using Fiesta.World.Data.Characters;
using Fiesta.World.Data.Guilds;
using Fiesta.World.Game.Characters;
using Fiesta.World.Game.Buffs;
using Fiesta.World.Game.Zones;
using Fiesta.World.Networking;
using Fiesta.World.Networking.Helpers;

namespace Fiesta.World.Game.Guilds.Academy
{
    public static class GuildAcademyManager
    {
        [ServiceStartMethod(ServiceStartStep.Logic)]
        public static void OnAppStart()
        {
            CharacterManager.OnCharacterLogin += On_CharacterManager_CharacterLogin;
            CharacterManager.OnCharacterLogout += On_CharacterManager_CharacterLogout;
            CharacterManager.OnCharacterLevelUp += On_CharacterManager_CharacterLevelUp;
        }
        private static void On_CharacterManager_CharacterLogin(Character Character)
        {
            if (Character.IsInGuildAcademy)
            {
                using (var packet = new GamePacket(GameOpCode.Server.H38.AcademyMemberLoggedIn))
                {
                    packet.WriteString(Character.Name, 16);

                    
                    Character.Guild.Broadcast(packet);
                    Character.GuildAcademy.Broadcast(packet);
                }

                using (var packet = new InterPacket(InterOpCode.World.H55.ZONE_AcademyMemberOnline))
                {
                    packet.WriteInt32(Character.Guild.ID);
                    packet.WriteInt32(Character.ID);


                    ZoneManager.Broadcast(packet);
                }
            }
        }
        private static void On_CharacterManager_CharacterLogout(Character Character)
        {
            if (Character.IsInGuildAcademy)
            {
                using (var packet = new GamePacket(GameOpCode.Server.H38.AcademyMemberLoggedOut))
                {
                    packet.WriteString(Character.Name, 16);


                    Character.Guild.Broadcast(packet);
                    Character.GuildAcademy.Broadcast(packet);
                }

                using (var packet = new InterPacket(InterOpCode.World.H55.ZONE_AcademyMemberOffline))
                {
                    packet.WriteInt32(Character.Guild.ID);
                    packet.WriteInt32(Character.ID);


                    ZoneManager.Broadcast(packet);
                }
            }
        }
        private static void On_CharacterManager_CharacterLevelUp(Character Character)
        {
            if (Character.IsInGuildAcademy)
            {
                using (var packet = new GamePacket(GameOpCode.Server.H38.AcademyMemberLevelUp))
                {
                    packet.WriteString(Character.Name, 16);
                    packet.WriteByte(Character.Level);


                    Character.Guild.Broadcast(packet);
                    Character.GuildAcademy.Broadcast(packet);
                }


                lock (Character.Guild.ThreadLocker)
                {
                    uint points;
                    if (GuildDataProvider.AcademyLevelUpPoints.TryGetValue(Character.Level, out points))
                    {
                        Character.GuildAcademy.Points += (ushort)points;
                    }



                    //add time to guild buff
                    var time = WorldService.Instance.Time;
                    var newTime = Math.Min(CharacterDataProvider.ChrCommon.GuildBuffMaxTime.TotalSeconds, (CharacterDataProvider.ChrCommon.GuildBuffAddTime.TotalSeconds + Character.GuildAcademy.GuildBuffKeepTime.TotalSeconds));
                    Character.GuildAcademy.GuildBuffKeepTime = TimeSpan.FromSeconds(newTime);

                    //update guild buff to all guild/aka members
                    var toUpdate = new List<Character>();
                    foreach (var member in Character.GuildAcademy.Members)
                    {
                        if (member.Character.IsOnline)
                        {
                            toUpdate.Add(member.Character);
                        }
                    }
                    foreach (var member in Character.Guild.Members)
                    {
                        if (member.Character.IsOnline
                            && !toUpdate.Contains(member.Character))
                        {
                            toUpdate.Add(member.Character);
                        }
                    }

                    BuffManager.SetBuff(GuildDataProvider.AcademyBuff, GuildDataProvider.AcademyBuffStrength, (uint)(newTime * 1000), toUpdate.ToArray());

                    toUpdate.Clear();
                    toUpdate = null;

                    //update guild buff to zones
                    using (var packet = new InterPacket(InterOpCode.World.H55.ZONE_AcademyBuffUpdate))
                    {
                        packet.WriteInt32(Character.Guild.ID);
                        packet.WriteDateTime(time);
                        packet.WriteDouble(newTime);



                        ZoneManager.Broadcast(packet);
                    }





                    //broadcast info and save guild
                    Character.GuildAcademy.BroadcastInfo();
                    Character.GuildAcademy.Save();
                }
            }
        }















        #region Game Client Handlers

        [GamePacketHandler(GameOpCode.Client.H38.GetAcademyList)]
        public static void On_GameClient_GetAcademyList(GameClient Client, FiestaPacket Packet)
        {
            if (Client.Character == null)
            {
                Client.Dispose();
                return;
            }



            const int GuildsPerPacket = 54;
            lock (GuildManager.ThreadLocker)
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
                                    listPacket = new GamePacket(GameOpCode.Server.H38.SendAcademyList);
                                    listPacket.WriteUInt16(6312);
                                    listPacket.WriteByte(1);
                                    listPacket.WriteUInt16((ushort)guildCount);
                                    listPacket.WriteUInt16(0);
                                    listPacket.WriteUInt16((ushort)Math.Min(GuildsPerPacket, guildCount - globalCount));
                                    listPacket.WriteUInt16(0);
                                }



                                Guild guild;
                                if (GuildManager.GetGuildByID(reader.GetInt32(0), out guild))
                                {
                                    //write packet
                                    listPacket.WriteString(guild.Name, 16);
                                    listPacket.WriteString(guild.Master.Character.Name, 16);
                                    listPacket.WriteUInt16((ushort)guild.Members.Count);
                                    listPacket.WriteUInt16((ushort)guild.Academy.Members.Count);
                                    listPacket.WriteUInt16(guild.Academy.Points); // Graduates
                                }
                                else
                                {
                                    Packet.Fill(38, 0); // guild get error
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

        [GamePacketHandler(GameOpCode.Client.H38.GetAcademyMemberList)]
        public static void On_GameClient_GetAcademyMemberList(GameClient Client, FiestaPacket Packet)
        {
            if (Client.Character == null)
            {
                Client.Dispose();
                return;
            }


            if (Client.Character.IsInGuildAcademy)
            {
                Client.Character.GuildAcademy.SendMemberList(Client);
            }
        }

        [GamePacketHandler(GameOpCode.Client.H38.JoinAcademy)]
        public static void On_GameClient_JoinAcademy(GameClient Client, FiestaPacket Packet)
        {
            string guildName;
            if (!Packet.ReadString(out guildName, 16))
            {
                Client.Dispose();
                return;
            }


            Guild guild;
            if (!GuildManager.GetGuildByName(guildName, out guild))
            {
                SH38Helpers.SendAcademyResponse(Client, guildName, GuildAcademyResponse.AcademyNotFound);
                return;
            }

            guild.Academy.AddMember(Client.Character, GuildAcademyRank.Member);
        }

        [GamePacketHandler(GameOpCode.Client.H38.LeaveAcademy)]
        public static void On_GameClient_LeaveAcademy(GameClient Client, FiestaPacket Packet)
        {
            if (Client.Character == null)
            {
                Client.Dispose();
                return;
            }


            if (Client.Character.IsInGuildAcademy)
            {
                Client.Character.GuildAcademy.RemoveMember(Client.Character.GuildAcademyMember);
            }
        }



        [GamePacketHandler(GameOpCode.Client.H38.AcademyChat)]
        public static void On_GameClient_AcademyChat(GameClient Client, FiestaPacket Packet)
        {
            byte len;
            string msg;
            if (!Packet.ReadByte(out len)
                || !Packet.ReadString(out msg, len))
            {
                Client.Dispose();
                return;
            }


            if (Client.Character.IsInGuildAcademy
                || Client.Character.IsInGuild)
            {
                if (Client.Character.IsInGuildAcademy
                    && Client.Character.GuildAcademyMember.IsChatBlocked)
                {
                    using (var packet = new GamePacket(GameOpCode.Server.H38.AcademyChatBlocked))
                    {
                        packet.WriteUInt16(6140);


                        Client.Send(packet);
                    }

                    return;
                }


                using (var packet = new GamePacket(GameOpCode.Server.H38.AcademyChat))
                {
                    packet.WriteInt32(Client.Character.Guild.ID);
                    packet.WriteString(Client.Character.Name, 16);
                    packet.WriteByte(len);
                    packet.WriteString(msg, len);



                    Client.Character.Guild.Broadcast(packet);
                    Client.Character.GuildAcademy.Broadcast(packet);
                }
            }
        }

        #endregion
    }
}