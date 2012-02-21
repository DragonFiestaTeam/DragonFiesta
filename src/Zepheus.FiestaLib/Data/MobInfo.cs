using System.Collections.Generic;
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
        public bool IsNpc { get; private set; }
        public bool IsAggro { get; private set; }
        public byte Type { get; private set; }
        public ushort Size { get; private set; }

        public List<DropInfo> Drops { get; private set; }

        public byte MinDropLevel { get; set; }
        public byte MaxDropLevel { get; set; }

        public static MobInfo Load(DataRow row)
        {
            MobInfo inf = new MobInfo
            {
                Name = (string)row["InxName"],
                ID = (ushort)row["ID"],
                Level = (byte)row["Level"],
                MaxHP = (uint)row["MaxHP"],
                RunSpeed = (ushort)row["RunSpeed"],
                IsNpc = (bool)row["IsNPC"],
                Size = (ushort)row["Size"],
                Type = (byte)row["Type"],
                IsAggro = (bool)row["IsPlayerSide"],
                Drops = new List<DropInfo>()
            };
            return inf;
        }
    }
}
