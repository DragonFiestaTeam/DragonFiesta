using System;
using Zepheus.World.Data;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;

namespace Zepheus.World.Managers
{
    public class BroadcastManager
    {
        public static BroadcastManager Instance { get; set; }

        [InitializerMethod]
        public static bool init()
        {
            Instance = new BroadcastManager();
            return true;
        }
        public void BroadcastInRange(WorldCharacter pCahr, Packet pPacket, bool ToAll)
        {
            // todo
        }
    }
}
