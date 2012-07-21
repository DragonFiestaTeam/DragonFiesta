﻿/*File for this file Basic Copyright 2012 no0dl */
using System;
using System.Data;
using MySql.Data.MySqlClient;
using Zepheus.Util;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.InterLib.Networking;
using Zepheus.InterLib;
using System.Collections.Generic;
using Zepheus.Zone.Managers;

namespace Zepheus.Zone.Game.Guilds
{
    public static class GuildManager
    {
        private static List<Guild> LoadedGuilds;
        private static object ThreadLocker;
        





        [InitializerMethod]
        public static void OnAppStart()
        {
            LoadedGuilds = new List<Guild>();
            ThreadLocker = new object();


            
      //   Managers.CharacterManager.OnCharacterLogin += On_CharacterManager_CharacterLogin;
        }
        private static void On_CharacterManager_CharacterLogin(ZoneCharacter Character)
        {
            SetGuildBuff(Character);
        }


        public static void RemoveGuildBuff(ZoneCharacter Character)
        {
          /*  Buff buff;
            if (Character.Buffs.GetBuff(b => b.AbStateInfo.ID.Equals(GuildDataProvider.AcademyBuff.ID), out buff))
            {
                Character.Buffs.Remove(buff);
            }*/
        }
        public static void SetGuildBuff(ZoneCharacter Character)
        {
            //Later
            //check if character needs guild buff
           /* if (Character.IsInGuild
                || Character.IsInGuildAcademy)
            {
                var remainingBuffTime = (Character.GuildAcademy.GuildBuffKeepTime - (ZoneService.Instance.Time - Character.GuildAcademy.GuildBuffUpdateTime)).TotalMilliseconds;
                if (remainingBuffTime > 0)
                {
                    Character.Buffs.Add(GuildDataProvider.AcademyBuff, GuildDataProvider.AcademyBuffStrength, (uint)remainingBuffTime);
                }
            }*/
        }
        public static bool GetGuildByID(int GuildID, out Guild Guild)
        {
            lock (ThreadLocker)
            {
                if ((Guild = LoadedGuilds.Find(g => g.ID.Equals(GuildID))) == null)
                {
                    //load from db
                    using (var con = Program.CharDBManager.GetClient().GetConnection())
                    {
                        using (var cmd = con.CreateCommand())
                        {
                            cmd.CommandText = "SELECT * FROM Guilds WHERE ID = @pID";

                            cmd.Parameters.Add(new MySqlParameter("@pID", GuildID));


                            using (var reader = cmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                    return false;

                                //create new guild
                                Guild = new Guild(reader, con);


                                //add to cache
                                LoadedGuilds.Add(Guild);
                            }
                        }
                    }
                }
            }

            return (Guild != null);
        }












        #region Internal Client Handlers

        [InterPacketHandler(InterHeader.ZONE_GuildCreated)]
        public static void On_InterClient_GuildCreated(InterClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            if (!Packet.TryReadInt(out guildID)
                || !Packet.TryReadInt(out characterID))
            {
                return;
            }


            Guild guild;
            if (GetGuildByID(guildID, out guild))
            {
                //check if character is on local zone, if so assign guild to him
                ZoneCharacter character;
                if (CharacterManager.GetLoggedInCharacter(characterID, out character))
                {
                    character.Guild = guild;
                    character.GuildAcademy = guild.Academy;


                    GuildMember member;
                    if (guild.GetMember(characterID, out member))
                    {
                        member.Character = character;
                        character.GuildMember = member;
                    }
                }
            }
        }

        [InterPacketHandler(InterHeader.ZONE_GuildMemberLogin)]
        public static void On_InterClient_GuildMemberLogin(InterClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            if (!Packet.TryReadInt(out guildID)
                || !Packet.TryReadInt(out characterID))
            {
                return;
            }

            Guild guild;
            if (GetGuildByID(guildID, out guild))
            {
                GuildMember member;
                if (guild.GetMember(characterID, out member))
                {
                    member.IsOnline = true;


                    ZoneCharacter character;
                    if (CharacterManager.GetLoggedInCharacter(characterID, out character))
                    {
                        character.Guild = guild;
                        character.GuildAcademy = guild.Academy;
                        character.GuildMember = member;
                        member.Character = character;
                    }
                }
            }
        }

        [InterPacketHandler(InterHeader.ZONE_GuildMemberLogout)]
        public static void On_InterClient_GuildMemberLogout(InterClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            if (!Packet.TryReadInt(out guildID)
                || !Packet.TryReadInt(out characterID))
            {

                return;
            }

            Guild guild;
            if (GetGuildByID(guildID, out guild))
            {
                GuildMember member;
                if (guild.GetMember(characterID, out member))
                {
                    member.Character = null;
                    member.IsOnline = false;
                }
            }
        }

        [InterPacketHandler(InterHeader.ZONE_GuildMessageUpdate)]
        public static void On_InterClient_GuildMessageUpdate(InterClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            DateTime createTime;
            ushort length;
            string message;
            if (!Packet.TryReadInt(out guildID)
                || !Packet.TryReadInt(out characterID)
                || !Packet.TryReadDateTime(out createTime)
                || !Packet.TryReadUShort(out length)
                || !Packet.TryReadString(out message, length))
            {
                return;
            }


            Guild guild;
            if (GetGuildByID(guildID, out guild))
            {
                //update guild
                guild.Message = message;
                guild.MessageCreateTime = createTime;
                guild.MessageCreaterID = characterID;
            }
        }

        [InterPacketHandler(InterHeader.ZONE_GuildMemberAdd)]
        public static void On_InterClient_GuildMemberAdd(InterClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            byte rank;
            ushort corp;
            if (!Packet.TryReadInt(out guildID)
                || !Packet.TryReadInt(out characterID)
                || !Packet.TryReadByte(out rank)
                || !Packet.TryReadUShort(out corp))
            {
                return;
            }

            Guild guild;
            if (GetGuildByID(guildID, out guild))
            {
                lock (guild.ThreadLocker)
                {
                    //create member
                    var member = new GuildMember(guild, characterID, (GuildRank)rank, corp)
                    {
                        IsOnline = true,
                    };

                    guild.Members.Add(member);



                    //check if member is on this zone, if so assign guild to him
                    ZoneCharacter character;
                    if (CharacterManager.GetLoggedInCharacter(characterID, out character))
                    {
                        character.Guild = guild;
                        character.GuildAcademy = guild.Academy;
                        character.GuildMember = member;

                        member.Character = character;


                        SetGuildBuff(character);
                    }
                }
            }
        }

        [InterPacketHandler(InterHeader.ZONE_GuildMemberRemove)]
        public static void On_InterClient_GuildMemberRemove(InterClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            if (!Packet.TryReadInt(out guildID)
                || !Packet.TryReadInt(out characterID))
            {
                return;
            }


            Guild guild;
            if (GetGuildByID(guildID, out guild))
            {
                lock (guild.ThreadLocker)
                {
                    GuildMember member;
                    if (guild.GetMember(characterID, out member))
                    {
                        //remove member and clean up
                        guild.Members.Remove(member);

                        member.Dispose();


                        //check if member is on this zone
                        ZoneCharacter character;
                        if (CharacterManager.GetLoggedInCharacter(characterID, out character))
                        {
                            character.Guild = null;
                            character.GuildAcademy = null;
                            character.GuildMember = null;


                            RemoveGuildBuff(character);
                        }
                    }
                }
            }
        }

        [InterPacketHandler(InterHeader.ZONE_GuildMemberRankUpdate)]
        public static void On_InterClient_GuildMemberRankUpdate(InterClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            byte newRank;
            if (!Packet.TryReadInt(out guildID)
                || !Packet.TryReadInt(out characterID)
                || !Packet.TryReadByte(out newRank))
            {
                return;
            }



            Guild guild;
            if (GetGuildByID(guildID, out guild))
            {
                lock (guild.ThreadLocker)
                {
                    GuildMember member;
                    if (guild.GetMember(characterID, out member))
                    {
                        member.Rank = (GuildRank)newRank;
                    }
                }
            }
        }

        #endregion
    }
}