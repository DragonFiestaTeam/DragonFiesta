using System;
using System.Collections.Generic;
using DragonFiesta.World.Core;
using DragonFiesta.World.EventHandler;

namespace DragonFiesta.World.Game.Event
{
   public delegate void CharacterEvent(WorldCharacter pChar);

   public class Event
   {
       public static void SetupLoginEvent()
       {
           EventManager.OnCharacterLogin += CharacterInfoEventHandler.CharacterLogin;
       }
   }
}
