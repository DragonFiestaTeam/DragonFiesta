using DragonFiesta.Data;
using DragonFiesta.Networking;
using DragonFiesta.Networking.Handler.Server;
using DragonFiesta.Util;
using System;
using DragonFiesta.DataProvider;

namespace DragonFiesta.Data
{
    public partial class Character
    {
        public void WriteBasicInfo(Packet packet)
        {
            string MapShort = DataProvider.DataProvider.Instance.GetMap(this.Position.Map).ShortName;
            packet.WriteInt(this.CharacterID);
            packet.FillPadding(this.Name, 16);
            packet.WriteInt(0);//unk
            packet.WriteUShort(this.CharacterLevel);
            packet.WriteByte((byte)this.CharacterSlot);//slot
            packet.FillPadding(MapShort, 12);

            packet.WriteByte(0);//unk

            packet.WriteInt(0);
            WriteLook(packet);
            WriteEquipment(packet);
            WriteRefinement(packet);

            packet.FillPadding(MapShort, 12);
            packet.WriteUInt(this.Position.XPos);//pos
            packet.WriteUInt(this.Position.YPos);//pos

            packet.WriteUInt(1568560460);//4C 55 7E 5D
            packet.WriteByte(0);
            packet.WriteInt(100);
            packet.WriteByte(0);
        }

        public  void WriteLook(Packet packet)
        {
            packet.WriteByte(Convert.ToByte(0x01 | ((byte)this.Class.pClassID << 2) | (Convert.ToByte(this.Look.Male)) << 7));
            packet.WriteByte(this.Look.Hair);
            packet.WriteByte(this.Look.HairColor);
            packet.WriteByte(this.Look.Face);
        }

        public void WriteDetailedInfo(Packet Packet)
        {
            Packet.WriteInt(this.CharacterID);
            Packet.WriteString(this.Name, 16);

            Packet.WriteByte(this.CharacterSlot);
            Packet.WriteByte((byte)this.CharacterLevel);
            Packet.WriteULong(this.EXP);


            Packet.WriteInt(12345678); // tmp ?
            Packet.WriteUShort((ushort)this.CurrentHPStones);
            Packet.WriteUShort((ushort)this.CurrentSPStones);
            Packet.WriteUInt(this.CurrentHP);
            Packet.WriteUInt(this.CurrentSP);

            Packet.WriteUInt(this.Fame);
            Packet.WriteULong(this.Money);



            Packet.WriteString(DataProvider.DataProvider.Instance.GetMap(this.Position.Map).ShortName, 12);
            Packet.WriteUInt(this.Position.XPos);
            Packet.WriteUInt(this.Position.YPos);
            Packet.WriteByte(this.Position.ZPos);



            //stat points
            Packet.WriteByte(this.StatePoints.StrBonus); // str points
            Packet.WriteByte(this.StatePoints.EndStat); // end points
            Packet.WriteByte(this.StatePoints.DexStat); // dex points
            Packet.WriteByte(this.StatePoints.IntStat); // int points
            Packet.WriteByte(this.StatePoints.SprStat); // spr points


            Packet.Fill(2, 0); // tmp ?

            Packet.WriteUInt(this.KillPoints);

            Packet.Fill(7, 0); // tmp ?
        }

        public void SendCreate(ClientBase pClient)
        {
            using (var packet = new Packet(SH5Type._Header,SH5Type.CharCreationOk))
            {
                packet.WriteByte(1);
                this.WriteBasicInfo(packet);
               pClient.SendPacket(packet);
            }
        }
        public void SendDelete(ClientBase pClient,byte slot)
        {
            using (var packet = new Packet(SH5Type._Header, SH5Type.CharDeleteOk))
            {
                packet.WriteByte(slot);
                pClient.SendPacket(packet);
            }
        }

        public void WriteEquipment(Packet packet)
        {
            packet.WriteUShort(0xffff);//helm
            packet.WriteUShort(0xffff);//weapon 1
            packet.WriteUShort(0xffff);//weapon2
            packet.WriteUShort(0xffff);//armor
            packet.Fill(6, 0xff);              // UNK
            packet.WriteUShort(0xffff);//pants
            packet.WriteUShort(0xffff);//boots
            packet.WriteUShort(0xffff);//costumepants
            packet.WriteUShort(0xffff);//costume armor
            packet.Fill(6, 0xff);              // UNK
            packet.WriteUShort(0xffff);//glasses
            packet.WriteUShort(0xffff);//costumehelm
            packet.Fill(2, 0xff);              // UNK
            packet.WriteUShort(0xffff);//costumeweapon
            packet.WriteUShort(0xffff);//wing
            packet.Fill(2, 0xff);              // UNK
            packet.WriteUShort(0xffff);//tail
            packet.WriteUShort(0xffff);//pet
        }

        public void WriteRefinement(Packet pPacket)
        {
            //TODO: pPacket.WriteByte(Convert.ToByte(this.Inventory.GetEquippedUpgradesByType(ItemType.Weapon) << 4 | this.Inventory.GetEquippedUpgradesByType(ItemType.Shield))); 
            pPacket.WriteByte(0xff); //this must be the above, but currently not cached
            pPacket.WriteByte(0xff);                    // UNK
            pPacket.WriteByte(0xff);                    // UNK
        }

        public  void WriteDetailedInfoExtra(Packet Packet,bool IsLevelUp = false)
        {
            if (!IsLevelUp)
            {
                Packet.WriteUShort(this.MabObjectID);
            }


            Packet.WriteLong((long)this.EXP);
            Packet.WriteULong(DataProvider.DataProvider.Instance.GetExpForLevel(this.Class.pClassID,this.CharacterLevel)); // exp for next level





            Packet.WriteInt(this.Class.ClassStats.STR); // base str
            //Packet.WriteUInt32(Character.Stats.FullStats.Str); // full str
            Packet.WriteInt(this.FullStats.STR);

            Packet.WriteInt(this.Class.ClassStats.END); // base end
            Packet.WriteInt(this.FullStats.END); // full end

            Packet.WriteInt(this.Class.ClassStats.DEX); // base dex
            Packet.WriteInt(this.FullStats.DEX); // full dex

            Packet.WriteInt(this.Class.ClassStats.INT); // base int
            Packet.WriteInt(this.FullStats.INT); // full int

            Packet.WriteInt(370); // wizdom. // 370
            Packet.WriteInt(3186); // wizdom? // 3186

            Packet.WriteInt(this.Class.ClassStats.SPR); // base spr
            Packet.WriteInt(this.FullStats.SPR); // full spr

            Packet.WriteInt(this.NormalStats.MinDamage); // base dmg min
            Packet.WriteInt(this.NormalStats.MaxDamage); // base dmg max
            Packet.WriteInt(this.FullStats.MinDamage); // base magic dmg min
            Packet.WriteInt(this.FullStats.MaxDamage); // base magic dmg max

            Packet.WriteInt(this.NormalStats.DEF); // base def
            Packet.WriteInt(this.FullStats.DEF); // increased def (e.g. buffs)


            Packet.WriteInt(this.NormalStats.AIM); // base aim
            Packet.WriteInt(this.FullStats.AIM); // increased aim

            Packet.WriteInt(this.NormalStats.DODGE); // base Evasion
            Packet.WriteInt(this.FullStats.DODGE); // increased Evasion

            Packet.WriteInt(this.NormalStats.MinMagicDamage); // increased dmg min
            Packet.WriteInt(this.NormalStats.MaxMagicDamage); // increased dmg max
            Packet.WriteInt(this.FullStats.MinMagicDamage); // increased magic dmg min
            Packet.WriteInt(this.FullStats.MaxMagicDamage); // increased magic dmg max


            Packet.WriteInt(this.NormalStats.MagicDef); // normal magic def
            Packet.WriteInt(this.FullStats.MagicDef); // increased  magic def

            Packet.WriteInt(1);
            Packet.WriteInt(20);
            Packet.WriteInt(2);
            Packet.WriteInt(40);

            Packet.WriteInt(this.FullStats.MaxHP); // max hp
            Packet.WriteInt(this.FullStats.MaxSP); // max sp

            Packet.WriteInt(0);

            Packet.WriteInt(this.Stones.SPMaxCount); // max hp stones
            Packet.WriteInt(this.Stones.SPMaxCount); // max sp stones

            Packet.Fill(64, 0); // buff bits ?

            if (!IsLevelUp)
            {
                Packet.WriteUInt(this.Position.XPos);
                Packet.WriteUInt(this.Position.YPos);
            }
        }
    }
}
