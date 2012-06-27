using System;
using Zepheus.Database.DataStore;
using System.Collections.Generic;
using System.Data;

namespace Zepheus.FiestaLib.Data
{
    public class MasterRewardItem
    {
        public byte Level { get; private set; }
        public Job Job { get; private set; }
        public ushort ItemID { get;  set; }
        public byte Upgrades { get; set; }
        public byte Count { get; set; }
       
        public ushort Str { get; private set; }
        public ushort End { get; private set; }
        public ushort Dex { get; private set; }
        public ushort Int { get; private set; }
        public ushort Spr { get; private set; }
        public MasterRewardItem()
        {
        }
        public MasterRewardItem(DataRow row)
         {
             this.ItemID = GetDataTypes.GetUshort(row["ItemID"]);
             this.Level = GetDataTypes.GetByte(row["Level"]);
             this.Job = (FiestaLib.Job)GetDataTypes.GetByte(row["Job"]);
             this.Upgrades = GetDataTypes.GetByte(row["Upgrades"]);
             this.Str = GetDataTypes.GetUshort(row["Str"]);
             this.End = GetDataTypes.GetUshort(row["End"]);
             this.Dex =  GetDataTypes.GetUshort(row["Dex"]);
             this.Int = GetDataTypes.GetUshort(row["Int"]);
             this.Spr = GetDataTypes.GetUshort(row["Spr"]);
             this.Count = GetDataTypes.GetByte(row["Count"]);
         }
    }
}
