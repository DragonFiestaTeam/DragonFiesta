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
			// TODO: Load
	        return null;
        }
    }
}
