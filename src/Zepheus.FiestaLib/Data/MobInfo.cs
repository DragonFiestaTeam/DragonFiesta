using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Zepheus.FiestaLib.SHN;
using Zepheus.Util;
using System.Data;

namespace Zepheus.FiestaLib.Data
{
    public sealed class MobInfo
    {
        public string Name { get; private set; }
        public ushort ID { get; private set; }
        public byte Level { get; private set; }
        public uint MaxHP { get; private set; }
        public ushort RunSpeed { get; private set; }
        public bool IsNPC { get; private set; }
        public bool IsAggro { get; private set; }
        public byte Type { get; private set; }
        public ushort Size { get; private set; }

        public List<DropInfo> Drops { get; private set; }

        public byte MinDropLevel { get; set; }
        public byte MaxDropLevel { get; set; }

        public static MobInfo Load(DataRow Row)
        {
            MobInfo inf = new MobInfo
            {
                Name = (string)Row["InxName"],
                ID = (ushort)Row["ID"],
                Level = (byte)Row["Level"],
                MaxHP = (uint)Row["MaxHP"],
                RunSpeed = (ushort)Row["RunSpeed"],
                IsNPC = (bool)Row["IsNPC"],
                Size = (ushort)Row["Size"],
                Type = (byte)Row["Type"],
                IsAggro = (bool)Row["IsPlayerSide"],
                Drops = new List<DropInfo>()
            };
            return inf;
        }
    }
}
