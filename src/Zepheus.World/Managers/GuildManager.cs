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
        public static GuildManager Instance { get; private set; }
        public GuildManager()
        {
        }
        [InitializerMethod]
        public static bool Initialize()
        {
            Instance = new GuildManager();
            return true;
        }
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
                GuildRank = 6,// 6 = GuildMember
                GuildID = GuildID, 
            };
            Guild g = GetGuildByID(GuildID);
           if(g != null)//prevence
           {
               g.GuildMembers.Add(pMember);
               pChar.Guild = g;
           }
        }
         public void RemoveMember(WorldCharacter pChar)
         {
            GuildMember pMember = pChar.Guild.GuildMembers.Find(m => m.CharID == pChar.ID);
            pChar.Guild.GuildMembers.Remove(pMember);
            pChar.Guild = null;
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
        
    }
}
