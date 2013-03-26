using DragonFiesta.Data;
using System;

namespace DragonFiesta.Messages.World
{
    [Serializable]
    public class ClientFromWorldToZone : ClientFromLoginToWorld
    {
        public int ZoneID { get; set; }
        public ushort RandomID { get; set; }
        public int CharacterID { get; set; }
    }
}
