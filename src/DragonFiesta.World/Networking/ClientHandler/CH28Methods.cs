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
   [PacketHandlerClass(CH28Type._Header)]
    public static class CH28Methods
    {
        [PacketHandler(CH28Type.GetQuickBar)]
        public static void QuickBarRequest(WorldClient client, Packet packet)
        {
         SH28Methods.SendQuickbar(client);
        }

        [PacketHandler(CH28Type.GetQuickBarState)]
        public static void QuickBarStateRequest(WorldClient client, Packet packet)
        {
            SH28Methods.SendQuickbarState(client);
        }

        [PacketHandler(CH28Type.GetClientSettings)]
        public static void ClientSettingsRequest(WorldClient client, Packet packet)
        {
            SH28Methods.SendClientSettings(client);
        }


        [PacketHandler(CH28Type.GetGameSettings)]
        public static void GameSettingsRequest(WorldClient client, Packet packet)
        {
            SH28Methods.SendGameSettings(client);
        }

        [PacketHandler(CH28Type.GetShortCuts)]
        public static void ShortcutsRequest(WorldClient client, Packet packet)
        {
            SH28Methods.SendShortcuts(client);
        }

        [PacketHandler(CH28Type.SaveQuickBar)]
        public static void SaveQuickBarRequest(WorldClient client, Packet packet)
        {
            // Load up 1 KB of data (well, try to)
            byte[] data;
            if (!packet.TryReadBytes(1024, out data))
            {
                Logs.Main.Warn("Unable to read 1024 bytes from stream for save");
                return;
            }

            // Save it.
            client.pCharacter.SetQuickBarData(data);
        }

        [PacketHandler(CH28Type.SaveGameSettings)]
        public static void SaveGameSettingsRequest(WorldClient client, Packet packet)
        {
            // Load up 64 B of data (well, try to)
            byte[] data;
            if (!packet.TryReadBytes(64, out data))
            {
                Logs.Main.Warn("Unable to read 64 bytes from stream for save");
                return;
            }

            // Save it.
            client.pCharacter.SetGameSettingsData(data);
        }

        [PacketHandler(CH28Type.SaveClientSettings)]
        public static void SaveClientSettingsRequest(WorldClient client, Packet packet)
        {
            byte[] data;
            if (!packet.TryReadBytes(392, out data))
            {
                Logs.Main.Warn("Unable to read 392 bytes from stream for save");
                return;
            }

            // Save it.
            client.pCharacter.SetClientSettingsData(data);
        }

       [PacketHandler(CH28Type.SaveQuickBarState)]
        public static void SaveQuickBarStateRequest(WorldClient client, Packet packet)
        {
            byte[] data;
            if (!packet.TryReadBytes(24, out data))
            {
                Logs.Main.Warn("Unable to read 24 bytes from stream for save");
                return;
            }

            // Save it.
            client.pCharacter.SetQuickBarStateData(data);
        }

       [PacketHandler(CH28Type.SaveShortcuts)]
        public static void SaveShortCutsRequest(WorldClient client, Packet packet)
        {
            byte[] data;
            if (!packet.TryReadBytes(308, out data))
            {
                Logs.Main.Warn("Unable to read 308 bytes from stream for save");
                return;
            }

            // Save it.
            client.pCharacter.SetShortcutsData(data);
        }
    }
}
