
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.Zone.Game;
using Zepheus.Zone.Networking;

namespace Zepheus.Zone.Handlers
{
    public sealed class Handler12
    {
        [PacketHandler(CH12Type.Unequip)]
        public static void UnequipHandler(ZoneClient client, Packet packet)
        {
            ZoneCharacter character = client.Character;

            byte sourceSlot;
            sbyte destinationSlot; //not so sure about this one anymore
            if (!packet.TryReadByte(out sourceSlot) ||
                !packet.TryReadSByte(out destinationSlot))
            {
                Log.WriteLine(LogLevel.Warn, "Could not read unequip values from {0}.", character.Name);
                return;
            }
            character.UnequipItem((ItemSlot)sourceSlot, destinationSlot);
        }
        [PacketHandler(CH12Type.BuyItem)]
        public static void BuyItem(ZoneClient client, Packet packet)
        {
            ZoneCharacter character = client.Character;
            ushort BuyItemID;
            int Amount;
            if (packet.TryReadUShort(out BuyItemID) && packet.TryReadInt(out Amount))
            {
               FiestaLib.Data.ItemInfo  BuyItem;
               Data.DataProvider.Instance.ItemsByID.TryGetValue(BuyItemID, out BuyItem);
               if (Amount < 255)
               {
                   if (character.GiveItem(BuyItemID, (byte)Amount) != InventoryStatus.FULL)
                   {
                       character.Money -= Amount * BuyItem.BuyPrice;
                       Handler4.SendMoney(character);
                   }
               }
               else
               {
                   while (Amount > 0)
                   {
                       if (character.GiveItem(BuyItemID, 255) != InventoryStatus.FULL)
                       {
                           character.Money -= Amount * BuyItem.BuyPrice;
                           Handler4.SendMoney(character);
                       }
                       if (Amount < 255)
                       {
                           if (character.GiveItem(BuyItemID, (byte)Amount) != InventoryStatus.FULL)
                           {
                               character.Money -= Amount * BuyItem.BuyPrice;
                               Handler4.SendMoney(character);
                           }
                           break;
                       }
                       Amount -= 255;
                   }
               }
            }
        }
        [PacketHandler(CH12Type.SellItem)]
        public static void SellItem(ZoneClient client, Packet packet)
        {
           byte slot;
           int sellcount;
           ZoneCharacter Character = client.Character;
           if (packet.TryReadByte(out slot) && packet.TryReadInt(out sellcount))
           {
               
               Item item;
               Character.InventoryItems.TryGetValue((sbyte)slot,out item);
               if (item != null)
               {
          
                   long FullSellPrice = sellcount * item.Info.SellPrice;
                   if (item.Amount > 1)
                   {
                       item.Amount -= (short)sellcount;
                       byte Slot = (byte)item.Slot;
                       Handler12.ModifyInventorySlot(Character, 0x24, Slot, Slot, item);
                       Character.Money += FullSellPrice;
                       Handler4.SendMoney(Character);
                       if (item.Info.Type == FiestaLib.Data.ItemType.Equip)
                       {
                           Program.CharDBManager.GetClient().ExecuteQuery("UPDATE equips SET Amount='" + item.Amount + "' WHERE Owner='" + item.Owner.ID + "' AND EquipID='" + item.ItemID + "' AND Slot='" + item.Slot + "'");
                       }
                       else
                       {
                           Program.CharDBManager.GetClient().ExecuteQuery("UPDATE items SET Amount='" + item.Amount + "' WHERE Owner='" + item.Owner.ID + "' AND ItemID='" + item.ItemID + "' AND Slot='" + item.Slot + "'");
                       }
                   }
                   else
                   {
                       Character.Money += FullSellPrice;
                       Handler4.SendMoney(Character);
                       Character.InventoryItems.Remove((sbyte)slot);
                       ResetInventorySlot(Character, slot);
                       if (item.Info.Type == FiestaLib.Data.ItemType.Equip)
                       {
                           Program.CharDBManager.GetClient().ExecuteQuery("DELETE  FROM equips WHERE Owner='" + item.Owner.ID + "' AND EquipID='" + item.ItemID + "' AND Slot='" + item.Slot + "'");
                       }
                       else
                       {
                           Program.CharDBManager.GetClient().ExecuteQuery("DELETE  FROM Items WHERE Owner='" + item.Owner.ID + "' AND ItemID='" + item.ItemID + "' AND Slot='" + item.Slot + "'");
                       }
                   }
                   System.Console.WriteLine(item.Info.Type);
               }
           }
        }
        [PacketHandler(CH12Type.LootItem)]
        public static void LootHandler(ZoneClient client, Packet packet)
        {
            ushort id;
            if (!packet.TryReadUShort(out id))
            {
                Log.WriteLine(LogLevel.Warn, "Invalid loot request.");
                return;
            }
            client.Character.LootItem(id);
        }

        [PacketHandler(CH12Type.UseItem)]
        public static void UseHandler(ZoneClient client, Packet packet)
        {
            sbyte slot;
            if (!packet.TryReadSByte(out slot))
            {
                Log.WriteLine(LogLevel.Warn, "Error reading used item slot.");
                return;
            }
            client.Character.UseItem(slot);
        }

        public static void SendItemUseOK(ZoneCharacter character)
        {
            using (var packet = new Packet(SH12Type.ItemUsedOk))
            {
                character.Client.SendPacket(packet);
            }
        }

        public static void SendItemUsed(ZoneCharacter character, Item item, ushort error = (ushort) 1792)
        {
            if (error == 1792)
            {
                SendItemUseOK(character);
            }

            using (var packet = new Packet(SH12Type.ItemUseEffect))
            {
                packet.WriteUShort(error); //when not ok, it'll tell you there will be no effect
                packet.WriteUShort(item.ItemID);
                character.Client.SendPacket(packet);
            }
        }


        public static void ObtainedItem(ZoneCharacter character, DroppedItem item, ObtainedItemStatus status)
        {
            using (var packet = new Packet(SH12Type.ObtainedItem))
            {
                packet.WriteUShort(item.ItemID);
                packet.WriteInt(item.Amount);
                packet.WriteUShort((ushort)status);
                packet.WriteUShort(0xffff);
                character.Client.SendPacket(packet);
            }
        }

        [PacketHandler(CH12Type.Equip)]
        public static void EquipHandler(ZoneClient client, Packet packet)
        {
            sbyte slot;
            if (!packet.TryReadSByte(out slot))
            {
                Log.WriteLine(LogLevel.Warn, "Error reading equipping slot.");
                return;
            }
            Item item;
            if (client.Character.InventoryItems.TryGetValue(slot, out item))
            {
                if (item is Equip)
                {
                    if (((Equip)item).Info.Level > client.Character.Level)
                    {
                        FailedEquip(client.Character, 645); // 85 02
                    }
                    else
                    {
                        client.Character.EquipItem((Equip)item);
                    }
                }
                else
                {
                    FailedEquip(client.Character);
                    Log.WriteLine(LogLevel.Warn, "{0} equippped an item. What a moron.", client.Character.Name);
                }
            }
        }

        [PacketHandler(CH12Type.MoveItem)]
        public static void MoveItemHandler(ZoneClient client, Packet packet)
        {
            byte from, oldstate, to, newstate;
            if(!packet.TryReadByte(out from) ||
                !packet.TryReadByte(out oldstate) ||
                !packet.TryReadByte(out to) ||
                !packet.TryReadByte(out newstate))
            {
                    Log.WriteLine(LogLevel.Warn, "Invalid item move received.");
                    return;
            }
            client.Character.MoveItem((sbyte)from, (sbyte)to);
        }

        [PacketHandler(CH12Type.DropItem)]
        public static void DropItemHandler(ZoneClient client, Packet packet)
        {
            sbyte slot;
            if (!packet.TryReadSByte(out slot))
            {
                Log.WriteLine(LogLevel.Warn, "Invalid drop request.");
                return;
            }
            client.Character.DropItemRequest(slot);
        }

        [PacketHandler(CH12Type.ItemEnhance)]
        public static void EnhancementHandler(ZoneClient client, Packet packet)
        {
            sbyte weapslot, stoneslot;
            if (!packet.TryReadSByte(out weapslot) ||
                !packet.TryReadSByte(out stoneslot))
            {
                Log.WriteLine(LogLevel.Warn, "Invalid item enhance request.");
                return;
            }
            client.Character.UpgradeItem(weapslot, stoneslot);
        }

        public static void SendUpgradeResult(ZoneCharacter character, bool success)
        {
            using (var packet = new Packet(SH12Type.ItemUpgrade))
            {
                packet.WriteUShort(success ? (ushort)2243 : (ushort)2245);
                character.Client.SendPacket(packet);
            }
        }

        public static void InventoryFull(ZoneCharacter character)
        {
            using (var packet = new Packet(SH12Type.InventoryFull))
            {
                packet.WriteUShort(522);
                character.Client.SendPacket(packet);
            }
        }

        public static void FailedUnequip(ZoneCharacter character)
        {
            using (var packet = new Packet(SH12Type.FailedUnequip))
            {
                packet.WriteUShort(706);
                character.Client.SendPacket(packet);
            }
        }

        public static void FailedEquip(ZoneCharacter character, ushort val = (ushort) 0)
        {
            using (var packet = new Packet(SH12Type.FailedEquip))
            {
                packet.WriteUShort(val);
                character.Client.SendPacket(packet);
            }
        }

        public static void ModifyEquipSlot(ZoneCharacter character, byte modifyslot, byte otherslot, Equip equip)
        {
            using (var packet = new Packet(SH12Type.ModifyEquipSlot))
            {
                packet.WriteByte(otherslot);
                packet.WriteByte(0x24); //aka the 'equipped' bool
                packet.WriteByte(modifyslot);

                if (equip == null)
                {
                    packet.WriteUShort(ushort.MaxValue);
                }
                else
                {
                    equip.WriteEquipStats(packet);
                }
                character.Client.SendPacket(packet);
            }
        }

        public static void ModifyInventorySlot(ZoneCharacter character, byte inventory, byte newslot, byte oldslot, Item item)
        {
            using (var packet = new Packet(SH12Type.ModifyItemSlot))
            {
                packet.WriteByte(oldslot);
                packet.WriteByte(inventory); //aka 'unequipped' bool
                packet.WriteByte(newslot);
                packet.WriteByte(0x24);
                if (item == null)
                {
                    packet.WriteUShort(0xffff);
                }
                else
                {
                    if (item is Equip)
                    {
                        ((Equip)item).WriteEquipStats(packet);
                    }
                    else
                    {
                        item.WriteItemStats(packet);
                    }
                }
                character.Client.SendPacket(packet);
            }
        }

        public static void ResetInventorySlot(ZoneCharacter character, byte slot)
        {

            
            using (var packet = new Packet(SH12Type.ModifyItemSlot))
            {
                packet.WriteByte(0);
                packet.WriteByte(0x20);
                packet.WriteByte(slot);
                packet.WriteByte(0x24);
                packet.WriteUShort(0xffff);
                character.Client.SendPacket(packet);
            }
            Item i;
            character.InventoryItems.TryGetValue((sbyte)slot, out i);
            character.InventoryItems.Remove((sbyte)slot);
        }
    }
}
