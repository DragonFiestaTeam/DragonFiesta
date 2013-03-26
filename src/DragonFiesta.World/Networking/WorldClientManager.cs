using System;
using DragonFiesta.Networking;
using DragonFiesta.Util;
using DragonFiesta.World.Networking.ServerHandler;

namespace DragonFiesta.World.Networking
{
    public class WorldClientManager : ClientManager
    {
        #region .ctor
        public WorldClientManager() : base()
        {
            
        }
        #endregion
        #region Properties

        public new static WorldClientManager Instance { get; private set; }
        #endregion
        #region Methods

        public static bool Initialize()
        {
            try
            {
                Instance = new WorldClientManager();
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

        public override void RegisterClient(ClientBase pClient)
        {
            base.RegisterClient(pClient);
            ushort pos = (ushort)Program.Random.Next(0, 480);
            pClient.XorPosition = pos;
            SH2Methods.SetXorPosition(pClient, pos);
        }


        #endregion
    }
}