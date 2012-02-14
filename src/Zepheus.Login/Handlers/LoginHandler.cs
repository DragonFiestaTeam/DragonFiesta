using System.Linq;
using System;
using Zepheus.Database;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Login.Networking;
using Zepheus.Login.InterServer;
using Zepheus.Util;
using MySql.Data.MySqlClient;
using System.Data;
namespace Zepheus.Login.Handlers
{
	public sealed class LoginHandler
	{
		[PacketHandler(CH3Type.Version)]
		public static void VersionInfo(LoginClient pClient, Packet pPacket)
		{
			ushort year;
			ushort version;
			if (!pPacket.TryReadUShort(out year) ||
				!pPacket.TryReadUShort(out version))
			{
				Log.WriteLine(LogLevel.Warn, "Invalid client version.");
				pClient.Disconnect();
				return;
			}
			Log.WriteLine(LogLevel.Debug, "Client version {0}:{1}.", year, version);
			using (Packet response = new Packet(SH3Type.VersionAllowed))
			{
				response.WriteShort(1);
				pClient.SendPacket(response);
			}
		}

		[PacketHandler(CH3Type.Login)]
		public static void Login(LoginClient pClient, Packet pPacket)
		{
			string MD5 = pPacket.ReadStringForLogin(34);
			char[] tmpUserAndPass = MD5.ToCharArray();
			string ClientPassword = "";

			string username = "";

			for (int i = 0; i < 18; i++)
			{
				username += tmpUserAndPass[i].ToString().Replace("\0", "");
			}
			for (int i = 18; i < 34; i++)
			{
				//is not hash is password from client

				ClientPassword += tmpUserAndPass[i].ToString().Replace("\0", "");
			}
			Log.WriteLine(LogLevel.Debug, "{0} tries to login.", username);
			/* Error codes
			* 0x42 - Unkown Error
			* 0x43 - DB Error
			* 0x44 - Auth Failed
			* 0x45 - Please check ID or password
			* 0x46 - DB Error
			* 0x47 - The ID has been blocked
			* 0x48 - World server maintenance
			* 0x49 - Timeout
			* 0x4a - Login Failed
			* 0x4b - Please accept the agreement. */
			bool Banned = false;
			DataTable LoginData = null;
			using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
			{
				LoginData = dbClient.ReadDataTable("SELECT `ID`, `Username`, `Password`, `Admin`, `Blocked` FROM accounts WHERE Username= '" + username + "'");
			}
			if (LoginData != null)
			{
				if (LoginData.Rows.Count > 0)
				{
					foreach (DataRow Row in LoginData.Rows)
					{
						string uIsername = (string)Row["Username"];
						string password = (string)Row["Password"];
						if (password == ClientPassword)
						{

							Banned = Database.DataStore.ReadMethods.EnumToBool(Row["Blocked"].ToString());
							if (Banned == true)
							{
								SendFailedLogin(pClient, ServerError.BLOCKED);

							}
							else if (ClientManager.Instance.IsLoggedIn(uIsername))
							{
								Log.WriteLine(LogLevel.Warn, "{0} is trying dual login. Disconnecting.", uIsername);
								pClient.Disconnect();

								break;
							}
							else
							{
								pClient.Username = uIsername;
								pClient.IsAuthenticated = true;
								pClient.Admin = (byte)Row["Admin"];
								pClient.AccountID = int.Parse(Row["ID"].ToString());
								AllowFiles(pClient, true);
								WorldList(pClient, false);
							}
						}
						else
						{
							SendFailedLogin(pClient, ServerError.AGREEMENT_MISSING);
						}
					}
				}
				else
				{
					SendFailedLogin(pClient, ServerError.AGREEMENT_MISSING);
				}
			}
		}
		[PacketHandler(CH3Type.WorldReRequest)]
		public static void WorldReRequestHandler(LoginClient pClient, Packet pPacket)
		{
			if (!pClient.IsAuthenticated)
			{
				Log.WriteLine(LogLevel.Warn, "Invalid world list request.");
				return;
			}
			WorldList(pClient, true);
		}

		[PacketHandler(CH3Type.FileHash)]
		public static void FileHash(LoginClient pClient, Packet pPacket)
		{
			string hash;
			if (!pPacket.TryReadString(out hash))
			{
				Log.WriteLine(LogLevel.Warn, "Empty filehash received.");
				SendFailedLogin(pClient, ServerError.EXCEPTION);
			}
			else
			{
				//allowfiles here fucks shit up?
			}
		}

		[PacketHandler(CH3Type.WorldSelect)]
		public static void WorldSelectHandler(LoginClient pClient, Packet pPacket)
		{
			if (!pClient.IsAuthenticated || pClient.IsTransferring)
			{
				Log.WriteLine(LogLevel.Warn, "Invalid world select request.");
				SendFailedLogin(pClient, ServerError.EXCEPTION);
				return;
			}

			byte id;
			if (!pPacket.TryReadByte(out id))
			{
				Log.WriteLine(LogLevel.Warn, "Invalid world select.");
				return;
			}
			WorldConnection world;
			if (WorldManager.Instance.Worlds.TryGetValue(id, out world))
			{
				switch (world.Status)
				{
					case WorldStatus.MAINTENANCE:
						Log.WriteLine(LogLevel.Warn, "{0} tried to join world in maintentance.", pClient.Username);
						SendFailedLogin(pClient, ServerError.SERVER_MAINTENANCE);
						return;
					case WorldStatus.OFFLINE:
						Log.WriteLine(LogLevel.Warn, "{0} tried to join offline world.", pClient.Username);
						SendFailedLogin(pClient, ServerError.SERVER_MAINTENANCE);
						return;
					default: Log.WriteLine(LogLevel.Debug, "{0} joins world {1}", pClient.Username, world.Name); break;
				}
				string hash = System.Guid.NewGuid().ToString().Replace("-", "");
				
				
				world.SendTransferClientFromWorld(pClient.AccountID, pClient.Username, pClient.Admin, pClient.Host, hash);
				Log.WriteLine(LogLevel.Debug, "Transferring login client {0}.", pClient.Username);
				pClient.IsTransferring = true;
				SendWorldServerIP(pClient, world, hash);
			}
			else
			{
				Log.WriteLine(LogLevel.Warn, "{0} selected invalid world.", pClient.Username);
				return;
			}
		}

		private static void InvalidClientVersion(LoginClient pClient)
		{
			using (Packet pack = new Packet(SH3Type.IncorrectVersion))
			{
				pack.Fill(10, 0);
				pClient.SendPacket(pack);
			}
		}

		private static void SendFailedLogin(LoginClient pClient, ServerError pError)
		{
			using (Packet pack = new Packet(SH3Type.Error))
			{
				pack.WriteUShort((ushort)pError);
				pClient.SendPacket(pack);
			}
		}

		private static void AllowFiles(LoginClient pClient, bool pIsOk)
		{
			using (Packet pack = new Packet(SH3Type.FilecheckAllow))
			{
				pack.WriteBool(pIsOk);
				pClient.SendPacket(pack);
			}
		}

		private static void WorldList(LoginClient pClient, bool pPing)
		{
			using (var pack = new Packet(pPing ? SH3Type.WorldistResend : SH3Type.WorldlistNew))
			{
				pack.WriteByte(11);//worldmax count
				//pack.WriteByte((byte)WorldManager.Instance.WorldCount);
				foreach (var world in WorldManager.Instance.Worlds.Values)
				{
					pack.WriteByte(world.ID);
					pack.WriteString(world.Name, 16);
					pack.WriteByte((byte)world.Status);
				}
				for (int i = 0; i < (11 - WorldManager.Instance.Worlds.Count); i++)
				{
					pack.WriteByte((byte)i);
					pack.WriteString("DUMMY" + i, 16);
					pack.WriteByte((byte)WorldStatus.OFFLINE);
				}
				pClient.SendPacket(pack);
			}
		}

		private static void SendWorldServerIP(LoginClient pClient, WorldConnection wc, string hash)
		{
			using (var pack = new Packet(SH3Type.WorldServerIP))
			{
				pack.WriteByte((byte)wc.Status);

				string ip = pClient.Host == "127.0.0.1" ? "127.0.0.1" : wc.IP;
				pack.WriteString(wc.IP, 16);

				pack.WriteUShort(wc.Port);
				pack.WriteString(hash, 32);
				pack.Fill(32, 0);
				pClient.SendPacket(pack);
			}
		}
	}
}
