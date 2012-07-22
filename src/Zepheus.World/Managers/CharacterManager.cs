using System;
using System.Collections.Generic;
using Zepheus.World.Data;
using Zepheus.World.Events;
using Zepheus.Util;
using Zepheus.Database.DataStore;
using Zepheus.Database.Storage;

namespace Zepheus.World.Managers
{
    public delegate void CharacterEvent(WorldCharacter Character);
    [ServerModule(InitializationStage.Clients)]
    public class CharacterManager
    {

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
        public bool GetCharacterByID(string Name, out WorldCharacter pChar)
        {
         World.Networking.WorldClient pclient = ClientManager.Instance.GetClientByCharname(Name);
           if (pclient != null)
           {
               pChar = pclient.Character;
               return true;
           }
           else
           {
               pChar = null;
              Character DBpChar  =  ReadMethods.ReadCharObjectFromDatabase(Name,Program.DatabaseManager);
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
