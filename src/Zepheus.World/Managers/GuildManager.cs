using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.World.Data;

namespace Zepheus.World.Managers
{
     [ServerModule(InitializationStage.Clients)]
       public sealed class GuildManager
       {
           #region Properties
           public List<GuildRequest> pRequests { get; private set; }
           public static GuildManager Instance { get; private set; }
           #endregion
           #region .ctor
           public GuildManager()
        {
            pRequests = new List<GuildRequest>();
        }
        [InitializerMethod]
        public static bool Initialize()
        {
            Instance = new GuildManager();
            return true;
        }
           #endregion
        #region Methods
        public void AddMember(WorldCharacter pChar,int GuildID)
        {
            GuildMember pMember = new GuildMember
            {
                CharID = pChar.ID,
                pMemberName = pChar.Character.Name,
                Level = pChar.Character.CharLevel,
                pMemberJob = pChar.Character.Job,
                pClient = pChar.Client,
                isOnline = true,
                GuildRank = GuildRanks.Member,
                GuildID = GuildID, 
            };
            pMember.AddToDatabase();
            Guild g = GetGuildByID(GuildID);
           if(g != null)//prevence
           {
               g.GuildMembers.Add(pMember);
               pChar.Guild = g;
           }
        }
        public void CreateGuildInvideRequest(string InvidetName, WorldCharacter pRequester)
        {
            Networking.WorldClient TargetClient = ClientManager.Instance.GetClientByCharname(InvidetName);
            if (TargetClient != null)
            {
                GuildRequest pRequest = new GuildRequest(pRequester.Client, TargetClient, pRequester.Guild);
                this.pRequests.Add(pRequest);
            }

        }
        public void RemoveMember(WorldCharacter pChar)
         {
            GuildMember pMember = pChar.Guild.GuildMembers.Find(m => m.CharID == pChar.ID);
            pChar.Guild.GuildMembers.Remove(pMember);
            pChar.Guild = null;
         }

        public void CreateGuild(WorldCharacter pChar,string GuildeName,string GuildPassword,bool GuildWar)
        {
            Guild gg = new Guild
             {
                 GuildMaster = pChar.Character.Name,
                 GuildPassword = GuildPassword,
                 Name = GuildeName,
                 GuildWar = GuildWar,
                 ID = DataProvider.Instance.Guilds.Count + 1,
             };

            GuildMember MasterMember = new GuildMember
            {
                CharID = pChar.ID,
                GuildRank = GuildRanks.Master,
                pClient = pChar.Client,
                isOnline = true,
                GuildID = gg.ID,
                Korp = 0,
                Level = pChar.Character.CharLevel,
                pMemberJob = pChar.Character.Job,
                pMemberName = pChar.Character.Name,
                MapName = DataProvider.GetMapname(pChar.Character.PositionInfo.Map),
            };
            MasterMember.AddToDatabase();
            gg.AddToDatabase();
            gg.GuildMembers.Add(MasterMember);
            DataProvider.Instance.Guilds.Add(gg.ID, gg);
        }
        public Guild GetGuildByID(int GuildID)
       {
           Guild Guild;
           if (DataProvider.Instance.Guilds.TryGetValue(GuildID, out Guild))
           {
               return Guild;
           }
           return null;
       }
        #endregion
       }
}
