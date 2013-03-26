using DragonFiesta.Data;
using DragonFiesta.InterNetwork;
using System;
using DragonFiesta.DataProvider;

namespace DragonFiesta.Messages.Zone
{
    [Serializable]
    public class RegisterMap : IMessage
    {
        public Guid Id { get; set; }
        public Map Map { get; set; }
    }
}
