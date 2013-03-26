using System;
using DragonFiesta.Database;

namespace DragonFiesta.Data
{
    [Serializable]
    public class Map : MapStorage
    {
        public ZoneServer pZoneServer { get; set; }
    }
}
