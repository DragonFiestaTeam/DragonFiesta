using System;
using System.Collections.Generic;
using DragonFiesta.Data;
using DragonFiesta.FiestaLib;

namespace DragonFiesta.DataProvider
{
    public partial class DataProvider
    {
  
        public Map GetMap(ushort MapID)
        {
            Map pMap;
            this.maps.TryGetValue(MapID, out pMap);
            return pMap;
        }

        public bool GetMap(ushort MapID, out Map map)
        {
            map = null;
            if (!this.maps.TryGetValue(MapID, out map))
                return false;

            return true;
        }

        #region Character

        public ulong GetExpForLevel(ClassID pClass, byte Level)
        {
            List<CharacterClass> Stats = new List<CharacterClass>();
            if(this.CharacterBaseStats.TryGetValue(pClass,out Stats))
            {
                CharacterClass my = Stats.Find(m => m.Level == Level);
                if (my != null)
                {
                    return my.EXP;
                }
                return ulong.MaxValue;
            }
            return ulong.MaxValue;
        }

        public CharacterStats GetClassStats(ClassID pClass,byte Level)
        {
            List<CharacterClass> Stats = new List<CharacterClass>();
            if (this.CharacterBaseStats.TryGetValue(pClass, out Stats))
            {
                CharacterStats my = Stats.Find(m => m.Level == Level).ClassStats;
                if (my != null)
                {
                    return my;
                }
                return null;
            }
            return null;
        }

        public StonePowerInfo GetStoneInfo(byte Level, ClassID CharacterClass)
        {
            return this.StoneList.Find(m => m.Level == Level && m.pClass == CharacterClass);
        }

        #endregion

    }
}
