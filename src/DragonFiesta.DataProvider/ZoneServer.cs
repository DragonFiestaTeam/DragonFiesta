using System.Collections.Generic;
using System;

namespace DragonFiesta.Data
{
    [Serializable]
    public class ZoneServer
    {
        public ushort ID { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public DateTime LastPing { get; set; }
        public Dictionary<ushort,Map> maps {get; set; }
        public bool IsReady { get; set; }
        public string QeueName { get; set; }
    }
}
