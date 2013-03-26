using System;
using DragonFiesta.Util;
using DragonFiesta.World.Game.Event;
using DragonFiesta.World.Game;

namespace DragonFiesta.World.Core
{

    public class EventManager
    {
        public static EventManager Instance { get; set; }

        public EventManager()
        {
            this.SetupEvents();
        }

        public void SetupEvents()
        {
            Event.SetupLoginEvent();
        }
        public static bool Initialize()
        {
            try
            {
                Instance = new EventManager();
                Logs.Main.Info("Successfully initialized EventManager");
                return true;
            }
            catch (Exception ex)
            {
                Logs.Main.Fatal("Could not initialize EventManager", ex);
                return false;
            }
        }


        #region evtndefines
        public static event CharacterEvent OnCharacterLogin;
        #endregion

        #region Execute
        public void InvokeOnCharacterLogin(WorldCharacter pChar)
        {
            OnCharacterLogin.Invoke(pChar);
        }
        #endregion

    }
}
