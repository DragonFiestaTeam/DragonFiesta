using System.Collections.Generic;

namespace Zepheus.Database.Storage
{
    public class Guild
    {
        public int ID { get;  set; }
        public string Name { get;  set; }
    }
    public class Guilds
    {
        public List<Guild> GuildList { get; set; }
    }
}
