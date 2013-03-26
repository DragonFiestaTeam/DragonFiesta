using System;
using DragonFiesta.InterNetwork;

namespace DragonFiesta.Messages.World
{
    [Serializable]
    public class ZonePing : IMessage
    {
        public Guid Id { get; set; }
        public ushort ZoneID { get; set; }
    }
}
