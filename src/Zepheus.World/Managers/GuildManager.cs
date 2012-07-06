using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.World.Data;

namespace Zepheus.World.Managers
{
     [ServerModule(InitializationStage.Clients)]
       public  class GuildManager : Academy
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
        public void AddMember(GuildRequest pRequest)
        {
           
            GuildMember pMember = new GuildMember
            {
                CharID = pRequest.pTarget.Character.ID,
                pMemberName = pRequest.pTarget.Character.Character.Name,
                Level = pRequest.pTarget.Character.Character.CharLevel,
                pMemberJob = pRequest.pTarget.Character.Character.Job,
                pClient = pRequest.pTarget,
                isOnline = true,
                GuildRank = GuildRanks.Member,
                GuildID = pRequest.Guild.ID, 
            };
            pMember.AddToDatabase();
            pRequest.Guild.GuildMembers.Add(pMember);
            pRequest.pTarget.Character.Character.GuildID = pMember.GuildID;
            pRequest.pTarget.Character.Guild = pRequest.Guild;
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
         public void RemoveGuildRequest(GuildRequest pRequest)
        {
           this.pRequests.Remove(pRequest);
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
                 ID = DataProvider.Instance.GuildsByID.Count + 1,
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
            DataProvider.Instance.GuildsByID.Add(gg.ID, gg);
        }
        public Guild GetGuildByName(string GuildName)
        {
            Guild gg;
            if (!DataProvider.Instance.GuildsByName.TryGetValue(GuildName, out gg))
                return null;

            return gg;
        }
        public Guild GetGuildByID(int GuildID)
       {
           Guild Guild;
           if (DataProvider.Instance.GuildsByID.TryGetValue(GuildID, out Guild))
           {
               return Guild;
           }
           return null;
       }
        #endregion
       }
}
