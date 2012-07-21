using System;
using System.Collections.Generic;
using Zepheus.World.Data;
using Zepheus.World.Events;
using Zepheus.Util;

namespace Zepheus.World.Managers
{
    public class CharacterManager
    {

        public static event EventHandler<WorldCharacter> OnCharacterLogin;
        public static event EventHandler<OnCharacterLogoutArgs> OnCharacterLogout;
        public static event EventHandler<OnCharacterLevelUpArgs> OnCharacterLevelUp;
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
        public bool GetCharacterByID(int ID, out WorldCharacter pChar, MySql.Data.MySqlClient.MySqlConnection Conn = null)
        {
           pChar = ClientManager.Instance.GetClientByCharID(ID).Character;
           if (pChar != null)
           {
               return true;
           }
           else
           {
               //todo read from datatbase when offline
               return false;
           }
        }
    }
}
