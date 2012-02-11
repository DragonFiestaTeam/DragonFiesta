using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zepheus.Util;
using Zepheus.FiestaLib.SHN;

namespace Zepheus.FiestaLib.Data
{
    public sealed class ItemInfo
    {
        public ushort ItemID { get; private set; }
        public ItemSlot Slot { get; private set; }
        public bool TwoHand { get; private set; }
        public string InxName { get; private set; }
        public byte MaxLot { get; private set; }
        public ushort AttackSpeed { get; private set; }
        public byte Level { get; private set; }
        public ItemType Type { get; private set; }
        public ItemClass Class { get; private set; }
        public byte UpgradeLimit { get; private set; }
        public Job Jobs { get; private set; }
        public ushort MinMagic { get; private set; }
        public ushort MaxMagic { get; private set; }
        public ushort MinMelee { get; private set; }
        public ushort MaxMelee { get; private set; }
        public ushort WeaponDef { get; private set; }
        public ushort MagicDef { get; private set; }
        public long BuyPrice { get; private set; }
        public long SellPrice { get; private set; }
        //item upgrade
        public ushort UpSucRation { get; private set; }
        public ushort UpResource { get; private set; }

        /// <summary>
        /// Needs serious fixing in the reader, as it throws invalid casts (files all use uint, but fuck those)
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static ItemInfo Load(DataRow Row)
        {
            ItemInfo itemInfo = new ItemInfo
            {
                ItemID = (ushort)Row["id"],
                Slot = (ItemSlot)Row["equip"],
                InxName = (string)Row["inxname"],
                MaxLot = (byte)Row["maxlot"],
                AttackSpeed = (ushort)Row["atkspeed"],
                Level = (byte)Row["demandlv"],
                Type = (ItemType)Row["type"],
                Class = (ItemClass)Row["class"],
                UpgradeLimit = (byte)Row["uplimit"],
                Jobs = UnpackWhoEquip((uint)Row["whoequip"]),
                TwoHand = (bool)Row["TwoHand"],
                MinMagic = (ushort)Row["minma"],
                MaxMagic = (ushort)Row["maxma"],
                MinMelee = (ushort)Row["minwc"],
                MaxMelee = (ushort)Row["maxwc"],
                WeaponDef = (ushort)Row["ac"],
                MagicDef = (ushort)Row["mr"],
                UpSucRation = (ushort)Row["UpSucRatio"],
                UpResource = (ushort)Row["UpResource"],
                SellPrice = (long)(uint)Row["SellPrice"],
                BuyPrice = (long)(uint)Row["BuyPrice"],
            };
            return itemInfo;
        }

       // [Obsolete("Too slow / incorrect?")]
        private static Job UnpackWhoEquip(uint value)
        {
            Job job = Job.None;
          //  string jobnames = "";
            for (int i = 0; i < 26; i++)
            {
                if ((value & (uint)Math.Pow(2, i)) != 0)
                {
                    job |= (Job)i;
            //        jobnames += ((Job)i).ToString() + " ";
                }
            }
            return job;
        }
    }

    public enum ItemType : byte
    {
        Equip = 0,
        Useable = 1,
        Etc = 2,
        Unknown = 3,
        Unknown2 = 5,
    }

    public enum ItemClass : byte
    {
        PremiumItem = 6,
        Shield = 7,
        BootsHelmet = 8,
        Furniture = 9,
        Accessory = 10,
        Skillbook = 11,
        ReturnScroll = 12,
        SilverWingsOnly = 13, // Csharp note: lelijk
        CraftStones = 14,
        PresentBox = 15,
        House = 18,
        RiderFood = 22,
        Rider = 23,
        Crafting = 24,
        Overlay = 26,
        Emotion = 27,

    }
}
