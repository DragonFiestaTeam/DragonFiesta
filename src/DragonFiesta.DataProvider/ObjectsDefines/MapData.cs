using DragonFiesta.Database;
using System;

namespace DragonFiesta.Data
{
    [Serializable]
    public class MapData
    {
        public ushort ID { get; private set; }

        public string ShortName { get; private set; }

        public string FullName { get; private set; }

        public int RegenX { get; private set; }

        public int RegenY { get; private set; }

        public byte Kingdom { get; private set; }

        public ushort ViewRange { get; private set; }

        public MapData() { }

        public void ReadMapFromDatabase(SQLResult result, int row)
        {
            this.ID = result.Read<UInt16>(row, "ID");
            this.ShortName = result.Read<String>(row, "ShortName");
            this.FullName = result.Read<String>(row, "FullName");
            this.Kingdom = result.Read<Byte>(row, "Kingdoom");
            this.RegenX = result.Read<Int32>(row, "RegenX");
            this.RegenY = result.Read<Int32>(row, "RegenY");
            this.ViewRange = result.Read<UInt16>(row, "ViewRange");
        }
    }
}
