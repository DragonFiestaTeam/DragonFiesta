using System;
using DragonFiesta.InterNetwork;

namespace DragonFiesta.Messages.Zone
{
    [Serializable]
    public class NewZoneServerStarted : IMessage
    {
        public Guid Id { get; set; }
        public string IP { get; set; }
        public ushort Port { get; set; }
    }
}
