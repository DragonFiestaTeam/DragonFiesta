using System;
using System.Data;
using System.Collections.Generic;
using Zepheus.World.Data;
using Zepheus.World.Events;
using Zepheus.Util;
using Zepheus.Database.DataStore;
using Zepheus.Database.Storage;
using Zepheus.Database;
namespace Zepheus.World.Managers
{
    public delegate void CharacterEvent(WorldCharacter Character);
    [ServerModule(InitializationStage.CharacterManager)]
    public class CharacterManager
    {
        public static event CharacterEvent CharacterLogin;//this wehen change map or another
        public static event CharacterEvent OnCharacterLogin;
        public static event CharacterEvent OnCharacterLogout;
        public static event CharacterEvent OnCharacterLevelUp;
        public static CharacterManager Instance { get; set; }
        [InitializerMethod]
        public static bool init()
        {
            Instance = new CharacterManager();

          
            return true;
        }
        public static void invokeLoggetInEvent(WorldCharacter pChar)
        {
            OnCharacterLogin.Invoke(pChar);
        }
        public static void InvokdeIngame(WorldCharacter pChar)
        {
            CharacterLogin.Invoke(pChar);
        }
        public static void InvokeLoggetOutInEvent(WorldCharacter pChar)
        {
         OnCharacterLogout.Invoke(pChar);
        }
        public static void OneLoadGuildInCharacter(WorldCharacter pChar)
        {
         DatabaseClient dbClient = Program.DatabaseManager.GetClient();
         int GuildID = dbClient.ReadInt32("SELECT GuildID FROM guildmembers WHERE CharID='" + pChar.ID + "'");
         int AcademyID = dbClient.ReadInt32("SELECT GuildID FROM guildacademymembers WHERE CharID='" + pChar.ID + "'");
            if(AcademyID > 0 && GuildID == 0)
            {
                Data.Guilds.Guild g;
                if (!Data.Guilds.GuildManager.GetGuildByID(AcademyID, out g))
                    return;
                pChar.GuildAcademy = g.Academy;
                pChar.IsInGuildAcademy = true;
            }
            else if(GuildID > 0 && AcademyID == 0)
            {
                Data.Guilds.Guild g;
                if (!Data.Guilds.GuildManager.GetGuildByID(GuildID, out g))
                    return;
                pChar.Guild = g;
                pChar.IsInGuild = true;
            }
        }
        public static bool GetLoggedInCharacter(int ID, out WorldCharacter pChar)
        {
            pChar = ClientManager.Instance.GetClientByCharID(ID).Character;
            if (pChar != null)
            {
                return true;
            }
            return false;
        }
        public static bool GetLoggedInCharacter(string Name, out WorldCharacter pChar)
        {
            pChar = ClientManager.Instance.GetClientByCharname(Name).Character;
            if (pChar != null)
            {
                return true;
            }
            return false;
        }
        public bool GetCharacterByID(int ID, out WorldCharacter pChar)
        {
         World.Networking.WorldClient pclient = ClientManager.Instance.GetClientByCharID(ID);
           if (pclient != null)
           {
               pChar = pclient.Character;
               return true;
           }
           else
           {
               pChar = null;
              Character DBpChar  =  ReadMethods.ReadCharObjectByIDFromDatabase(ID,Program.DatabaseManager);
              WorldCharacter ReaderChar = new WorldCharacter(DBpChar, null);
              pChar = ReaderChar;
              if (DBpChar == null)
              {
                  return false;
              }
              return true;
           }
        }
    }
}
