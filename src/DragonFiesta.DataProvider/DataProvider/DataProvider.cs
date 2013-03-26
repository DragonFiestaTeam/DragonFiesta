using System;
using DragonFiesta.FiestaLib;
using DragonFiesta.Database;
using DragonFiesta.Util;
using DragonFiesta.Data;
using System.Collections.Generic;

namespace DragonFiesta.DataProvider
{
    public partial  class DataProvider
    {
        public DatabaseManager GameDB { get; set; }

        public static DataProvider Instance { get; set; }

        public DataProvider(DatabaseManager pManager)
        {
            this.GameDB = pManager;
            Setup();
        }

        public static bool Initialize(DatabaseManager pManager)
        {
            try
            {
                Instance = new DataProvider(pManager);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed Load DataProvider {0}", ex);
                return false;
            }
        }

        private void Setup()
        {
            LoadMaps();
            LoadStoneByList();
            LoadCharacterBaseStats();
        }

        #region Setup

        private void LoadStoneByList()
        {
            using (DatabaseClient pClient = this.GameDB.GetClient())
            {
                SQLResult pResult = pClient.Select(Query.GetStoneList);

                for (int r = 0; r < pResult.Count; r++)
                {
                    StonePowerInfo Stone = new StonePowerInfo
                    {
                        pClass = (ClassID)pResult.Read<Byte>(r, "Class"),
                        Level = pResult.Read<Byte>(r, "Level"),
                        SPStoneEffectID = pResult.Read<Int32>(r, "SPStoneEffectID"),
                        HPStoneEffectID = pResult.Read<Int32>(r, "SPStoneEffectID"),
                        HPStonePrice = pResult.Read<Int64>(r, "HPStonePrice"),
                        SPStonePrice = pResult.Read<Int64>(r, "SPStonePrice"),
                        SoulSP = pResult.Read<Int32>(r, "SoulSP"),
                        SoulHP = pResult.Read<Int32>(r, "SoulHP"),
                        SPMaxCount = pResult.Read<Int32>(r, "SPMaxCount"),
                        HPMaxCount = pResult.Read<Int32>(r, "HPMaxCount"),
                    };
                    this.StoneList.Add(Stone);
                }
                Console.WriteLine("Loaded {0} StoneInfos", this.StoneList.Count);
            }
        }

        private void LoadMaps()
        {
            using (DatabaseClient pClient = this.GameDB.GetClient())
            {
                SQLResult pResult = pClient.Select(Query.GetAllMaps);

                for (int r = 0; r < pResult.Count; r++)
                {
                    Map map = new Map();
                    map.ReadMapFromDatabase(pResult, r);
                    this.maps.Add(map.ID, map);
                }
            }
            Console.WriteLine("Loaded {0} maps", this.maps.Count);
        }

        private void LoadCharacterBaseStats()
        {
            #region ClassInit
            this.CharacterBaseStats.Add(ClassID.Archer, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Cleric, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.CleverFighter, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Enchanter, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Fighter, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Gambit, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Gladiator, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Guardian, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.HawkArcher, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.HighCleric, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.HolyKnight, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Knight, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Mage, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Paladin, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Ranger, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Reaper, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Renegade, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Scout, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.SharpShooter, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Spectre, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Trickster, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Warlock, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Warrior, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.Wizard, new List<CharacterClass>());
            this.CharacterBaseStats.Add(ClassID.WizMage, new List<CharacterClass>());
            #endregion
            using (DatabaseClient pClient = this.GameDB.GetClient())
            {
             foreach(var ClassStats in this.CharacterBaseStats)
             {
                SQLResult pResult = pClient.Select(Query.GetBaseStats,ClassStats.Key);
                for (int r = 0; r < pResult.Count; r++)
                {
                    CharacterClass BaseStats = new CharacterClass
                    {
                        pClassID = ClassStats.Key,
                        Level = pResult.Read<Byte>(r, "Level"),
                        EXP = pResult.Read<UInt64>(r, "Exp")
                    };
                    BaseStats.ClassStats = new CharacterStats
                    {
                        MaxHP = pResult.Read<Int32>(r, "HP"),
                        MaxSP = pResult.Read<Int32>(r, "SP"),
                        STR = pResult.Read<Int32>(r, "Str"),
                        INT = pResult.Read<Int32>(r, "Int"),
                        END = pResult.Read<Int32>(r, "End"),
                        DEX = pResult.Read<Int32>(r, "Dex"),
                        SPR = pResult.Read<Int32>(r, "Spr"),
                        AIM = pResult.Read<Int32>(r, "AIM"),
                        DODGE = pResult.Read<Int32>(r, "Dodge"),
                        DEF = pResult.Read<Int32>(r, "Def"),
                        MagicDef = pResult.Read<Int32>(r, "MagicDef"),
                        MinDamage = pResult.Read<Int32>(r, "MinDamage"),
                        MaxDamage = pResult.Read<Int32>(r, "MaxDamage"),
                        MinMagicDamage = pResult.Read<Int32>(r, "MinMagicDamage"),
                        MaxMagicDamage = pResult.Read<Int32>(r, "MaxMagicDamage"),
                    };
                    StonePowerInfo power = this.GetStoneInfo(BaseStats.Level, BaseStats.pClassID);
                    if (power != null)
                    {
                        BaseStats.StonesPowers = power;
                        ClassStats.Value.Add(BaseStats);
                    }
                    else
                        Console.WriteLine("warning can not load StoneInfo {0} {1}", BaseStats.Level, BaseStats.pClassID);
                }
                Console.WriteLine("Loaded {0} BaseStats by ClassID {1}", ClassStats.Value.Count,ClassStats.Key);
             }
            }
 
        }
        #endregion
    }
}
