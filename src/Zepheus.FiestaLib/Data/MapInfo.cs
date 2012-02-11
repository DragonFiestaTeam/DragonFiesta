using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zepheus.FiestaLib.SHN;
using Zepheus.Util;

namespace Zepheus.FiestaLib.Data
{
    public sealed class MapInfo
    {
        public ushort ID { get; private set; }
        public string ShortName { get; private set; }
        public string FullName { get; private set; }
        public int RegenX { get; private set; }
        public int RegenY { get; private set; }
        public byte Kingdom { get; private set; }
        public ushort ViewRange { get; private set; }

        public List<ShineNPC> NPCs { get; set; }

        public MapInfo() { }
        public MapInfo(ushort id, string shortname, string fullname, int regenx, int regeny, byte kingdom, ushort viewrange)
        {
            this.ID = id;
            this.ShortName = shortname;
            this.FullName = fullname;
            this.RegenX = regenx;
            this.RegenY = regeny;
            this.Kingdom = kingdom;
            this.ViewRange = viewrange;
        }

        public static MapInfo Load(DataRow Row)
        {
            MapInfo info = new MapInfo
            {
                ID = (ushort)Row["ID"],
                ShortName = (string)Row["MapName"],
                FullName = (string)Row["Name"],
                RegenX = (Int32)Row["RegenX"],
                RegenY = (Int32)Row["RegenY"],
                Kingdom = (byte)(SByte)Row["KingdomMap"],
                ViewRange = (ushort)Row["Sight"],
            };
            return info;
        }
    }
}
