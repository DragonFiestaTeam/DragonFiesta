using DragonFiesta.Data;
using System.Collections.Generic;
using DragonFiesta.Networking;
using DragonFiesta.Util;
using System;

namespace DragonFiesta.World
{
    public class MapManager
    {

                #region .ctor
        public MapManager()
        {
            
        }
        #endregion
        #region Properties

        public static MapManager Instance { get; private set; }

        #endregion
        #region Methods

        public static bool Initialize()
        {
            try
            {
                Instance = new MapManager();
            }
            catch(Exception e)
            {
                if (!ExceptionManager.Instance.HandleException(e))
                {
                    Logs.Main.Fatal("Could not initializing LoginClientManager", e);
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
