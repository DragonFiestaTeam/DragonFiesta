using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragonFiesta.Data
{
    public class Acount
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte Admin { get; set; }
        public bool Banned { get; set; }
        public bool Logged { get; set; }
    }
}
