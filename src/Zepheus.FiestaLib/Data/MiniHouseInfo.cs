using System.Data;
namespace Zepheus.FiestaLib.Data
{
    public sealed class MiniHouseInfo
    {
        public ushort ID { get; private set; }
        public ushort KeepTimeHour { get; private set; }
        public ushort HPTick { get; private set; }
        public ushort SPTick { get; private set; }
        public ushort HPRecovery { get; private set; }
        public ushort SPRecovery { get; private set; }

        // public int Slot { get; set; } // No idea, only 5 or 10
        // public string Name { get; set; } // Not needed for now
        // public ushort CastTime { get; set; } // Not needed for now

        public MiniHouseInfo(DataRow row)
        {
            ID = (ushort)row["Handle"];
            KeepTimeHour = (ushort)row["KeepTime_Hour"];
            HPTick = (ushort)row["HPTick"];
            SPTick = (ushort)row["SPTick"];
            HPRecovery = (ushort)row["HPRecovery"];
            SPRecovery = (ushort)row["SPRecovery"];
        }
    }
}
