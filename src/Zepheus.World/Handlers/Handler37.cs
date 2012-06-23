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
        }
    }
}
