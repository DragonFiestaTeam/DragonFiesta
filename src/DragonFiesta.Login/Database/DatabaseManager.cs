using System;
using System.Linq;
using DragonFiesta.Database.EF_Models;
using DragonFiesta.Util;

namespace DragonFiesta.Login.Database
{
	public  class DatabaseManager
	{
        public static string ConnectionString { get; private set; }
        public static AccountEntities Entity { get; set; }

        public static void Initialize()
        {
            Entity = DragonFiesta.Database.EntityFactory.GetAccountEntity(Configuration.LoginConfiguration.Instance.Entity);
            ConnectionString = DragonFiesta.Database.EntityFactory.GetAccountEntityConnectionString(Configuration.LoginConfiguration.Instance.Entity);
        }

		public static AuthState Auth(string username, string pass)
		{
            
			try
			{
				using(var db = new AccountEntities(ConnectionString))
				{
				    var account = db.v_Auth.FirstOrDefault(a => a.name == username);
                    if(account == null)
                        return AuthState.NoSuchAccount;
				    if(account.password != pass)
                        return AuthState.WrongPassword;
                    if(!account.can_login)
                        return AuthState.Blocked;
				    return AuthState.Ok;
				}
			}
			catch (Exception ex)
			{
				Logs.Database.Error("Exception during auth", ex);
				return AuthState.DbError;
			}
		}
        public static bool CreateAccount(string name, string password)
        {
            return true;
        }
        public static bool VersionAllowed(ushort year, ushort version)
        {
            try
            {
                using (var db = new AccountEntities(ConnectionString))
                {
                    var v = db.versions.FirstOrDefault(ver => ver.year == year && ver.version_number == version);
                    if (v == null)
                        return false;
                    return v.allowed;
                }
            }
            catch(Exception e)
            {
                Logs.Database.Error("could not check if version allowed", e);
                return false;
            }
        }
        public static bool CheckHashes(string hash)
        {
            using (var db = new AccountEntities(ConnectionString))
            {
                var hash_row = db.hashes.FirstOrDefault(h => h.hash_string == hash);
                return hash_row != null && hash_row.allow_login;
            }
        }
	}
}
