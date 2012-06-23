using System;
using Zepheus.Zone.Game;

namespace Zepheus.Zone.Data
{
   public class CommercialItem
    {
       public ZoneCharacter Owner { get; set; }
       public byte InventorySlot { get; set; }
       public byte CommercialSlot { get; set; }

      public  CommercialItem(ZoneCharacter owner,byte InventorySlot,byte Commerciaslot)
      {
          this.Owner = owner;
          this.InventorySlot = InventorySlot;
          this.CommercialSlot = Commerciaslot;
      }
    }

}
