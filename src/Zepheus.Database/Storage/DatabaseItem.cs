using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zepheus.Database.Storage
{
  public  class DatabaseItem
    {
        public int ID { get; set; }
        public int Owner { get; set; }
        public int ObjectID { get; set; }
        public short Slot { get; set; }
        public short Amount { get; set; }
        public Character Character { get; set; }
    }
}
