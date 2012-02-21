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
        public byte IncStr { get; set; }
        public byte IncEnd { get; set; }
        public byte IncDex { get; set; }
        public byte IncInt { get; set; }
        public byte IncSpr { get; set; }
        public DateTime? Expires { get; set; }
        public Character Character { get; set; }
        
    }
}
