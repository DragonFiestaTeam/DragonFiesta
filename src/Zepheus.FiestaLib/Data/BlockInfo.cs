﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data;

namespace Zepheus.FiestaLib.Data
{
    public sealed class BlockInfo
    {
        public ushort MapID { get; private set; }
        public string ShortName { get; private set; }
  
        private byte Read { get;  set; }

        private int width;
        private int height;

        public int Width { get { return width * 50; } }
        public int Height { get { return (int)(height * 6.25); } }

        public BlockInfo(DataRow Row, ushort mapid)
        {
            MapID = mapid;
            LoadBasics(Row);
        }

        private void LoadBasics(DataRow Row)
        {
            width = (int)Row["Width"];
            height  = (int)Row["Height"];
            Read = (byte)(SByte)Row["Byte"];
        }


        private static readonly byte[] powers = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128, 255 };
        public bool CanWalk(int x, int y)
        {
            if (x <= 0 || y <= 0 || x >= Width || y >= Height) return false;
            return true;
            //rest latter
        }
    }
}
