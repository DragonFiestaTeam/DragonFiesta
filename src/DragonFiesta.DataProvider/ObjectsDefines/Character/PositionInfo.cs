using System;
using DragonFiesta.Database;
using DragonFiesta.Networking;
using DragonFiesta.DataProvider;

namespace DragonFiesta.Data
{
    public class PositionInfo
    {
        public uint XPos { get; set; }
        public uint YPos { get; set; }
        public ushort Map { get; set; }
        public byte ZPos { get; set; }
       
        public void ReadFromDatabase(SQLResult result,int row)
        {
            this.Map = result.Read<UInt16>(row, "map");
            this.XPos = result.Read<UInt32>(row, "XPos");
            this.YPos = result.Read<UInt32>(row, "YPos");
            this.ZPos = result.Read<Byte>(row, "ZPos");
        }

        public void Write(Packet pPacket)
        {
            pPacket.FillPadding(DataProvider.DataProvider.Instance.GetMap(this.Map).ShortName, 12);
            pPacket.WriteUInt(this.XPos);
            pPacket.WriteUInt(this.YPos);
        }
    }
}
