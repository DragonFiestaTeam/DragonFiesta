using System;
using DragonFiesta.Database;
using DragonFiesta.FiestaLib;
using DragonFiesta.Networking;

namespace DragonFiesta.Data
{
    public class CharacterClass
    {
        public ClassID pClassID { get; set; }

        public Byte Level { get; set; }

        public StonePowerInfo StonesPowers { get; set; }
        public ulong EXP { get; set; }

        public CharacterStats ClassStats { get; set; }
    }
}
