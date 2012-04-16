using System;
using System.Data;
namespace Zepheus.FiestaLib.Data
{
    public sealed class MobInfoServer
    {
        public UInt32 ID { get; private set; }
        public String InxName { get; private set; }
        public Byte Visible { get; private set; }
        public UInt16 AC { get; private set; }
        public UInt16 TB { get; private set; }
        public UInt16 MR { get; private set; }
        public UInt16 MB { get; private set; }
        public UInt32 EnemyDetectType { get; private set; }
        public UInt32 MobKillInx { get; private set; }
        public UInt32 MonExp { get; private set; }
        public UInt16 ExpRange { get; private set; }
        public UInt16 DetectCha { get; private set; }
        public Byte ResetInterval { get; private set; }
        public UInt16 CutInterval { get; private set; }
        public UInt32 CutNonAT { get; private set; }
        public UInt32 FollowCha { get; private set; }
        public UInt16 PceHPRcvDly { get; private set; }
        public UInt16 PceHPRcv { get; private set; }
        public UInt16 AtkHPRcvDly { get; private set; }
        public UInt16 AtkHPRcv { get; private set; }
        public UInt16 Str { get; private set; }
        public UInt16 Dex { get; private set; }
        public UInt16 Con { get; private set; }
        public UInt16 Int { get; private set; }
        public UInt16 Men { get; private set; }
        public UInt32 MobRaceType { get; private set; }
        public Byte Rank { get; private set; }
        public UInt32 FamilyArea { get; private set; }
        public UInt32 FamilyRescArea { get; private set; }
        public Byte FamilyRescCount { get; private set; }
        public UInt16 BloodingResi { get; private set; }
        public UInt16 StunResi { get; private set; }
        public UInt16 MoveSpeedResi { get; private set; }
        public UInt16 FearResi { get; private set; }
        public String ResIndex { get; private set; }
        public UInt16 KQKillPoint { get; private set; }
        public Byte Return2Regen { get; private set; }
        public Byte IsRoaming { get; private set; }
        public Byte RoamingNumber { get; private set; }
        public UInt16 RoamingDistance { get; private set; }
        public UInt16 MaxSP { get; private set; }
        public Byte BroadAtDead { get; private set; }
        public UInt16 TurnSpeed { get; private set; }
        public UInt16 WalkChase { get; private set; }
        public Byte AllCanLoot { get; private set; }
        public UInt16 DmgByHealMin { get; private set; }
        public UInt16 DmgByHealMax { get; private set; }

        public static MobInfoServer Load(DataRow row)
        {
            MobInfoServer info = new MobInfoServer
            {
                ID = (uint)row["ID"],
                InxName = (string)row["InxName"],
                Visible = (byte)row["Visible"],
                AC = (ushort)row["AC"],
                TB = (ushort)row["TB"],
                MR = (ushort)row["MR"],
                MB = (ushort)row["MB"],
                EnemyDetectType = (uint)row["EnemyDetectType"],
                MobKillInx = (uint)row["MobKillInx"],
                MonExp = (uint)row["MonEXP"],
                ExpRange = (ushort)row["EXPRange"],
                DetectCha = (ushort)row["DetectCha"],
                ResetInterval =(byte)row["ResetInterval"],
                CutInterval = (ushort)row["CutInterval"],
                CutNonAT = (uint)row["CutNonAT"],
                FollowCha = (uint)row["FollowCha"],
                PceHPRcvDly = (ushort)row["PceHPRcvDly"],
                PceHPRcv = (ushort)row["PceHPRcv"],
                AtkHPRcvDly = (ushort)row["AtkHPRcvDly"],
                AtkHPRcv = (ushort)row["AtkHPRcv"],
                Str = (ushort)row["Str"],
                Dex = (ushort)row["Dex"],
                Con = (ushort)row["Con"],
                Int = (ushort)row["Int"],
                Men = (ushort)row["Men"],
                MobRaceType = (uint)row["MobRaceType"],
                Rank = (byte)row["Rank"],
                FamilyArea = (uint)row["FamilyArea"],
                FamilyRescArea = (uint)row["FamilyRescArea"],
                FamilyRescCount = (byte)row["FamilyRescCount"],
                BloodingResi = (ushort)row["BloodingResi"],
                StunResi = (ushort)row["StunResi"],
                MoveSpeedResi = (ushort)row["MoveSpeedResi"],
                FearResi = (ushort)row["FearResi"],
                ResIndex = (string)row["ResIndex_"],
                KQKillPoint = (ushort)row["KQKillPoint"],
                Return2Regen = (byte)row["Return2Regen"],
                IsRoaming = (byte)row["IsRoaming"],
                RoamingNumber = (byte)row["RoamingNumber"],
                RoamingDistance = (ushort)row["RoamingDistance"],
                MaxSP = (ushort)row["MaxSP"],
                BroadAtDead = (byte)row["BroadAtDead"],
                TurnSpeed = (ushort)row["TurnSpeed"],
                WalkChase = (ushort)row["WalkChase"],
                AllCanLoot = (byte)row["AllCanLoot"],
                DmgByHealMin =(ushort)row["DmgByHealMin"],
                DmgByHealMax = (ushort)row["DmgByHealMax"],
            };
            return info;
        }
    }
}
