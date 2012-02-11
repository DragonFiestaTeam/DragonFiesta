using System;
using Zepheus.Util;
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
        public UInt32 MonEXP { get; private set; }
        public UInt16 EXPRange { get; private set; }
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

        public static MobInfoServer Load(DataRow Row)
        {
            MobInfoServer info = new MobInfoServer
            {
                ID = (uint)Row["ID"],
                InxName = (string)Row["InxName"],
                Visible = (byte)Row["Visible"],
                AC = (ushort)Row["AC"],
                TB = (ushort)Row["TB"],
                MR = (ushort)Row["MR"],
                MB = (ushort)Row["MB"],
                EnemyDetectType = (uint)Row["EnemyDetectType"],
                MobKillInx = (uint)Row["MobKillInx"],
                MonEXP = (uint)Row["MonEXP"],
                EXPRange = (ushort)Row["EXPRange"],
                DetectCha = (ushort)Row["DetectCha"],
                ResetInterval =(byte)Row["ResetInterval"],
                CutInterval = (ushort)Row["CutInterval"],
                CutNonAT = (uint)Row["CutNonAT"],
                FollowCha = (uint)Row["FollowCha"],
                PceHPRcvDly = (ushort)Row["PceHPRcvDly"],
                PceHPRcv = (ushort)Row["PceHPRcv"],
                AtkHPRcvDly = (ushort)Row["AtkHPRcvDly"],
                AtkHPRcv = (ushort)Row["AtkHPRcv"],
                Str = (ushort)Row["Str"],
                Dex = (ushort)Row["Dex"],
                Con = (ushort)Row["Con"],
                Int = (ushort)Row["Int"],
                Men = (ushort)Row["Men"],
                MobRaceType = (uint)Row["MobRaceType"],
                Rank = (byte)Row["Rank"],
                FamilyArea = (uint)Row["FamilyArea"],
                FamilyRescArea = (uint)Row["FamilyRescArea"],
                FamilyRescCount = (byte)Row["FamilyRescCount"],
                BloodingResi = (ushort)Row["BloodingResi"],
                StunResi = (ushort)Row["StunResi"],
                MoveSpeedResi = (ushort)Row["MoveSpeedResi"],
                FearResi = (ushort)Row["FearResi"],
                ResIndex = (string)Row["ResIndex"],
                KQKillPoint = (ushort)Row["KQKillPoint"],
                Return2Regen = (byte)Row["Return2Regen"],
                IsRoaming = (byte)Row["IsRoaming"],
                RoamingNumber = (byte)Row["RoamingNumber"],
                RoamingDistance = (ushort)Row["RoamingDistance"],
                MaxSP = (ushort)Row["MaxSP"],
                BroadAtDead = (byte)Row["BroadAtDead"],
                TurnSpeed = (ushort)Row["TurnSpeed"],
                WalkChase = (ushort)Row["WalkChase"],
                AllCanLoot = (byte)Row["AllCanLoot"],
                DmgByHealMin =(ushort)Row["DmgByHealMin"],
                DmgByHealMax = (ushort)Row["DmgByHealMax"],
            };
            return info;
        }
    }
}
