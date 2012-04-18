
namespace Zepheus.Zone.Game
{
    public class DroppedEquip : DroppedItem
    {
        public ushort Dex { get; set; }
        public ushort Str { get; set; }
        public ushort End { get; set; }
        public ushort Int { get; set; }
        public ushort Spr { get; set; }
        public ushort Upgrades { get; set; }

        public DroppedEquip(Equip pBase)
        {
            this.Amount = 1;
            //this.Expires = pBase.Expires;
            this.Dex = pBase.Dex;
            this.Str = pBase.Str;
            this.End = pBase.End;
            this.Int = pBase.Int;
            this.Upgrades = pBase.Upgrades;
            this.ItemID = pBase.ID;
        }
    }
}
