using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

using Zepheus.FiestaLib.SHN;
using Zepheus.Util;
using System.Data;
namespace Zepheus.FiestaLib.Data
{
    public sealed class MiniHouseInfo
    {
        public ushort ID { get; private set; }
        public ushort KeepTime_Hour { get; private set; }
        public ushort HPTick { get; private set; }
        public ushort SPTick { get; private set; }
        public ushort HPRecovery { get; private set; }
        public ushort SPRecovery { get; private set; }

        // public int Slot { get; set; } // No idea, only 5 or 10
        // public string Name { get; set; } // Not needed for now
        // public ushort CastTime { get; set; } // Not needed for now

        public MiniHouseInfo(DataRow Row)
        {
            ID = (ushort)Row["Handle"];
            KeepTime_Hour = (ushort)Row["KeepTime_Hour"];
            HPTick = (ushort)Row["HPTick"];
            SPTick = (ushort)Row["SPTick"];
            HPRecovery = (ushort)Row["HPRecovery"];
            SPRecovery = (ushort)Row["SPRecovery"];
        }
    }
}
