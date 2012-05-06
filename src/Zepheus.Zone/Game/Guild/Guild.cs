using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zepheus.Zone.Game.Guild
{
    public sealed class Guild
    {
        public GuildNotice GuildNote { get; set; }
        public int GuildID { get; set; }
        public DateTime? Date { get; set; }
        public List<GuildMember> GuildMembers { get; set; }
    }
    public sealed class GuildNotice
    {
        public string NoticeOwner { get; set; }
        public string Notice { get; set; }
    }
}
