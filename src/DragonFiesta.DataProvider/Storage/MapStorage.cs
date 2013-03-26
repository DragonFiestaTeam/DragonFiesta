using System;
using System.Collections.Generic;
using DragonFiesta.Networking;

namespace DragonFiesta.Data
{
    [Serializable]
    public class MapStorage : MapData
    {
        Dictionary<ushort, MapObject> MapObject = new Dictionary<ushort, MapObject>();
    }
}
