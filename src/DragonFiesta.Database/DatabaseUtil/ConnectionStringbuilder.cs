using System.Data.EntityClient;
using System.Data.SqlClient;
using System;

namespace DragonFiesta.Database.DatabaseUtil
{
    public class ConnectionStringbuilder
    {
        public static string CreateEntityString(EntitySetting setting)
        {
            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();

            entityBuilder.Metadata = setting.Metadata;
            // Set the provider-specific connection string.
            if (setting.Option == 1)
            {
                entityBuilder.ProviderConnectionString = CreateMYSQLConnectionString(setting);
            }
            else
            entityBuilder.ProviderConnectionString = CreateMSSQLConnectionString(setting);


            entityBuilder.Provider = setting.ProviderName;

            return entityBuilder.ToString();
        }

        public static string CreateMYSQLConnectionString(EntitySetting setting)
        {
            string connectionString = String.Format("server={0};User Id={3};password={4};database={2}",
                setting.host, "3306", setting.DatabaseName, setting.Username, setting.Password);
            return connectionString;
        }
        public static string CreateMSSQLConnectionString(EntitySetting setting, bool security = false, bool multi = true)
        {
            // Initialize the connection string builder for the
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder =
                new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = setting.DataSource;
     
            sqlBuilder.InitialCatalog = setting.DataCatalog;
            if (!(string.IsNullOrEmpty(setting.Username) && string.IsNullOrEmpty(setting.Password)))
            {
                sqlBuilder.UserID = setting.Username;
                sqlBuilder.Password = setting.Password;
            }
            sqlBuilder.IntegratedSecurity = security;
            sqlBuilder.MultipleActiveResultSets = multi; //allows you to have multiple datareaders at once

            // Build the SqlConnection connection string.
            return sqlBuilder.ToString();
        }
    }
}
