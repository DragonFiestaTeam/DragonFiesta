using System;
using System.Data;

namespace Zepheus.FiestaLib.Data
{
    public sealed class Mount
    {
        public byte MinLevel { get; set; }
        public ushort ItemID { get; set; }
        public long TickSpeed { get; set; }
        public ushort Handle { get; set; }
        public ushort speed { get; set; }
        public DateTime Tick { get; set; }
        public ushort Food { get; set; }
        public int CastTime { get; set; }
        public ushort Cooldown { get; set; }
        public byte ItemSlot { get; set; }
        public bool permanent { get; set; }
        public static Mount LoadMount(DataRow Data)
        {
			// todo: load
	        return null;
        }
    }
}
