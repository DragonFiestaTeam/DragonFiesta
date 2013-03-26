using System;
using DragonFiesta.Login.DataTypes;
using DragonFiesta.Login.Database;
using DragonFiesta.Login.Networking;
using DragonFiesta.Login.Networking.ServerHandler;
using DragonFiesta.Networking;
using DragonFiesta.Util;
using Zepheus.FiestaLib;

namespace DragonFiesta.Login.Core
{
	public class LoginManager
	{
		#region .ctor
		#endregion
		#region Properties
		public static LoginManager Instance { get; private set; }
		#endregion
		#region Methods

        public static void Initialize()
        {
                Instance = new LoginManager();
        }
        
		public void Auth(ClientBase pClient, string username, string password)
		{
			try
			{

				var client = pClient as LoginClient;
				if (client == null)
				{
					throw new UnexpectedTypeException(typeof (LoginClient), pClient.GetType());
				}
				switch (DatabaseManager.Auth(username, password))
				{
					case AuthState.Ok:
						// User is allowed to login.
						SH3Methods.AllowFiles(client);
						SH3Methods.WorldList(client);
						break;
					case AuthState.DbError:
						// Error in the Database, send error.
						SH3Methods.LoginError(client, ServerError.DatabaseError);
						break;
					case AuthState.Blocked:
						// User is blocked, send error
						SH3Methods.LoginError(client, ServerError.Blocked);
						break;
					case AuthState.WrongPassword:
					case AuthState.NoSuchAccount:
						// Invalid credentitals, send error
						SH3Methods.LoginError(client, ServerError.InvalidCredentials);
						break;
					default:
						// We dont expect anything else in here
						// So this wont ever happen without a
						// fatal error or external influence
						throw new UnexpectedEnumerationValueException();
				}
			}
			catch (Exception e)
			{
				if(!ExceptionManager.Instance.HandleException(e))
					throw;
			}
		}
        public void VersionCheck(ClientBase pClient, LoginVersion pVersion)
        {
			try
			{
				var client = pClient as LoginClient;
				if(client == null)
					throw new UnexpectedTypeException(typeof (LoginClient), pClient.GetType());

				if(client.Version == null)
				{	// client not yet send version packet.
					client.Version = pVersion;
				}
				else if(client.Version.Equals(pVersion))
				{	// client already send version-packet with the same content
					return;
				}
				else
				{	// client send two different version packets. this should not happen
					Logs.Main.ErrorFormat("client tried to login while sending 2 diff versions..");
					return;
				}
				bool allowed = DatabaseManager.VersionAllowed(pVersion.Year, pVersion.Version);
				SH3Methods.VersionAllowed(pClient, allowed);
			}
			catch(Exception e)
			{
				if (!ExceptionManager.Instance.HandleException(e))
					throw;
			}
        }
        public void FileHashes(ClientBase sender, string hash)
        {
            try
            {
                bool allowed = DatabaseManager.CheckHashes(hash);
                LoginClient client = sender as LoginClient;
                if(client == null)
                    throw new UnexpectedTypeException(typeof (LoginClient), sender.GetType());
                client.HashesAllowed = allowed;
            }
            catch (Exception e)
            {
                if (!ExceptionManager.Instance.HandleException(e))
                    throw;
            }
        }
		#endregion
    }
}
