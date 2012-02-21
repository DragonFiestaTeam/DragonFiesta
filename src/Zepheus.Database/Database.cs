namespace Zepheus.Database
{
	public class Database
	{
		public string DatabaseName;
		public uint PoolMinSize;
		public uint PoolMaxSize;

		public Database(string databaseName, uint poolMinSize, uint poolMaxSize)
		{
			DatabaseName = databaseName;

			PoolMinSize = poolMinSize;
			PoolMaxSize = poolMaxSize;
		}
	}
}
