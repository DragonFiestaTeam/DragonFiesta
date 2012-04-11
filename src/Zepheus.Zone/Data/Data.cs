using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zepheus.Zone.Data
{
    public sealed class Data
    {
        public enum NpcFlags : ushort
        {
            Normal = 0,
            Vendor = 1,
            Teleporter = 2
        }
    }
}
