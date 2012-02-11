using System;
using Zepheus.Util;
using System.Collections.Generic;
using System.Data;

namespace Zepheus.FiestaLib.Data
{
	public sealed class ShineNPC
	{
        public Int16 MobID { get; private set; }
		public String MobName { get; private set; }
		public String Map { get; private set; }
		public Int32 Coord_X { get; private set; }
		public Int32 Coord_Y { get; private set; }
		public Int16 Direct { get; private set; }
		public Byte NPCMenu { get; private set; }
		public String Role { get; private set; }
        public String RoleArg0 { get; private set; }
        public ushort Flags { get; private set; }
        public List<Vendor> VendorItems { get; set; }
		public static ShineNPC Load(DataRow Row)
		{
			ShineNPC info = new ShineNPC
			{
                MobID = (short)(Int32)Row["MobID"],
                Flags = (ushort)Row["Flags"],
				MobName = (string)Row["MobName"],
				Map = (string)Row["Map"],
				Coord_X = (int)Row["RegenX"],
				Coord_Y = (int)Row["RegenY"],
				Direct = (short)(Int32)Row["Direct"],
                NPCMenu = (byte)(SByte)Row["NPCMenu"],
                Role = (string)Row["Role"],
                RoleArg0 = (string)Row["RoleArg0"],
			};
            if (info.Flags == 1)
            {
                info.VendorItems = new List<Vendor>();
            }
      
			return info;
		}
	}
}
