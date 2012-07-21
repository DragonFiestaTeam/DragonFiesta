using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Fiesta.Core.Networking;
using Fiesta.Zone.Data.Guilds;
using Fiesta.Zone.Game.Buffs;
using Fiesta.Zone.Game.Characters;
using Fiesta.Zone.Inter;
using Fiesta.Zone.Networking;

namespace Fiesta.Zone.Game.Guilds
{
    public static class GuildManager
    {
        private static List<Guild> LoadedGuilds;
        private static object ThreadLocker;
        





        [ServiceStartMethod(ServiceStartStep.Logic)]
        public static void OnAppStart()
        {
            LoadedGuilds = new List<Guild>();
            ThreadLocker = new object();


            
            CharacterManager.OnCharacterLogin += On_CharacterManager_CharacterLogin;
        }
        private static void On_CharacterManager_CharacterLogin(Character Character)
        {
            SetGuildBuff(Character);
        }


        public static void RemoveGuildBuff(Character Character)
        {
            Buff buff;
            if (Character.Buffs.GetBuff(b => b.AbStateInfo.ID.Equals(GuildDataProvider.AcademyBuff.ID), out buff))
            {
                Character.Buffs.Remove(buff);
            }
        }
        public static void SetGuildBuff(Character Character)
        {
            //check if character needs guild buff
            if (Character.IsInGuild
                || Character.IsInGuildAcademy)
            {
                var remainingBuffTime = (Character.GuildAcademy.GuildBuffKeepTime - (ZoneService.Instance.Time - Character.GuildAcademy.GuildBuffUpdateTime)).TotalMilliseconds;
                if (remainingBuffTime > 0)
                {
                    Character.Buffs.Add(GuildDataProvider.AcademyBuff, GuildDataProvider.AcademyBuffStrength, (uint)remainingBuffTime);
                }
            }
        }
        public static bool GetGuildByID(int GuildID, out Guild Guild)
        {
            lock (ThreadLocker)
            {
                if ((Guild = LoadedGuilds.Find(g => g.ID.Equals(GuildID))) == null)
                {
                    //load from db
                    using (var con = DatabaseManager.DB_Game.GetConnection())
                    {
                        using (var cmd = con.CreateCommand())
                        {
                            cmd.CommandText = "SELECT * FROM Guilds WHERE ID = @pID";

                            cmd.Parameters.Add(new SqlParameter("@pID", GuildID));


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

        [InterWorldPacketHandlerMethod(InterOpCode.World.H53.ZONE_GuildCreated)]
        public static void On_InterClient_GuildCreated(InterWorldClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            if (!Packet.ReadInt32(out guildID)
                || !Packet.ReadInt32(out characterID))
            {
                Client.Dispose();
                return;
            }


            Guild guild;
            if (GetGuildByID(guildID, out guild))
            {
                //check if character is on local zone, if so assign guild to him
                Character character;
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

        [InterWorldPacketHandlerMethod(InterOpCode.World.H53.ZONE_GuildMemberLogin)]
        public static void On_InterClient_GuildMemberLogin(InterWorldClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            if (!Packet.ReadInt32(out guildID)
                || !Packet.ReadInt32(out characterID))
            {
                Client.Dispose();
                return;
            }

            Guild guild;
            if (GetGuildByID(guildID, out guild))
            {
                GuildMember member;
                if (guild.GetMember(characterID, out member))
                {
                    member.IsOnline = true;


                    Character character;
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

        [InterWorldPacketHandlerMethod(InterOpCode.World.H53.ZONE_GuildMemberLogout)]
        public static void On_InterClient_GuildMemberLogout(InterWorldClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            if (!Packet.ReadInt32(out guildID)
                || !Packet.ReadInt32(out characterID))
            {
                Client.Dispose();
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

        [InterWorldPacketHandlerMethod(InterOpCode.World.H53.ZONE_GuildMessageUpdate)]
        public static void On_InterClient_GuildMessageUpdate(InterWorldClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            DateTime createTime;
            ushort length;
            string message;
            if (!Packet.ReadInt32(out guildID)
                || !Packet.ReadInt32(out characterID)
                || !Packet.ReadDateTime(out createTime)
                || !Packet.ReadUInt16(out length)
                || !Packet.ReadString(out message, length))
            {
                Client.Dispose();
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

        [InterWorldPacketHandlerMethod(InterOpCode.World.H53.ZONE_GuildMemberAdd)]
        public static void On_InterClient_GuildMemberAdd(InterWorldClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            byte rank;
            ushort corp;
            if (!Packet.ReadInt32(out guildID)
                || !Packet.ReadInt32(out characterID)
                || !Packet.ReadByte(out rank)
                || !Packet.ReadUInt16(out corp))
            {
                Client.Dispose();
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
                    Character character;
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

        [InterWorldPacketHandlerMethod(InterOpCode.World.H53.ZONE_GuildMemberRemove)]
        public static void On_InterClient_GuildMemberRemove(InterWorldClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            if (!Packet.ReadInt32(out guildID)
                || !Packet.ReadInt32(out characterID))
            {
                Client.Dispose();
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
                        Character character;
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

        [InterWorldPacketHandlerMethod(InterOpCode.World.H53.ZONE_GuildMemberRankUpdate)]
        public static void On_InterClient_GuildMemberRankUpdate(InterWorldClient Client, InterPacket Packet)
        {
            int guildID, characterID;
            byte newRank;
            if (!Packet.ReadInt32(out guildID)
                || !Packet.ReadInt32(out characterID)
                || !Packet.ReadByte(out newRank))
            {
                Client.Dispose();
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