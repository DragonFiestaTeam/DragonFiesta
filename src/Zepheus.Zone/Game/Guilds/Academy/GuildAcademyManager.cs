using System;
using Fiesta.Core.Networking;
using Fiesta.Zone.Game.Characters;
using Fiesta.Zone.Inter;
using Fiesta.Zone.Networking;

namespace Fiesta.Zone.Game.Guilds.Academy
{
    public static class GuildAcademyManager
    {

















        #region Internal Client Handlers

        [InterWorldPacketHandlerMethod(InterOpCode.World.H55.ZONE_AcademyMemberJoined)]
        public static void On_WorldClient_AcademyMemberJoined(InterWorldClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            DateTime registerDate;
            if (!Packet.ReadInt32(out guildID)
                || !Packet.ReadInt32(out characterID)
                || !Packet.ReadDateTime(out registerDate))
            {
                Client.Dispose();
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


                Character character;
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

        [InterWorldPacketHandlerMethod(InterOpCode.World.H55.ZONE_AcademyMemberLeft)]
        public static void On_WorldClient_AcademyMemberLeft(InterWorldClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            if (!Packet.ReadInt32(out guildID)
                || !Packet.ReadInt32(out characterID))
            {
                Client.Dispose();
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


                    Character character;
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

        [InterWorldPacketHandlerMethod(InterOpCode.World.H55.ZONE_AcademyMemberOnline)]
        public static void On_WorldClient_AcademyMemberOnline(InterWorldClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            if (!Packet.ReadInt32(out guildID)
                || !Packet.ReadInt32(out characterID))
            {
                Client.Dispose();
                return;
            }


            Guild guild;
            if (GuildManager.GetGuildByID(guildID, out guild))
            {
                GuildAcademyMember member;
                if (guild.Academy.GetMember(characterID, out member))
                {
                    member.IsOnline = true;


                    Character character;
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

        [InterWorldPacketHandlerMethod(InterOpCode.World.H55.ZONE_AcademyMemberOffline)]
        public static void On_WorldClient_AcademyMemberOffline(InterWorldClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            if (!Packet.ReadInt32(out guildID)
                || !Packet.ReadInt32(out characterID))
            {
                Client.Dispose();
                return;
            }


            Guild guild;
            if (GuildManager.GetGuildByID(guildID, out guild))
            {
                GuildAcademyMember member;
                if (guild.Academy.GetMember(characterID, out member))
                {
                    member.IsOnline = false;


                    Character character;
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

        [InterWorldPacketHandlerMethod(InterOpCode.World.H55.ZONE_AcademyBuffUpdate)]
        public static void On_WorldClient_AcademyBuffUpdate(InterWorldClient Client, InterPacket Packet)
        {
            int guildID;
            DateTime updateTime;
            double keepTime;
            if (!Packet.ReadInt32(out guildID)
                || !Packet.ReadDateTime(out updateTime)
                || !Packet.ReadDouble(out keepTime))
            {
                Client.Dispose();
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