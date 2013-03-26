using System;
using DragonFiesta.Database;

namespace DragonFiesta.Data
{
    public class CharacterStatePoints
    {
        public int CurHP { get; set; }
        public int CurSP { get; set; }

        public byte StrBonus { get; set; }
        public byte EndStat { get; set; }
        public byte DexStat { get; set; }
        public byte IntStat { get; set; }
        public byte SprStat { get; set; }

        public void ReadCharacterStatsFromDatabase(SQLResult result, int row)
        {
            this.StrBonus = result.Read<Byte>(row, "Str");
            this.EndStat = result.Read<Byte>(row, "End");
            this.DexStat = result.Read<Byte>(row, "Dex");
            this.SprStat = result.Read<Byte>(row, "Spr");
            this.IntStat = result.Read<Byte>(row, "StrInt");
        }
        public void ReadCurrentHPAndSP(SQLResult result, int row)
        {
            this.CurHP = result.Read<Int32>(row, "CurHP");
            this.CurSP = result.Read<Int32>(row, "CurSP");
        }
    }
}
