using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragonFiesta.Database
{
    public class EntitySetting
    {
        public byte Option { get; set; } // 1= mysql 2= mssql
        public string DataSource { get; set; }
        public string DataCatalog { get; set; }
        public string Metadata { get; set; }
        public string ProviderName { get; set; }

        public string host { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
