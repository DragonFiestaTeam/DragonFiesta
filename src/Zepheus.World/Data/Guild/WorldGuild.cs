using System.Linq;
using Zepheus.Database.Storage;

namespace Zepheus.World.Data
{
	public sealed class WorldGuild
	{
		private readonly Guild guild;

		public string Name { get { return guild.Name; } private set { guild.Name = value; } }

		public WorldGuild(Guild guild)
		{
			this.guild = guild;
		}

		public static WorldGuild GetGuild(int id)
		{
			WorldGuild g;
			if (DataProvider.Instance.Guilds.TryGetValue(id, out g))
			{
				if (g == null)
				{
					Guilds guild = new Guilds();

					var ng = guild.GuildList.First(v => v.ID == id);
					if (ng == null)
					{
						return null;
					}
					else
					{
						g = new WorldGuild(ng);
						DataProvider.Instance.Guilds.Add(id, g);
					}
				}
				
			}
			return g;
		}
	}
}
