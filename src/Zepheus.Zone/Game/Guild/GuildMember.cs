using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zepheus.Zone.Game.Guild
{
   public sealed class GuildMember
    {
       public ZoneCharacter Member { get; set; }
       public Guild MemberGuild { get; set; }
    }
}
