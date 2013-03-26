using System.Data;

namespace Zepheus.FiestaLib.Data
{
    public sealed class ActiveSkillInfo
    {
        public ushort ID { get; private set; }
        public string Name { get; private set; }
        public byte Step { get; private set; }
        public string Required { get; private set; }
        public ushort SP { get; private set; }
        public ushort HP { get; private set; }
        public ushort Range { get; private set; }
        public uint CoolTime { get; private set; }
        public uint CastTime { get; private set; }
        public ushort SkillAniTime { get; set; }
        public ushort MinDamage { get; private set; }
        public ushort MaxDamage { get; private set; }
        public bool IsMagic { get; private set; }
        public byte DemandType { get; private set; }
        public byte MaxTargets { get; private set; }

        public static ActiveSkillInfo Load(DataRow row)
        {
            ActiveSkillInfo inf = new ActiveSkillInfo
            {
                           
                ID = (ushort)row["ID"],
                Name = (string)row["InxName"],
                Step = (byte)row["Step"],
                Required = (string)row["DemandSk"],
                SP = (ushort)row["SP"],
                HP = (ushort)row["HP"],
                Range = (ushort)row["Range"],
                CoolTime = (uint)row["DlyTime"],
                CastTime = (uint)row["CastTime"],
                DemandType = (byte)row["DemandType"],
                MaxTargets = (byte)row["TargetNumber"],
            };

            ushort maxdamage = (ushort)row["MaxWC"];
            if (maxdamage == 0)
            {
                inf.IsMagic = true;
                inf.MinDamage = (ushort)row["MinMA"];
                inf.MaxDamage = (ushort)row["MaxMA"];
            }
            else
            {
                inf.MaxDamage = maxdamage;
                inf.MinDamage = (ushort)row["MinWC"];
            }
            return inf;
        }
    }
}
