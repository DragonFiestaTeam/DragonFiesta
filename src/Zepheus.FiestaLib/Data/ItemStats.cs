using System.Data;

namespace Zepheus.FiestaLib.Data
{
    public class ItemStats
    {
        public ushort Str { get; set; }
        public ushort End { get; set; }
        public ushort Dex { get; set; }
        public ushort Int { get; set; }
        public ushort Spr { get; set; }

        public static ItemStats LoadItemStatsFromDatabase(DataRow row)
        {
			// ToDo Load
	        return null;
        }
    }
}
