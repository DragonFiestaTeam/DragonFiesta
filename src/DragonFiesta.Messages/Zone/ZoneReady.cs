using System;
using DragonFiesta.InterNetwork;
using DragonFiesta.Data;

namespace DragonFiesta.Messages.Zone
{
    [Serializable]
    public class ZoneReady : IMessage
    {
        public Guid Id { get; set; }
        public int ZoneID { get; set; }
    }
}
