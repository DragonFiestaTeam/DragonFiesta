namespace DragonFiesta.Configuration.Sections
{
    public class ServerSettingsSection
    {
        public ServerSettingsSection()
        {
            
        }
        public ServerSettingsSection(string pListenIp, ushort pPort)
        {
            this.ListenIP = pListenIp;
            this.Port = pPort;
        }
        public ServerSettingsSection(string pListenIp, ushort pPort, bool pAutoGet, string pExternIP = "") : this(pListenIp, pPort)
        {
            this.ExternalIP = pExternIP;
            this.AutoGetExternIP = pAutoGet;
        }

        public string ListenIP;
        public ushort Port;
        public string ExternalIP;
        public bool AutoGetExternIP;
    }
}