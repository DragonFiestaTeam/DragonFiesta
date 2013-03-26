using System;
using DragonFiesta.FiestaLib;
namespace DragonFiesta.Data
{
    public class StonePowerInfo
    {
        public ClassID pClass { get; set; }

        public Byte Level { get; set; }

        public int SPStoneEffectID { get; set; }

        public int HPStoneEffectID { get; set; }

        public long HPStonePrice { get; set; }

        public long SPStonePrice { get; set; }

        public int HPMaxCount { get; set; }

        public int SPMaxCount { get; set; }

        public int SoulHP { get; set; }

        public int SoulSP { get; set; }
    }
}
