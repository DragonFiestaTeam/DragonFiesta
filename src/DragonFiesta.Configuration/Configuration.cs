using System.IO;
using System.Xml.Serialization;
using DragonFiesta.Configuration.Sections;

namespace DragonFiesta.Configuration
{
    public abstract class Configuration
    {
        #region Ignore
        [XmlIgnore]
        public static Configuration Instance { get; set; }
        #endregion

        public DatabaseSettingsSection DatabaseSettings = new DatabaseSettingsSection(DatabaseOption.MySQL, "localhost",
                                                             "root", "root", "fiestare_login",3306,10,12);

        public DatabaseSettingsSection WorldAndGameSettings = new DatabaseSettingsSection(DatabaseOption.MySQL, "localhost",
                                                             "root", "root", "fiestare_login", 3306, 10, 12, "fiestare_game", "fiestare_World");

        public ServerSettingsSection ServerSettings = new ServerSettingsSection("0.0.0.0", 9000, false);
        public QueueSettings QueueSettings = new QueueSettings();

        public void CreateDefaultFolder()
        {
            string folder = "Configuration";
            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public void WriteXml()
        {
            CreateDefaultFolder();
            string path = Path.Combine(
                "Configuration",
                string.Format("{0}.xml", Instance.GetType()));
            var writer = new XmlSerializer(Instance.GetType());
            var file = new StreamWriter(path);
            writer.Serialize(file, this);
            file.Close();
        }
        public bool ReadXml()
        {
            string path = Path.Combine(
                "Configuration",
                string.Format("{0}.xml", Instance.GetType()));
            
            if (File.Exists(path))
            {
                var reader = new XmlSerializer(Instance.GetType());
                var file = new StreamReader(path);
                var cfg = (Configuration)reader.Deserialize(file);
                Instance = cfg;
                file.Close();
                return true;
            }
            else return false;
        }
    }
}
