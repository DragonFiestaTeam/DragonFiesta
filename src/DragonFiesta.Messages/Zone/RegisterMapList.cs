using DragonFiesta.InterNetwork;
using DragonFiesta.Data;
using System;
using System.Collections.Generic;

namespace DragonFiesta.Messages.Zone
{
    [Serializable]
    public class RegisterMapList : IMessage
    {
        public Guid Id { get; set; }
        public ushort ZoneID { get; set; }
        public Dictionary<ushort, Map> maps { get; set; }
    }
}
