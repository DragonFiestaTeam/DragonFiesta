namespace DragonFiesta.Configuration.Sections
{
    public class DatabaseSettingsSection
    {
        public DatabaseSettingsSection()
        {
            this.DbOption = DatabaseOption.Unknown;
        }
        public DatabaseSettingsSection(DatabaseOption pOption, string pHost, string pUser, string pPassword, string pDbName, uint mPort, uint mMinPoolSize,uint mMaxPoolSize,string GameDb = null,string worlddb = null)
        {
            this.DbOption = pOption;
            this.Host = pHost;
            this.User = pUser;
            this.Password = pPassword;
            this.DbName = pDbName;

            this.MaxPoolSize = mMinPoolSize;
            this.MaxPoolSize = mMaxPoolSize;
            this.DatabasePort = mPort;
            this.GameDB = GameDb;
            this.WorldDb = worlddb;

        }

        public DatabaseOption DbOption;
        public string Host = "127.0.0.1";
        public string User = "root";
        public string Password = "";
        public string DbName = "fiestare_login";
        public string GameDB = "fiestare_Game";
        public string WorldDb = "fiestare_World";
        public uint DatabasePort = 3306;
        public uint MinPoolSize = 10;
        public uint MaxPoolSize = 12;
    }

    public enum DatabaseOption
    {
        Unknown = 0,
        MySQL = 1,
        MSSQL = 2
    }
}