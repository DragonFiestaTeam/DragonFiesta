using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.World.Data;
using Zepheus.World.Networking;

namespace Zepheus.World.Managers
{
     [ServerModule(InitializationStage.Clients)]
       public  class GuildManager : Academy
       {
           #region Properties
           public List<GuildRequest> pRequests { get; private set; }
           public List<ChangeRequest> ChangeRequest { get; private set; }
           public static GuildManager Instance { get; private set; }
           #endregion
           #region .ctor
           public GuildManager()
        {
            pRequests = new List<GuildRequest>();
            ChangeRequest = new List<ChangeRequest>();
        }
        [InitializerMethod]
        public static bool Initialize()
        {
            Instance = new GuildManager();
      
            return true;
        }
           #endregion
        #region Methods
        public void AddReuest(ChangeRequest Request)
        {
            this.ChangeRequest.Add(Request);
        }
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
            pRequest.Guild.GuildMembers.Add(pMember);
            pRequest.pTarget.Character.Character.GuildID = pMember.GuildID;
            pMember.pClient.Character.Guild = pRequest.Guild;
            pRequest.pTarget.Character.Guild = pRequest.Guild;
            pMember.AddToDatabase();
            foreach (var GuildMember in pRequest.Guild.GuildMembers)
            {
                if (GuildMember.isOnline)
                {
                    using (var packet = new Packet(SH29Type.AddGuildMember))
                    {
                        pMember.WriteInfo(packet);
                        GuildMember.pClient.SendPacket(packet);
                        GuildMember.SendMemberStatus(true, pMember.pMemberName);

                    }
                }
            }
            for (int i = 0; i < pRequest.Guild.GuildMembers.Count; i += 20)
            {
                Packet pack = Guild.MultiMemberList(pRequest.Guild.GuildMembers, i, i + Math.Min(20, pRequest.Guild.GuildMembers.Count - i), pRequest.Guild.GuildMembers.Count);
                pMember.pClient.SendPacket(pack);
            }
            using (var p2 = new Packet(SH4Type.CharacterGuildinfo))
            {
                pRequest.Guild.Details.WriteMessageAsGuildMember(p2, pRequest.Guild);
                pMember.pClient.SendPacket(p2);
            }
        }
        public void CreateGuildInvideRequest(string InvidetName, WorldCharacter pRequester)
        {
            Networking.WorldClient TargetClient = ClientManager.Instance.GetClientByCharname(InvidetName);
            if (TargetClient != null)
            {
                GuildRequesterResponse Response = new GuildRequesterResponse(pRequester.Client, TargetClient);
                if (Response.GetReponse())
                {
                    GuildRequest pRequest = new GuildRequest(pRequester.Client, TargetClient, pRequester.Guild);
                    this.pRequests.Add(pRequest);
                }
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

        private bool GetFreeGuildSlot(out int GuildID)
        {
            GuildID = 0;
            for (int i = 0; i < int.MaxValue; i++)
            {
                if (!DataProvider.Instance.GuildsByID.ContainsKey(i))
                {
                    if(i != 0)
                    {
                    GuildID = i;
                    return true;
                    }
                    //GuildID = 1;
                 
                }

            }
            return false;
        }
        public void CreateGuild(WorldCharacter pChar,string GuildeName,string GuildPassword,bool GuildWar)
        {
            int GuildID;
            if (!GetFreeGuildSlot(out GuildID))
                return;

            Guild gg = new Guild
             {
                 GuildMaster = pChar.Character.Name,
                 GuildPassword = GuildPassword,
                 Name = GuildeName,
                 GuildWar = GuildWar,
                 ID = GuildID,
             };
            gg.Details = new DetailsMessage
            {
                Creater = "",
                Message = "",
                CreateTime = DateTime.Now,
                lenght = 0,
                GuildOwner = gg.Name,
            };
            gg.GuildAcademy = new Academy
            {
                AcademyMembers = new List<AcademyMember>(),
                Details = gg.Details,
                Guild = gg,
                GuildAcademy = gg.GuildAcademy,
                GuildBuffTime = 0,
                GuildMaster = gg.GuildMaster,
                GuildMembers = gg.GuildMembers,
                GuildPassword = gg.GuildPassword,
                GuildWar = gg.GuildWar,
                ID = gg.ID,
                MaxMemberCount = gg.MaxMemberCount,
                Name = gg.Name,
                RegisterDate = gg.RegisterDate,
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
            pChar.Guild = gg;
            pChar.Character.GuildID = gg.ID;
            Log.WriteLine(LogLevel.Debug, "Create New Guild With ID {0}", gg.ID);
            DataProvider.Instance.GuildsByID.Add(gg.ID, gg);
            DataProvider.Instance.GuildsByName.Add(gg.Name, gg);
            InterServer.InterHandler.CreateGuildOfZones(gg);
            pChar.BroudCastGuildNameResult();
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
