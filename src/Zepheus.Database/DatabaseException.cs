using System;

namespace Zepheus.Database
{
	public class DatabaseException : Exception
	{
		public DatabaseException(string sMessage) : base(sMessage)
		{
		}
	}
}