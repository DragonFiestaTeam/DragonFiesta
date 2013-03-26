using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragonFiesta.Data.Transfer
{
    public class ClientTransfer
    {
        public int AccountID { get; set; }
        public string UserName { get; set; }
        public byte Access_level { get; set; }
        public string AuthHash { get; set; }//use in login to world
        public ushort RandomID { get; set; }//use in world to zone
        public int CharacterID { get; set; }//use in world to zone
        public DateTime TransferStartTime { get; set; }
        public TransferType Type { get; set; }
        public string IP { get; set; }
    }
}
