using System;
using System.Collections.Generic;
using DragonFiesta.InterNetwork;
using DragonFiesta.Data;
using DragonFiesta.DataProvider;

namespace DragonFiesta.Messages.Zone
{
    [Serializable]
    public class NewZoneServerStarted : IMessage
    {

        public Guid Id { get; set; }
        public string IP { get; set; }
        public ushort Port { get; set; }
        public Dictionary<ushort, Map> Maps { get; set; }
    }
}
