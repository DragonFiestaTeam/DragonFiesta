namespace DragonFiesta.Login.DataTypes
{
	public class LoginVersion
	{
		public LoginVersion(ushort pYear, ushort pVersion)
		{
			this.Year = pYear;
			this.Version = pVersion;
		}

		public override bool Equals(object other)
		{
			if(other == null) return false;
			if(other.GetType() != typeof(LoginVersion)) return false;
			var v = (LoginVersion) other;
			return this.Version == v.Version
			       && this.Year == v.Year;
		}

		public ushort Year { get; private set; }
		public ushort Version { get; private set; }
	}
}
