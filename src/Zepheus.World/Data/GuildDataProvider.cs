using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zepheus.Database;
using Zepheus.Util;

namespace Zepheus.World.Data
{
    public class GuildDataProvider
    {
        public Dictionary<byte, uint> AcademyLevelUpPoints;
        public static GuildDataProvider Instance { get; set; }

        [InitializerMethod]
        public static bool Init()
        {
            Instance = new GuildDataProvider();
            Log.WriteLine(LogLevel.Info, "GuildDataProvider Initialsize");
            return true;
        }
        public GuildDataProvider()
        {
            LoadAcademyLevelUpPonts();
        }
        private void LoadAcademyLevelUpPonts()
        {
            AcademyLevelUpPoints = new Dictionary<byte, uint>();
            DataTable AcademyPoints = null;

            using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
            {
               AcademyPoints = dbClient.ReadDataTable("SELECT * FROM AcademyLevelPoints");
            }

            if (AcademyPoints!= null)
            {
                foreach (DataRow row in AcademyPoints.Rows)
                {
                   AcademyLevelUpPoints.Add((byte)row["Level"],(uint)row["Points"]);
                }
            }
        }
    }
}
