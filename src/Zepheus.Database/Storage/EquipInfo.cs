using System;

namespace Zepheus.Database.Storage
{
    public class EquipInfo
    {
        public int ID { get; set; }
        public int Owner { get; set; }
        public short Slot { get; set; }
        public int EquipID { get; set; }
        public byte Upgrades { get; set; }
        public ushort IncStr { get; set; }
        public ushort IncEnd { get; set; }
        public ushort IncDex { get; set; }
        public ushort IncInt { get; set; }
        public ushort IncSpr { get; set; }
        public DateTime? Expires { get; set; }
        public Character Character { get; set; }
        
    }
}
