using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zepheus.Util;
using Zepheus.FiestaLib.SHN;

namespace Zepheus.FiestaLib.Data
{
    public sealed class ItemUseEffectInfo
    {
        public ushort ID { get;  set; }
        public string AbState { get; private set; }
        public List<ItemEffect> Effects { get; private set; }

        public ItemUseEffectInfo()
        {
            Effects = new List<ItemEffect>();
        }

        public static ItemUseEffectInfo Load(DataRow Row, out string InxName)
        {
            ItemUseEffectInfo info = new ItemUseEffectInfo();
            InxName = (string)Row["ItemIndex"];

            ItemUseEffectType typeA = (ItemUseEffectType)(uint)Row["UseEffectA"];
            if (typeA != ItemUseEffectType.None)
            {
                ItemEffect effect = new ItemEffect();
                effect.Type = typeA;
                effect.Value = (uint)Row["UseValueA"];
                info.Effects.Add(effect);
            }

            ItemUseEffectType typeB = (ItemUseEffectType)(uint)Row["UseEffectB"];
            if (typeB != ItemUseEffectType.None)
            {
                ItemEffect effect = new ItemEffect();
                effect.Type = typeB;
                effect.Value = (uint)Row["UseValueB"];
                info.Effects.Add(effect);
            }

            ItemUseEffectType typeC = (ItemUseEffectType)(uint)Row["UseEffectC"];
            if (typeC != ItemUseEffectType.None)
            {
                ItemEffect effect = new ItemEffect();
                effect.Type = typeC;
                effect.Value = (uint)Row["UseValueC"];
                info.Effects.Add(effect);
            }
            info.AbState = (string)Row["UseAbStateName"];
            return info;
        }
    }

    public struct ItemEffect
    {
        public ItemUseEffectType Type { get; set; }
        public uint Value { get; set; }
    }

    public enum ItemUseEffectType : byte
    {
        HP = 0,
        SP = 1,
        AbState = 4,
        ScrollTier = 5,
        None = 6,
    }
}
