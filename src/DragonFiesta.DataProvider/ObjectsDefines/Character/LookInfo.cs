using System;
using DragonFiesta.Database;

namespace DragonFiesta.Data
{
    public class LookInfo
    {
        public byte Hair { get; set; }
        public byte HairColor { get; set; }
        public byte Face { get; set; }
        public byte Male { get; set; }

        public void ReadFromDatabase(SQLResult result,int row)
        {
            this.Male = result.Read<Byte>(row, "Male");
            this.Hair = result.Read<Byte>(row, "Hair");
            this.HairColor = result.Read<Byte>(row, "HairColor");
            this.Face = result.Read<Byte>(row, "Face");
        }
    }
}
