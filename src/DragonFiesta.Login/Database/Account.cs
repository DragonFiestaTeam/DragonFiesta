using System;
using DragonFiesta.Util;
using DragonFiesta.Database;

namespace DragonFiesta.Login.Database
{
	public  class Account
	{
		public static AuthState Auth(string username, string pass,out byte Access_level,out int AccountID)//used in mysql
		{
            Access_level = 0;
            AccountID = -1;
			try
			{
                using (DatabaseClient pClient = DB.LoginDB.GetClient())
                {

                 SQLResult account =  pClient.Select(String.Format(Query.AuthLogin, username));
                 
                    if(account.Count == 0)
                        return AuthState.NoSuchAccount;
                    if (account.Read<String>(0,"password") != pass)
                        return AuthState.WrongPassword;
                    if (!account.Read<Boolean>(0, "can_login"))
                        return AuthState.Blocked;

                    
                    AccountID = account.Read<Int32>(0, "ID");
                    Access_level = 12;//todo get acceslevel
                    return AuthState.Ok;
                }
			}
			catch (Exception ex)
			{
				Logs.Database.Error("Exception during auth", ex);
				return AuthState.DbError;
			}
		}

        public static byte GetAccessLevel(DatabaseClient pClient, int AccountID)
        {
            SQLResult result = pClient.Select(string.Format("select * from access_levels where id={0}", AccountID));
            if (result.Count == 1)
                return result.Read<Byte>(0, "Access_level");

            return 0;
        }

        public static bool CreateAccount(string name, string password)
        {
            return true;
        }
        public static bool VersionAllowed(ushort year, ushort version)
        {
            try
            {
                using (DatabaseClient db = DB.LoginDB.GetClient())
                {
                  SQLResult v = db.Select(String.Format(Query.CheckVersion, year, version));
                    if (v.Count == 1)
                        return true;

                    return false;
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
            using (DatabaseClient db = DB.LoginDB.GetClient())
            {
                SQLResult v = db.Select(String.Format(Query.CheckVersionhash,hash));
                if (v.Count == 1)
                    return true;
            }
           return false;
        }
	}
}
