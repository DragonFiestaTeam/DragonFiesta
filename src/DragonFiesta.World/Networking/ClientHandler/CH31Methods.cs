using System;
using DragonFiesta.Networking;
using DragonFiesta.World.Networking;
using DragonFiesta.Networking.Handler.Client;
using DragonFiesta.Util;
using DragonFiesta.FiestaLib;
using DragonFiesta.Data;
using DragonFiesta.Data.Transfer;
using DragonFiesta.World.Networking.ServerHandler;
using DragonFiesta.World.Database;
using DragonFiesta.World.Game;
using DragonFiesta.World.Core;
using DragonFiesta.Messages.Zone;

namespace DragonFiesta.World.Networking.ClientHandler
{
    [PacketHandlerClass(CH31Type._Header)]
    public static class CH31Methods
    {
        [PacketHandler(CH31Type.GetUnk)]
        public static void UnknownRequest(WorldClient client, Packet packet)
        {
           SH31Methods.SendUnknown(client);
        }
    }
}
