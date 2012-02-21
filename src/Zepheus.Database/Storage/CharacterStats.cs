using System.Data;

namespace Zepheus.Database.Storage
{
	public class CharacterStats
	{
		public byte StrStats { get; set; }
		public byte EndStats { get; set; }
		public byte DexStats { get; set; }
		public byte IntStats { get; set; }
		public byte SprStats { get; set; }

		public void ReadFromDatabase(DataRow row)
		{
			this.StrStats = byte.Parse(row["Str"].ToString());
			this.EndStats = byte.Parse(row["End"].ToString());
			this.DexStats = byte.Parse(row["Dex"].ToString());
			this.SprStats = byte.Parse(row["Spr"].ToString());
			this.IntStats = byte.Parse(row["StrInt"].ToString());
		}
	}
}