using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zepheus.World.Data
{
    public class Guild
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<int> GuildMembers = new List<int>();
        public string GuildPassword { get; set; }
        public int GuildMaster { get; set; }
    }
    public class Guilds
    {
        public List<Guild> GuildList { get; set; }
    }
}
