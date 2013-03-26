using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragonFiesta.Data.Transfer
{
    public enum TransferType : byte
    {
        LoginToWorld = 1,
        WorldToZone = 2,
        MapToMap = 3,
    }
}
