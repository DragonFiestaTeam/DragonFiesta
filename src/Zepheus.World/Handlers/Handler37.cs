using System;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.World.Networking;

namespace Zepheus.World.Handlers
{
    public sealed class Handler37
    {
        [PacketHandler(CH37Type.MasterRequest)]
        public static void MasterRequest(WorldClient client, Packet packet)
        {
            string playername = string.Empty;
            string target = string.Empty;
            if (!packet.TryReadString(out playername, 16) && !packet.TryReadString(out target, 16))
                return;
            MasterManager.Instance.AddMasterRequest(client, target);

        }
    }
}
