namespace Zepheus.Util
{
	public enum InitializationStage
	{
		Metadata = 0,
		Settings = 1,
		DataStore = 2,
		Services = 3,
		SpecialDataProvider = 4,
		Worker = 5,
		Clients = 6,
		Networking = 7
	}
}