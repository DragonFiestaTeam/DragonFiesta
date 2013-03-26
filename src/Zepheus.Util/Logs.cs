using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace DragonFiesta.Util
{
	public static class Logs
	{
		public static ILog Network = LogManager.GetLogger("Network");
		public static ILog Main = LogManager.GetLogger("Main");
		public static ILog InterNetwork = LogManager.GetLogger("InterNetwork");
		public static ILog Database = LogManager.GetLogger("Database");
	}
}