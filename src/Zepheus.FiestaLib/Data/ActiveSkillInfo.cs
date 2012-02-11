using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zepheus.FiestaLib.SHN;
using Zepheus.Util;

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

        public static ActiveSkillInfo Load(DataRow Row)
        {
            ActiveSkillInfo inf = new ActiveSkillInfo
            {
                           
                ID = (ushort)Row["ID"],
                Name = (string)Row["InxName"],
                Step = (byte)Row["Step"],
                Required = (string)Row["DemandSk"],
                SP = (ushort)Row["SP"],
                HP = (ushort)Row["HP"],
                Range = (ushort)Row["Range"],
                CoolTime = (uint)Row["DlyTime"],
                CastTime = (uint)Row["CastTime"],
                DemandType = (byte)Row["DemandType"],
                MaxTargets = (byte)Row["TargetNumber"],
            };

            ushort maxdamage = (ushort)Row["MaxWC"];
            if (maxdamage == 0)
            {
                inf.IsMagic = true;
                inf.MinDamage = (ushort)Row["MinMA"];
                inf.MaxDamage = (ushort)Row["MaxMA"];
            }
            else
            {
                inf.MaxDamage = maxdamage;
                inf.MinDamage = (ushort)Row["MinWC"];
            }
            return inf;
        }
    }
}
