using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragonFiestaDatatbaseConverter.Data;
using DragonFiestaDatatbaseConverter.SHN;
using DragonFiestaDatatbaseConverter.ShineTable;
using System.IO;

namespace DragonFiestaDatatbaseConverter.Handlers
{
  public  class ConvertMaps
    {
        public static Dictionary<ushort, MapInfo> MapsByID { get; private set; }
        public static Dictionary<string, MapInfo> MapsByName { get; private set; }
        public static void LoadMaps()
        {
         MapsByID = new Dictionary<ushort, MapInfo>();
          MapsByName = new Dictionary<string, MapInfo>();
            using (var file = new SHNFile(Program.folder + @"\MapInfo.shn"))
            {
                if (Database.DatabaseHelper.Instance.runSQL("CREATE TABLE `mapinfo` (`ID` int(11) NOT NULL,`ShortName` varchar(255) NOT NULL,`RegenX` int(11) NOT NULL,`RegenY` int(11) NOT NULL,`KingdomMap` tinyint(4) NOT NULL,`ViewRange` int(11) NOT NULL,PRIMARY KEY (`ID`))"))
                {
                    using (DataTableReaderEx reader = new DataTableReaderEx(file))
                    {
                        while (reader.Read())
                        {
                            MapInfo info = MapInfo.Load(reader);
                            info.NPCs = new List<ShineNPC>();
                            lock(reader)
                            {
                            if (Database.DatabaseHelper.Instance.runSQL("INSERT INTO MapInfo (ID,ShortName,RegenX,RegenY,KingdomMap,ViewRange) VALUES ('" + info.ID + "','" + info.ShortName + "','" + info.RegenX + "','" + info.RegenY + "','" + info.Kingdom + "','" + info.ViewRange + "');"))
                            {
                                MapsByID.Add(info.ID, info);
                                MapsByName.Add(info.ShortName, info);
                            }
                        }

                        }
                    }
                }
            }

            Dictionary<ushort, BlockInfo> Blocks = new Dictionary<ushort, BlockInfo>();
            if (Database.DatabaseHelper.Instance.runSQL("CREATE TABLE `blockinfo` (`MapID` int(11) NOT NULL,`Height` int(11) NOT NULL,  `Width` int(11) NOT NULL,`ShortName` varchar(255) NOT NULL,  PRIMARY KEY (`MapID`)) "))
            {
                foreach (var map in MapsByID.Values)
                {
                    string renderpath = Program.folder + @"\BlockInfo\" + map.ShortName + ".shbd";
                    if (File.Exists(renderpath))
                    {
                        BlockInfo info = new BlockInfo(renderpath, map.ID);
                        Database.DatabaseHelper.Instance.runSQL("INSERT BlockInfo (MapID,Height,Width,ShortName) VALUES ('" + info.MapID + "','" + info.Height + "','" + info.Width + "','" + info.ShortName + "');");

                    }
                }
            }
            using (var tables = new ShineReader(Program.folder + @"\NPC.txt"))
            {
                if (Database.DatabaseHelper.Instance.runSQL("CREATE TABLE `linktable` (`argument` varchar(255) NOT NULL,`MapServer` varchar(255) NOT NULL,`MapClient` varchar(255) NOT NULL,`Coord_X` int(32) NOT NULL,`Coord_Y` int(32) NOT NULL,`Direct` int(11) NOT NULL,`LevelFrom` int(11) NOT NULL,`LevelTo` int(11) NOT NULL,`Party` smallint(3) NOT NULL,PRIMARY KEY (`argument`))"))
                {
                Dictionary<string, LinkTable> NpcLinkTable = new Dictionary<string, LinkTable>();
                using (DataTableReaderEx reader = new DataTableReaderEx(tables["LinkTable"]))
                {
                        while (reader.Read())
                        {
                            LinkTable link = LinkTable.Load(reader);

                            Database.DatabaseHelper.Instance.runSQL("INSERT INTO LinkTable (argument,MapServer,MapClient,Direct,LevelFrom,LevelTo,Party,Coord_Y,Coord_X) VALUES ('" + link.argument + "','" + link.MapServer + "','" + link.MapClient + "','" + link.Direct + "','" + link.LevelFrom + "','" + link.LevelTo + "','" + link.Party + "','" + link.Coord_Y + "'," + link.Coord_X + ");");
                        }
                }
                }
            
                if(Database.DatabaseHelper.Instance.runSQL("CREATE TABLE `ShineNPC` ( `ID` int(11) NOT NULL,`ShortName` varchar(255) NOT NULL,`RegenX` int(11) NOT NULL,`RegenY` int(11) NOT NULL,`KingdomMap` tinyint(4) NOT NULL,`ViewRange` int(11) NOT NULL);"))
                using (DataTableReaderEx reader = new DataTableReaderEx(tables["ShineNPC"]))
                {
                    while (reader.Read())
                    {
                        ShineNPC npc = ShineNPC.Load(reader);
                        MapInfo mi = null;
                        if (MapsByName.TryGetValue(npc.Map, out mi))
                        {
                            
                            Database.DatabaseHelper.Instance.runSQL("INSERT INTO ShineNPC (ID,ShortName,RegenX,RegenY,KingdomMap,ViewRange) VALUES ('" + mi.ID + "','" + mi.ShortName + "','" + mi.RegenX + "','" + mi.RegenY + "','" + mi.Kingdom + "','" + mi.ViewRange + "');");
                            mi.NPCs.Add(npc);
                        }
                    }
                }
            }

            Log.WriteLine(LogLevel.Info, "Loaded {0} maps.", MapsByID.Count);
        }
        public ushort GetMapidFromMapShortName(string name)
        {
            MapInfo mi = null;
            if (MapsByName.TryGetValue(name, out mi))
            {
                return mi.ID;
            }
            return 0;
        }

        public string GetMapShortNameFromMapid(ushort id)
        {
            MapInfo mi = null;
            if (MapsByID.TryGetValue(id, out mi))
            {
                return mi.ShortName;
            }
            return "";
        }
    }
}
