using System;
using Zepheus.Zone.Game;

namespace Zepheus.Zone.Managers
{
   public class CharacterManager
    {
      //public static Event OnCharacterLogin(ZoneCharacter pChar);

      public static bool GetLoggedInCharacter(int ID,out ZoneCharacter pChar)
      {
          pChar = ClientManager.Instance.GetClientByCharID(ID).Character;
          if (pChar != null)
          {
              return true;
          }
          return false;
      }
    }
}
