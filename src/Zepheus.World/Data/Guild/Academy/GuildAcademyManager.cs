﻿/*File for this file Basic Copyright 2012 no0dl */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Zepheus.Util;
using Zepheus.FiestaLib.Networking;
using Zepheus.World.Networking;
using Zepheus.FiestaLib;
using Zepheus.World.Managers;
using Zepheus.InterLib.Networking;
using Zepheus.InterLib;
/*using Fiesta.Core.Networking;
using Fiesta.World.Data.Characters;
using Fiesta.World.Data.Guilds;
using Fiesta.World.Game.Characters;
using Fiesta.World.Game.Buffs;
using Fiesta.World.Game.Zones;
using Fiesta.World.Networking;
using Fiesta.World.Networking.Helpers;*/

namespace Zepheus.World.Data.Guilds.Academy
{
    [ServerModule(InitializationStage.Clients)]  
    public static class GuildAcademyManager
    {
        [InitializerMethod]
        public static bool OnAppStart()
        {
            CharacterManager.OnCharacterLogin += On_CharacterManager_CharacterLogin;
            CharacterManager.OnCharacterLogout += On_CharacterManager_CharacterLogout;
            CharacterManager.OnCharacterLevelUp += On_CharacterManager_CharacterLevelUp;
            return true;
        }
        private static void On_CharacterManager_CharacterLogin(WorldCharacter Character)
        {
            if (Character.IsInGuildAcademy)
            {
                using (var packet = new Packet(SH38Type.AcademyMemberLoggedIn))
                {
                    packet.WriteString(Character.Character.Name, 16);

                    
                    Character.Guild.Broadcast(packet);
                    Character.GuildAcademy.Broadcast(packet);
                }

                using (var packet = new InterPacket(InterLib.Networking.InterHeader.ZONE_AcademyMemberOnline))
                {
                    packet.WriteInt(Character.Guild.ID);
                    packet.WriteInt(Character.ID);


                    ZoneManager.Instance.Broadcast(packet);
                }
            }
        }
        private static void On_CharacterManager_CharacterLogout(WorldCharacter Character)
        {
            if (Character.IsInGuildAcademy)
            {
                using (var packet = new Packet(SH38Type.AcademyMemberLoggedOut))
                {
                    packet.WriteString(Character.Character.Name, 16);


                    Character.Guild.Broadcast(packet);
                    Character.GuildAcademy.Broadcast(packet);
                }

                using (var packet = new InterPacket(InterHeader.ZONE_AcademyMemberOffline))
                {
                    packet.WriteInt(Character.Guild.ID);
                    packet.WriteInt(Character.ID);


                    ZoneManager.Instance.Broadcast(packet);
                }
            }
        }
        private static void On_CharacterManager_CharacterLevelUp(WorldCharacter Character)
        {
            //fix later
            if (Character.IsInGuildAcademy)
            {
                using (var packet = new Packet(SH38Type.AcademyMemberLevelUp))
                {
                    packet.WriteString(Character.Character.Name, 16);
                    packet.WriteByte(Character.Character.CharLevel);


                    Character.Guild.Broadcast(packet);
                    Character.GuildAcademy.Broadcast(packet);
                }


                lock (Character.Guild.ThreadLocker)
                {
                    uint points;
                    if (GuildDataProvider.Instance.AcademyLevelUpPoints.TryGetValue(Character.Character.CharLevel, out points))
                    {
                        Character.GuildAcademy.Points += (ushort)points;
                    }



                    //add time to guild buff
                    var time = Program.CurrentTime;
                    //var newTime = Math.Min(CharacterDataProvider.ChrCommon.GuildBuffMaxTime.TotalSeconds, (CharacterDataProvider.ChrCommon.GuildBuffAddTime.TotalSeconds + Character.GuildAcademy.GuildBuffKeepTime.TotalSeconds));
                    //Character.GuildAcademy.GuildBuffKeepTime = TimeSpan.FromSeconds(newTime);

                    //update guild buff to all guild/aka members
                    var toUpdate = new List<WorldCharacter>();
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

                   //BuffManager.SetBuff(GuildDataProvider.AcademyBuff, GuildDataProvider.AcademyBuffStrength, (uint)(newTime * 1000), toUpdate.ToArray());

                    toUpdate.Clear();
                    toUpdate = null;

                    //update guild buff to zones
                    using (var packet = new InterPacket(InterHeader.ZONE_AcademyBuffUpdate))
                    {
                        packet.WriteInt(Character.Guild.ID);
                        packet.WriteDateTime(time);
                        packet.WriteDouble(900);//fix later



                        ZoneManager.Instance.Broadcast(packet);
                    }





                    //broadcast info and save guild
                    Character.GuildAcademy.BroadcastInfo();
                    Character.GuildAcademy.Save();
               }
            }
        }


        #region Game Client Handlers

        [PacketHandler(CH38Type.GetAcademyList)]
        public static void On_GameClient_GetAcademyList(WorldClient Client, Packet pPacket)
        {
            if (Client.Character == null)
            {
                return;
            }



            const int GuildsPerPacket = 54;
            lock (GuildManager.ThreadLocker)
            {
                using (var con = Program.DatabaseManager.GetClient().GetConnection())
                {
                    //get guild count
                    int guildCount;
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM Guilds";


                        guildCount = Convert.ToInt32(cmd.ExecuteScalar());
                    }


                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "SELECT ID FROM Guilds";


                        Packet listPacket = null;
                        var count = 0;
                        var globalCount = 0;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (listPacket == null)
                                {
                                    listPacket = new Packet(SH38Type.SendAcademyList);
                                    listPacket.WriteUShort(6312);
                                    listPacket.WriteByte(1);
                                    listPacket.WriteUShort((ushort)guildCount);
                                    listPacket.WriteUShort(0);
                                    listPacket.WriteUShort((ushort)Math.Min(GuildsPerPacket, guildCount - globalCount));
                                    listPacket.WriteUShort(0);
                                }



                                Guild guild;
                                if (GuildManager.GetGuildByID(reader.GetInt32("ID"), out guild))
                                {
                                    //write packet
                                    listPacket.WriteString(guild.Name, 16);
                                    listPacket.WriteString(guild.Master.Character.Character.Name, 16);
                                    listPacket.WriteUShort((ushort)guild.Members.Count);
                                    listPacket.WriteUShort((ushort)guild.Academy.Members.Count);
                                    listPacket.WriteUShort(guild.Academy.Points); // Graduates
                                }
                                else
                                {
                                    pPacket.Fill(38, 0); // guild get error
                                }



                                globalCount++;
                                count++;
                                if (count >= Math.Min(GuildsPerPacket, guildCount - globalCount))
                                {
                                    //send packet
                                    Client.SendPacket(listPacket);

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

        [PacketHandler(CH38Type.GetAcademyMemberList)]
        public static void On_GameClient_GetAcademyMemberList(WorldClient Client, Packet Packet)
        {
            if (Client.Character == null)
            {
                return;
            }


            if (Client.Character.IsInGuildAcademy)
            {
                Client.Character.GuildAcademy.SendMemberList(Client);
            }
        }

        [PacketHandler(CH38Type.JoinAcademy)]
        public static void On_GameClient_JoinAcademy(WorldClient Client, Packet Packet)
        {
            string guildName;
            if (!Packet.TryReadString(out guildName, 16))
            {
                return;
            }


            Guild guild;
            if (!GuildManager.GetGuildByName(guildName, out guild))
            {
                Handlers.Handler38.SendAcademyResponse(Client, guildName, GuildAcademyResponse.AcademyNotFound);
                return;
            }

            guild.Academy.AddMember(Client.Character, GuildAcademyRank.Member);
        }

        [PacketHandler(CH38Type.LeaveAcademy)]
        public static void On_GameClient_LeaveAcademy(WorldClient Client, Packet Packet)
        {
            if (Client.Character == null)
            {
                return;
            }


            if (Client.Character.IsInGuildAcademy)
            {
                Client.Character.GuildAcademy.RemoveMember(Client.Character.GuildAcademyMember);
            }
        }



        [PacketHandler(CH38Type.AcademyChat)]
        public static void On_GameClient_AcademyChat(WorldClient Client, Packet Packet)
        {
            byte len;
            string msg;
            if (!Packet.TryReadByte(out len)
                || !Packet.TryReadString(out msg, len))
            {
                return;
            }


            if (Client.Character.IsInGuildAcademy
                || Client.Character.IsInGuild)
            {
                if (Client.Character.IsInGuildAcademy
                    && Client.Character.GuildAcademyMember.IsChatBlocked)
                {
                    using (var packet = new Packet(SH38Type.AcademyChatBlocked))
                    {
                        packet.WriteUShort(6140);


                        Client.SendPacket(packet);
                    }

                    return;
                }


                using (var packet = new Packet(SH38Type.AcademyChat))
                {
                    packet.WriteInt(Client.Character.Guild.ID);
                    packet.WriteString(Client.Character.Character.Name, 16);
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