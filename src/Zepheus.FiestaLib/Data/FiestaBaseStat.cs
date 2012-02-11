using System;
using Zepheus.Util;
using System.Data;

namespace Zepheus.FiestaLib.Data
{
    public sealed class FiestaBaseStat
    {
        public Job Job { get; private set; }
        public Int32 Level { get; private set; }
        public Int32 Strength { get; private set; }
        public Int32 Endurance { get; private set; }
        public Int32 Intelligence { get; private set; }
        public Int32 Dexterity { get; private set; }
        public Int32 Spirit { get; private set; }
        public Int32 SoulHP { get; private set; }
        public Int32 MAXSoulHP { get; private set; }
        public Int32 PriceHPStone { get; private set; }
        public Int32 SoulSP { get; private set; }
        public Int32 MAXSoulSP { get; private set; }
        public Int32 PriceSPStone { get; private set; }
        public Int32 AtkPerAP { get; private set; }
        public Int32 DmgPerAP { get; private set; }
        public Int32 MaxPwrStone { get; private set; }
        public Int32 NumPwrStone { get; private set; }
        public Int32 PricePwrStone { get; private set; }
        public Int32 PwrStoneWC { get; private set; }
        public Int32 PwrStoneMA { get; private set; }
        public Int32 MaxGrdStone { get; private set; }
        public Int32 NumGrdStone { get; private set; }
        public Int32 PriceGrdStone { get; private set; }
        public Int32 GrdStoneAC { get; private set; }
        public Int32 GrdStoneMR { get; private set; }
        public Int32 PainRes { get; private set; }
        public Int32 RestraintRes { get; private set; }
        public Int32 CurseRes { get; private set; }
        public Int32 ShockRes { get; private set; }
        public UInt32 MaxHP { get; private set; }
        public UInt32 MaxSP { get; private set; }
        public Int32 CharTitlePt { get; private set; }
        public Int32 SkillPwrPt { get; private set; }

        public static FiestaBaseStat Load(DataRow Row, Job job)
        {
            FiestaBaseStat info = new FiestaBaseStat
            {
                Job = job,
                Level = (int)Row["Level"],
                Strength = (int)Row["Strength"],
                Endurance = (int)Row["Constitution"],
                Intelligence = (int)Row["Intelligence"],
                Dexterity = (int)Row["Dexterity"],
                Spirit = (int)Row["MentalPower"],
                SoulHP = (int)Row["SoulHP"],
                MAXSoulHP = (int)Row["MAXSoulHP"],
                PriceHPStone = (int)Row["PriceHPStone"],
                SoulSP = (int)Row["SoulSP"],
                MAXSoulSP = (int)Row["MAXSoulSP"],
                PriceSPStone = (int)Row["PriceSPStone"],
                AtkPerAP = (int)Row["AtkPerAP"],
                DmgPerAP = (int)Row["DmgPerAP"],
                MaxPwrStone = (int)Row["MaxPwrStone"],
                NumPwrStone = (int)Row["NumPwrStone"],
                PricePwrStone = (int)Row["PricePwrStone"],
                PwrStoneWC = (int)Row["PwrStoneWC"],
                PwrStoneMA = (int)Row["PwrStoneMA"],
                MaxGrdStone = (int)Row["MaxGrdStone"],
                NumGrdStone = (int)Row["NumGrdStone"],
                PriceGrdStone = (int)Row["PriceGrdStone"],
                GrdStoneAC = (int)Row["GrdStoneAC"],
                GrdStoneMR = (int)Row["GrdStoneMR"],
                PainRes = (int)Row["PainRes"],
                RestraintRes = (int)Row["RestraintRes"],
                CurseRes = (int)Row["CurseRes"],
                ShockRes = (int)Row["ShockRes"],
                MaxHP = (ushort)(int)Row["MaxHP"],
                MaxSP = (ushort)(int)Row["MaxSP"],
                CharTitlePt = (int)Row["CharTitlePt"],
                SkillPwrPt = (int)Row["SkillPwrPt"],
            };
           
            return info;
        }
    }
}
