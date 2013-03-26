namespace DragonFiesta.Configuration.Sections
{
    public class DatabaseSettingsSection
    {
        public DatabaseSettingsSection()
        {
            this.DbOption = DatabaseOption.Unknown;
        }
        public DatabaseSettingsSection(DatabaseOption pOption, string pHost, string pUser, string pPassword, string pDbName)
        {
            this.DbOption = pOption;
            this.Host = pHost;
            this.User = pUser;
            this.Password = pPassword;
            this.DbName = pDbName;
        }

        public DatabaseOption DbOption;
        public string Host = "127.0.0.1";
        public string User = "root";
        public string Password = "";
        public string DbName = "fiestare_login";
 
    }

    public enum DatabaseOption
    {
        Unknown = 0,
        MySQL = 1,
        MSSQL = 2
    }
}