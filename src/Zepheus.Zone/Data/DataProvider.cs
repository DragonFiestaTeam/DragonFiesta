using System;
using System.Collections.Generic;
using System.IO;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Data;
using Zepheus.FiestaLib.ShineTable;
using Zepheus.FiestaLib.SHN;
using Zepheus.Util;
using Zepheus.Database;
using MySql.Data.MySqlClient;
using System.Data;

namespace Zepheus.Zone.Data
{
    [ServerModule(Util.InitializationStage.SpecialDataProvider)]
    public sealed class DataProvider
    {
        public Dictionary<ushort, MapInfo> MapsByID { get; private set; }
        public Dictionary<string, MapInfo> MapsByName { get; private set; }
        public Dictionary<string, LinkTable> NpcLinkTable { get; private set; }
        public Dictionary<string, MobInfoServer> MobData { get; private set; }
        public Dictionary<ushort, BlockInfo> Blocks { get; private set; }
        public Dictionary<Job, List<FiestaBaseStat>> JobInfos { get; private set; }
        public Dictionary<ushort, ItemInfo> ItemsByID { get; private set; }
        public Dictionary<string, ItemInfo> ItemsByName { get; private set; }
        public Dictionary<string, DropGroupInfo> DropGroups { get; private set; }
        public Dictionary<ushort, MobInfo> MobsByID { get; private set; }
        public Dictionary<string, MobInfo> MobsByName { get; private set; }
        public Dictionary<ushort, ItemUseEffectInfo> ItemUseEffects { get; private set; }
        public Dictionary<string, RecallCoordinate> RecallCoordinates { get; private set; }
        public Dictionary<byte, ulong> ExpTable { get; private set; }
        public Dictionary<ushort, MiniHouseInfo> MiniHouses { get; private set; }

        public Dictionary<ushort, ActiveSkillInfo> ActiveSkillsByID { get; private set; }
        public Dictionary<string, ActiveSkillInfo> ActiveSkillsByName { get; private set; }
        public static DataProvider Instance { get; private set; }

        public DataProvider()
        {
            //LoadMaps();
            LoadMaps(null); //this loads all the maps, but we get issues with zone spread (fix later)
            LoadJobStats();
            LoadExpTable();
            LoadItemInfo();
            //LoadRecallCoordinates();
            LoadMobs();
            LoadDrops();
            LoadItemInfoServer();
            LoadMiniHouseInfo();
            LoadActiveSkills();
            LaodVendors();
        }

        [InitializerMethod]
        public static bool Load()
        {
            try
            {

                Instance = new DataProvider();
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, "Error loading dataprovider: {0}", ex.ToString());
                return false;
            }
        }

        private static readonly string[] DropGroupNames = new string[] { "DropGroupA", "DropGroupB", "RandomOptionDropGroup" };
        private void LoadItemInfoServer()
        {
            try
            {
                DataTable ItemDataInf = null;
                using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
                {
                    ItemDataInf = dbClient.ReadDataTable("SELECT  *FROM data_iteminfoserver");
                }
                foreach(DataRow Row in ItemDataInf.Rows)
                {
                    ushort itemid = (ushort)Row["ID"];
                    ItemInfo item;
                    if (ItemsByID.TryGetValue(itemid, out item))
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            string groupname = (string)Row[DropGroupNames[i]];
                            if (groupname.Length > 2)
                            {
                                DropGroupInfo group;
                                if (DropGroups.TryGetValue(groupname, out group))
                                {
                                    group.Items.Add(item);
                                }
                                else
                                {
                                    Log.WriteLine(LogLevel.Warn, "{0} was assigned to unknown DropGroup {1}.", item.InxName, groupname);
                                }
                            }
                        }
                    }
                    else Log.WriteLine(LogLevel.Warn, "ItemInfoServer has obsolete item ID: {0}.", itemid);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, "Error loading ItemInfoServer.shn: {0}", ex);
            }
        }
        private void LoadDrops()
        {
            DropGroups = new Dictionary<string, DropGroupInfo>();
            try
            {
                DataTable dropgroupinfoData = null;
                DataTable itemdroptableData = null;
                using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
                {
                    dropgroupinfoData = dbClient.ReadDataTable("SELECT  *FROM dropgroupinfo");
                    itemdroptableData = dbClient.ReadDataTable("SELECT  *FROM itemdroptable");
                }
                if (dropgroupinfoData != null)
                {
                    foreach (DataRow Row in dropgroupinfoData.Rows)
                    {
                        DropGroupInfo info = DropGroupInfo.Load(Row);
                        if (DropGroups.ContainsKey(info.GroupID))
                        {
                            Log.WriteLine(LogLevel.Warn, "Duplicate DropGroup ID found: {0}.", info.GroupID);
                            continue;
                        }
                        DropGroups.Add(info.GroupID, info);
                    }
                }
                int dropcount = 0;
                if (itemdroptableData != null)
                {
                    foreach (DataRow Row in itemdroptableData.Rows)
                    {
                        string mobid = (string)Row["MobId"];
                        MobInfo mob;
                        if (MobsByName.TryGetValue(mobid, out mob))
                        {
                            mob.MinDropLevel = (byte)Row["MinLevel"];
                            mob.MaxDropLevel = (byte)Row["MaxLevel"];
                            string dropgroup = (string)Row["GroupID"];
                            if (dropgroup.Length <= 2) continue;
                            DropGroupInfo group;
                            if (DropGroups.TryGetValue(dropgroup, out group))
                            {
                                float rate = (float)Row["Rate"];
                                DropInfo info = new DropInfo(group, rate);
                                mob.Drops.Add(info);
                                ++dropcount;
                            }
                            else
                            {
                                //this seems to happen a lot so disable this for the heck of it.
                                Log.WriteLine(LogLevel.Warn, "Could not find DropGroup {0}.", dropgroup);
                            }
                        }
                        else Log.WriteLine(LogLevel.Warn, "Could not find mobname: {0} for drop.", mobid);
                    }
                }
                //first we load the dropgroups
                Log.WriteLine(LogLevel.Info, "Loaded {0} DropGroups, with {1} drops in total.", DropGroups.Count, dropcount);
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, "Error loading DropTable: {0}", ex);
            }
        }

        private void LoadActiveSkills()
        {
            ActiveSkillsByID = new Dictionary<ushort, ActiveSkillInfo>();
            ActiveSkillsByName = new Dictionary<string, ActiveSkillInfo>();
                  DataTable ActiveSkillData = null;
                using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
                {
                    ActiveSkillData = dbClient.ReadDataTable("SELECT  *FROM activeskill");
                }
                if (ActiveSkillData != null)
                {
                    foreach (DataRow Row in ActiveSkillData.Rows)
                    {
                        ActiveSkillInfo info = ActiveSkillInfo.Load(Row);
                        if (ActiveSkillsByID.ContainsKey(info.ID) || ActiveSkillsByName.ContainsKey(info.Name))
                        {

                            Log.WriteLine(LogLevel.Warn, "Duplicate ActiveSkill found: {0} ({1})", info.ID, info.Name);
                            continue;
                        }
                        ActiveSkillsByID.Add(info.ID, info);
                        ActiveSkillsByName.Add(info.Name, info);
                    }
                }
            Log.WriteLine(LogLevel.Info, "Loaded {0} ActiveSkills.", ActiveSkillsByID.Count);
        }

       /* private void LoadRecallCoordinates()
        {
            RecallCoordinates = new Dictionary<string, RecallCoordinate>();
            if (!File.Exists(folder + @"\RecallCoordinates.txt"))
            {
                Log.WriteLine(LogLevel.Warn, "Could not find RecallCoordinates.txt, return scrolls won't work.");
                return;
            }

            using (var data = new ShineReader(folder + @"\RecallCoordinates.txt"))
            {

                var recallData = data["RecallPoint"];

                using (var reader = new DataTableReaderEx(recallData))
                {
                    while (reader.Read())
                    {
                        var rc = RecallCoordinate.Load(reader);
                        RecallCoordinates.Add(rc.ItemIndex, rc);
                    }
                }

                Log.WriteLine(LogLevel.Info, "Loaded {0} recall coordinates.", RecallCoordinates.Count);
            }
        }*/
        private void LoadMobs()
        {
            try
            {
                MobsByID = new Dictionary<ushort, MobInfo>();
                MobsByName = new Dictionary<string, MobInfo>();
                DataTable createture_names = null;
                 DataTable creature_proto = null;
                using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
                {
                    createture_names = dbClient.ReadDataTable("SELECT  *FROM creature_names");
                    creature_proto = dbClient.ReadDataTable("SELECT  *FROM creature_proto");
                }
                if (createture_names != null)
                {
                    foreach (DataRow Row in createture_names.Rows)
                    {
                        MobInfo info = MobInfo.Load(Row);
                        if (MobsByID.ContainsKey(info.ID) || MobsByName.ContainsKey(info.Name))
                        {
                            Log.WriteLine(LogLevel.Warn, "Duplicate mob ID found in MobInfo.shn: {0}.", info.ID);
                            continue;
                        }
                        MobsByID.Add(info.ID, info);

                        MobsByName.Add(info.Name, info);
                    }
                }
                MobData = new Dictionary<string, MobInfoServer>();
                if (creature_proto != null)
                {
                    foreach (DataRow Row in creature_proto.Rows)
                    {
                        MobInfoServer info = MobInfoServer.Load(Row);
                        if (MobData.ContainsKey(info.InxName))
                        {
                            Log.WriteLine(LogLevel.Warn, "Duplicate mob ID found in MobInfoServer.shn: {0}.", info.InxName);
                            continue;
                        }
                        //  Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE Vendors SET NPCID='" + info.ID + "' WHERE NPCID='" + info.InxName + "'");
                        MobData.Add(info.InxName, info);
                    }
                }
                Log.WriteLine(LogLevel.Info, "Loaded {0} mobs.", MobsByID.Count);

            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, "Error loading MobInfo {0}", ex.Message);
            }
        }

        public MobInfo GetMobInfo(ushort id)
        {
            MobInfo toret;
            if (MobsByID.TryGetValue(id, out toret))
            {
                return toret;
            }
            else return null;
        }

        public ushort GetMobIDFromName(string name)
        {
            MobInfoServer mis = null;
            if (MobData.TryGetValue(name, out mis))
            {
                return (ushort)mis.ID;
            }
            return 0;
        }

        public FiestaBaseStat GetBaseStats(Job job, byte level)
        {
            return JobInfos[job][level - 1];
        }

        public void LoadJobStats()
        {
            LoadJobStatsNEW();
        }

        public void LoadJobStatsNEW()
        {
            // Temp set a dict for every job/filename
            Dictionary<string, Job> sj = new Dictionary<string, Job>();
            sj.Add("Archer", Job.Archer);
            sj.Add("Assassin", Job.Reaper);
            sj.Add("Chaser", Job.Gambit);
            sj.Add("Cleric", Job.Cleric);
            sj.Add("CleverFighter", Job.CleverFighter);
            sj.Add("Closer", Job.Spectre);
            sj.Add("Cruel", Job.Renegade);
            sj.Add("Enchanter", Job.Enchanter);
            sj.Add("Fighter", Job.Fighter);
            sj.Add("Gladiator", Job.Gladiator);
            sj.Add("Guardian", Job.Guardian);
            sj.Add("HawkArcher", Job.HawkArcher);
            sj.Add("HighCleric", Job.HighCleric);
            sj.Add("HolyKnight", Job.HolyKnight);
            sj.Add("Joker", Job.Trickster); // hah
            sj.Add("Knight", Job.Knight);
            sj.Add("Mage", Job.Mage);
            sj.Add("Paladin", Job.Paladin);
            sj.Add("Ranger", Job.Ranger);
            sj.Add("Scout", Job.Scout);
            sj.Add("SharpShooter", Job.SharpShooter);
            sj.Add("Warrock", Job.Warlock); // ITS A GAME. AND YOU LOST IT
            sj.Add("Warrior", Job.Warrior);
            sj.Add("Wizard", Job.Wizard);
            sj.Add("WizMage", Job.WizMage);

            // DAMN THATS A LONG LIST

            Log.WriteLine(LogLevel.Debug, "Trying to load {0} jobs.", sj.Count);
            JobInfos = new Dictionary<Job, List<FiestaBaseStat>>();

            foreach (var kvp in sj)
            {
                DataTable BaseData = null;
                using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
                {
                    BaseData = dbClient.ReadDataTable("SELECT  * FROm BaseStats WHERE Class='" + kvp.Value + "'");
                }
                if (BaseData != null)
                {
                    List<FiestaBaseStat> stats = new List<FiestaBaseStat>();
                    foreach (DataRow Row in BaseData.Rows)
                    {
                        stats.Add(FiestaBaseStat.Load(Row, kvp.Value));
                    }
                    JobInfos.Add(kvp.Value, stats);
                    Log.WriteLine(LogLevel.Debug, "Loaded {0} levels for job {1}", stats.Count, kvp.Value.ToString());
                }
            }
        }

        public void LoadItemInfo()
        {
            Dictionary<string, ItemUseEffectInfo> effectcache = new Dictionary<string, ItemUseEffectInfo>();
            ItemUseEffects = new Dictionary<ushort, ItemUseEffectInfo>();
            DataTable effectid = null;
            DataTable dataItem = null;
            using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
            {
                effectid = dbClient.ReadDataTable("SELECT  *FROM data_itemuseeffect");
                dataItem = dbClient.ReadDataTable("SELECT  *FROM data_iteminfo");
            }

            if (effectid != null)
            {
                foreach (DataRow Row in effectid.Rows)
                {
                    string inxname;
                    ItemUseEffectInfo info = ItemUseEffectInfo.Load(Row, out inxname);
                    effectcache.Add(inxname, info);
                }
            }

            ItemsByID = new Dictionary<ushort, ItemInfo>();
            ItemsByName = new Dictionary<string, ItemInfo>();
            if (dataItem != null)
            {
                foreach (DataRow Row in dataItem.Rows)
                {
                    ItemInfo info = ItemInfo.Load(Row);
                    if (ItemsByID.ContainsKey(info.ItemID) || ItemsByName.ContainsKey(info.InxName))
                    {
                        Log.WriteLine(LogLevel.Warn, "Duplicate item found ID: {0} ({1}).", info.ItemID, info.InxName);

                        continue;
                    }
                    ItemsByID.Add(info.ItemID, info);
                    ItemsByName.Add(info.InxName, info);

                    if (effectcache.ContainsKey(info.InxName))
                    {
                        if (info.Type != ItemType.Useable)
                        {
                            Log.WriteLine(LogLevel.Warn, "Invalid useable item: {0} ({1})", info.ItemID, info.InxName);
                            continue;
                        }
                        ItemUseEffectInfo effectinfo = effectcache[info.InxName];
                        effectinfo.ID = info.ItemID;
                        ItemUseEffects.Add(effectinfo.ID, effectinfo);
                    }
                }
            }
            effectcache.Clear();
            Log.WriteLine(LogLevel.Info, "Loaded {0} items.", ItemsByID.Count);
        }


        public ItemInfo GetItemInfo(ushort ID)
        {
            ItemInfo info;
            if (ItemsByID.TryGetValue(ID, out info))
            {
                return info;
            }
            else return null;
        }

        public void LoadExpTable()
        {
            try
            {

                ExpTable = new Dictionary<byte, ulong>();
                        DataTable dataItem = null;
                using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
                {
                    dataItem = dbClient.ReadDataTable("SELECT  *FROM expTable");
                }

                if (dataItem != null)
                {
                    foreach (DataRow Row in dataItem.Rows)
                    {
                        byte level = (byte)Row["Level"];
                        ulong exp = (ulong)Row["Exp"];
                        ExpTable.Add(level, exp);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, "Error loading ExpTable: {0}", ex.Message);

            }
        }
        public ulong GetMaxExpForLevel(byte pLevel)
        {
            ulong ret = 0;
            if (!ExpTable.TryGetValue(pLevel, out ret))
            {
                Log.WriteLine(LogLevel.Warn, "Something tried to get the amount of EXP for level {0} (which is higher than it's max, {1}). Please backtrace the calls to this function!", pLevel, ExpTable.Count);
                Log.WriteLine(LogLevel.Warn, Environment.StackTrace);
            }
            return ret;
        }

        public void LoadMaps(List<ushort> toload = null)
        {
            MapsByID = new Dictionary<ushort, MapInfo>();
            MapsByName = new Dictionary<string, MapInfo>();
            DataTable MapDataInf = null;
            DataTable LinktableData = null;
            DataTable ShineNpcData = null;
            using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
            {
                MapDataInf = dbClient.ReadDataTable("SELECT  *FROM mapinfo");
                LinktableData = dbClient.ReadDataTable("SELECT *FROM LinkTable");
                ShineNpcData = dbClient.ReadDataTable("SELECT *FROM ShineNpc");
            }
            if (MapDataInf != null)
            {
                foreach (DataRow Row in MapDataInf.Rows)
                {
                    MapInfo info = MapInfo.Load(Row);
                    info.NPCs = new List<ShineNPC>();
                    if (MapsByID.ContainsKey(info.ID))
                    {
                        Log.WriteLine(LogLevel.Debug, "Duplicate map ID {0} ({1})", info.ID, info.FullName);
                        MapsByID.Remove(info.ID);
                        MapsByName.Remove(info.ShortName);
                    }
                    if (toload == null || toload.Contains(info.ID))
                    {
                        MapsByID.Add(info.ID, info);
                        MapsByName.Add(info.ShortName, info);
                    }
                }
            }
            Blocks = new Dictionary<ushort, BlockInfo>();
            foreach (var map in MapsByID.Values)
            {
                DataTable BlockData = null;
                using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
                {
                    BlockData = dbClient.ReadDataTable("SELECT  *FROM blockinfo WHERE MapID='" + map.ID + "'");
                }
                if (BlockData != null)
                {
                    if (BlockData.Rows.Count > 0)
                    {
                        foreach (DataRow Row in BlockData.Rows)
                        {
                            BlockInfo info = new BlockInfo(Row, map.ID);

                            Blocks.Add(map.ID, info);
                        }
                    }
                    else
                    {
                        Log.WriteLine(LogLevel.Warn, "No BlockInfo for Map {0}", map.ShortName);
                    }
                }
            }
            NpcLinkTable = new Dictionary<string, LinkTable>();
            if (LinktableData != null)
            {
                foreach (DataRow Row in LinktableData.Rows)
                {
                    LinkTable link = LinkTable.Load(Row);
                    if (Program.IsLoaded(GetMapidFromMapShortName(link.MapClient)))
                    {
                        NpcLinkTable.Add(link.argument, link);
                    }
                }
            }
            if (ShineNpcData != null)
            {
                foreach (DataRow Row in ShineNpcData.Rows)
                {
                    ShineNPC npc = ShineNPC.Load(Row);
                    MapInfo mi = null;
                    if (Program.IsLoaded(GetMapidFromMapShortName(npc.Map)) && MapsByName.TryGetValue(npc.Map, out mi))
                    {

                        mi.NPCs.Add(npc);
                    }
                }
            }
            Log.WriteLine(LogLevel.Info, "Loaded {0} maps.", MapsByID.Count);
        }
        public void LaodVendors()
        {
            foreach (var Map in MapsByID.Values)
            {
                foreach (var Npc in Map.NPCs)
                {

                    if (Npc.Flags == 1)
                    {
                        DataTable VendorData = null;
                        using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
                        {
                            VendorData = dbClient.ReadDataTable("SELECT *FROM Vendors WHERE NPCID='" + Npc.MobID + "'");
                        }
                        if (VendorData != null)
                        {
                            foreach (DataRow Row in VendorData.Rows)
                            {
                                Vendor Vendor = new Vendor();
                                Vendor.ItemID = (ushort)Row["ItemID"];
                                Vendor.InvSlot = (byte)Row["InvSlot"];
                                ItemInfo Item;
                                if (ItemsByID.TryGetValue(Vendor.ItemID, out Item))
                                {
                                    Vendor.Item = Item;
                                    Vendor.VendorName = Npc.MobName;
                                    Npc.VendorItems.Add(Vendor);
                                }
                            }
                        }
                    }
                }
            }
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

        public string GetMapFullNameFromMapid(ushort id)
        {
            MapInfo mi = null;
            if (MapsByID.TryGetValue(id, out mi))
            {
                return mi.FullName;
            }
            return "";
        }
        public void LoadMiniHouseInfo()
        {
            MiniHouses = new Dictionary<ushort, MiniHouseInfo>();
            DataTable HouseData = null;
               using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
                {
                    HouseData = dbClient.ReadDataTable("SELECT *FROM minihouse");
                }
               if (HouseData != null)
               {
                   foreach (DataRow Row in HouseData.Rows)
                   {
                       MiniHouseInfo mhi = new MiniHouseInfo(Row);
                       MiniHouses.Add(mhi.ID, mhi);
                   }
               }
            Log.WriteLine(LogLevel.Info, "Loaded {0} Mini Houses.", MiniHouses.Count);
        }
    }
       
}
