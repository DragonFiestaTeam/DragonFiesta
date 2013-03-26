namespace DragonFiesta.Login.Database
{
	public enum AuthState
	{
		Ok,
		UnkownError,
		NoSuchAccount,
		WrongPassword,
		Blocked,

		DbError
	}
}