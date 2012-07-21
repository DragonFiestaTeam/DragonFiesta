/*File for this file Basic Copyright 2012 no0dl */

using System;
using Zepheus.InterLib.Networking;
using Zepheus.Zone.Game;
using Zepheus.InterLib;
using Zepheus.Zone.Managers;

namespace Zepheus.Zone.Game.Guilds.Academy
{
    public static class GuildAcademyManager
    {
        #region Internal Client Handlers
        [InterPacketHandler(InterHeader.ZONE_AcademyMemberJoined)]
        public static void On_WorldClient_AcademyMemberJoined(InterClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            DateTime registerDate;
            if (!Packet.TryReadInt(out guildID)
                || !Packet.TryReadInt(out characterID)
                || !Packet.TryReadDateTime(out registerDate))
            {
                return;
            }



            Guild guild;
            if (GuildManager.GetGuildByID(guildID, out guild))
            {
                var member = new GuildAcademyMember(guild.Academy, characterID, GuildAcademyRank.Member, registerDate)
                {
                    IsOnline = true,
                };
                guild.Academy.Members.Add(member);


                ZoneCharacter character;
                if (CharacterManager.GetLoggedInCharacter(characterID, out character))
                {
                    member.Character = character;
                    
                    character.Guild = guild;
                    character.GuildAcademy = guild.Academy;
                    character.GuildAcademyMember = member;


                    GuildManager.SetGuildBuff(character);
                }
            }
        }

        [InterPacketHandler(InterHeader.ZONE_AcademyMemberLeft)]
        public static void On_WorldClient_AcademyMemberLeft(InterClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            if (!Packet.TryReadInt(out guildID)
                || !Packet.TryReadInt(out characterID))
            {
                return;
            }


            Guild guild;
            if (GuildManager.GetGuildByID(guildID, out guild))
            {
                GuildAcademyMember member;
                if (guild.Academy.GetMember(characterID, out member))
                {
                    guild.Academy.Members.Remove(member);
                    member.Dispose();


                    ZoneCharacter character;
                    if (CharacterManager.GetLoggedInCharacter(characterID, out character))
                    {
                        character.Guild = null;
                        character.GuildAcademy = null;
                        character.GuildAcademyMember = null;


                        GuildManager.RemoveGuildBuff(character);
                    }
                }
            }
        }

        [InterPacketHandler(InterHeader.ZONE_AcademyMemberOnline)]
        public static void On_WorldClient_AcademyMemberOnline(InterClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            if (!Packet.TryReadInt(out guildID)
                || !Packet.TryReadInt(out characterID))
            {
                return;
            }


            Guild guild;
            if (GuildManager.GetGuildByID(guildID, out guild))
            {
                GuildAcademyMember member;
                if (guild.Academy.GetMember(characterID, out member))
                {
                    member.IsOnline = true;


                    ZoneCharacter character;
                    if (CharacterManager.GetLoggedInCharacter(characterID, out character))
                    {
                        character.Guild = guild;
                        character.GuildAcademy = guild.Academy;
                        character.GuildAcademyMember = member;

                        member.Character = character;
                    }
                }
            }
        }

        [InterPacketHandler(InterHeader.ZONE_AcademyMemberOffline)]
        public static void On_WorldClient_AcademyMemberOffline(InterClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            if (!Packet.TryReadInt(out guildID)
                || !Packet.TryReadInt(out characterID))
            {
                return;
            }


            Guild guild;
            if (GuildManager.GetGuildByID(guildID, out guild))
            {
                GuildAcademyMember member;
                if (guild.Academy.GetMember(characterID, out member))
                {
                    member.IsOnline = false;


                    ZoneCharacter character;
                    if (CharacterManager.GetLoggedInCharacter(characterID, out character))
                    {
                        character.Guild = null;
                        character.GuildAcademy = null;
                        character.GuildAcademyMember = null;

                        member.Character = null;
                    }
                }
            }
        }

        [InterPacketHandler(InterHeader.ZONE_AcademyBuffUpdate)]
        public static void On_WorldClient_AcademyBuffUpdate(InterClient Client, InterPacket Packet)
        {
            int guildID;
            DateTime updateTime;
            double keepTime;
            if (!Packet.TryReadInt(out guildID)
                || !Packet.TryReadDateTime(out updateTime)
                || !Packet.TryReadDouble(out keepTime))
            {
                //Client.Dispose();
                return;
            }


            Guild guild;
            if (GuildManager.GetGuildByID(guildID, out guild))
            {
                guild.Academy.GuildBuffUpdateTime = updateTime;
                guild.Academy.GuildBuffKeepTime = TimeSpan.FromSeconds(keepTime);
            }
        }

        #endregion
    }
}