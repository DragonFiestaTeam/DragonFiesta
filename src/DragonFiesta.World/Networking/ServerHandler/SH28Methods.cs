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
using DragonFiesta.Networking.Handler.Server;

namespace DragonFiesta.World.Networking.ServerHandler
{
    public class SH28Methods
    {
        public static void SendShortcuts(WorldClient client)
        {
            using (var packet = new Packet(SH28Type._Header,SH28Type.LoadShortCuts))
            {

                packet.WriteHexAsBytes("01 30");
               // byte[] data = client.pCharacter.Shortcuts;
               /* bool hasData = data != null;
                packet.WriteBool(hasData);
                packet.WriteBytes(hasData ? data : new byte[] { 0 });*/
                client.SendPacket(packet);
            }
        }

        public static void SendGameSettings(WorldClient client)
        {
            using (var packet = new Packet(SH28Type._Header,SH28Type.LoadGameSettings))
            {
                packet.WriteHexAsBytes("01 30");
                client.SendPacket(packet);
            }
        }

        public static void SendClientSettings(WorldClient client)
        {
            using (var packet = new Packet(SH28Type._Header,SH28Type.LoadClientSettings))
            {
                packet.WriteHexAsBytes("01 30");
                client.SendPacket(packet);
            }
        }

        public static void SendQuickbar(WorldClient client)
        {
            using (var packet = new Packet(SH28Type._Header,SH28Type.LoadQuickBar))
            {
                packet.WriteHexAsBytes("01 30");
                client.SendPacket(packet);
            }
        }

        public static void SendQuickbarState(WorldClient client)
        {
            using (var packet = new Packet(SH28Type._Header,SH28Type.LoadQuickBarState))
            {
                packet.WriteHexAsBytes("01 30");

                client.SendPacket(packet);
            }
        }
    }
}
