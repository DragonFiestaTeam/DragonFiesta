using System;
using System.Xml.Serialization;
using System.IO;

using Zepheus.Util;

namespace Zepheus.FiestaLib.Data
{
	//to update player stats, have to find out more later
	public sealed class BaseStats
    {
        /*
         * 
        public static int GetStatValue(WorldCharacter pCharacter, StatsByte pByte)
        {
            switch (pByte)
            {
                case StatsByte.MinMelee:
                    return pCharacter.MinDamage;
                case StatsByte.MaxMelee:
                    return pCharacter.MaxDamage;
                case StatsByte.MinMagic:
                    return pCharacter.MinMagic;
                case StatsByte.MaxMagic:
                    return pCharacter.MaxMagic;
                case StatsByte.WDef:
                    return pCharacter.WeaponDef;
                case StatsByte.MDef:
                    return pCharacter.MagicDef;
                case StatsByte.Aim:
                    return 5; //TODO load additional equip stats
                case StatsByte.Evasion:
                    return 5;
                case StatsByte.StrBonus:
                    return pCharacter.StrBonus;
                case StatsByte.EndBonus:
                    return pCharacter.EndBonus;
                default:
                    return 0;
            }
        }
        */

        public BaseStatsEntry this[byte level] {
            get
            {
                if (Entries.ContainsKey(level))
                    return Entries[level];
                else return null;
            }
        }
        public Job Job { get; set; }
        public readonly SerializableDictionary<byte, BaseStatsEntry> Entries = new SerializableDictionary<byte, BaseStatsEntry>();

        public BaseStats()
        {

        }

        public BaseStats(Job pJob)
        {
            this.Job = pJob;
        }

        public bool GetEntry(byte pLevel, out BaseStatsEntry pEntry)
        {
            return this.Entries.TryGetValue(pLevel, out pEntry);
        }

        public static bool TryLoad(string pFile, out BaseStats pStats)
        {
            pStats = new BaseStats();
            try
            {
                using (var file = File.Open(pFile, FileMode.Open))
                {
                    XmlSerializer xser = new XmlSerializer(typeof(BaseStats));
                    pStats = (BaseStats)xser.Deserialize(file);
                   // Log.WriteLine(LogLevel.Info, "Job {0} loaded! Data for {1} levels.", pStats.Job.ToString(), pStats.entries.Count);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, "Exception while loading stats from job {0}: {1}", pFile, ex.ToString());
                return false;
            }
        }
    }
}
