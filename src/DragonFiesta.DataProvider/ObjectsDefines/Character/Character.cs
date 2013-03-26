using System;
using DragonFiesta.Database;
using DragonFiesta.FiestaLib;
using DragonFiesta.Networking;

namespace DragonFiesta.Data
{
    public partial class Character : MapObject
    {
        public Acount AccountInfo { get; set; }

        public virtual ClientBase pClient { get; set; }

        public int CharacterID { get; set; }

        public string Name { get; set; }

        public byte CharacterLevel { get; set; }

        public byte CharacterSlot { get; set; }
        
        public LookInfo Look { get; set; }

        public CharacterClass Class { get; set; }

        public PositionInfo Position { get; set; }

        public CharacterStats NormalStats { get; set; }// Basesis with equipment

        public CharacterStats FullStats { get; set; }//with boni and buffs etc

        public CharacterStatePoints StatePoints { get; set; }

        public StonePowerInfo  Stones { get; set; }

        public uint CurrentHP { get; set; }

        public uint CurrentSP { get; set; }

         public ushort CurrentSPStones { get; set; }

         public ushort CurrentHPStones { get; set; }

        public ulong Money { get; set; }

        public ulong EXP { get; set; }

        public uint Fame { get; set; }

        public uint KillPoints { get; set; }

        public byte[] QuickBar { get; set; }

        public byte[] Shortcuts { get; set; }

        public byte[] QuickBarState { get; set; }

        public byte[] GameSettings { get; set; }

        public byte[] ClientSettings { get; set; }

        public void ReadFromDatabase(SQLResult pResult, int row)
        {

            this.CharacterID = pResult.Read<Int32>(row, "CharacterID");
            this.CharacterLevel = pResult.Read<Byte>(row, "Level");

            this.CharacterSlot = pResult.Read<Byte>(row, "Slot");
            this.Name = pResult.Read<String>(row, "Name");
            this.Money = pResult.Read<UInt64>(row, "Money");


            this.Position = new PositionInfo();
            this.StatePoints = new CharacterStatePoints();
            this.Look = new LookInfo();
            this.Class = new CharacterClass();
            this.Class.ClassStats = new CharacterStats();
            this.Stones = new StonePowerInfo();
            this.FullStats = new CharacterStats();
            this.NormalStats = new CharacterStats();

            this.Class.pClassID = (ClassID)pResult.Read<Byte>(row, "Job");

            CharacterStats Stats = DataProvider.DataProvider.Instance.GetClassStats(this.Class.pClassID, this.CharacterLevel);
            StonePowerInfo inf = DataProvider.DataProvider.Instance.GetStoneInfo(this.CharacterLevel, this.Class.pClassID);
            if (Stats != null && inf != null )
            {
                this.NormalStats = Stats;
                this.Stones = inf;
                this.Position.ReadFromDatabase(pResult, row);
                this.StatePoints.ReadCharacterStatsFromDatabase(pResult, row);
                this.StatePoints.ReadCurrentHPAndSP(pResult, row);
                this.Look.ReadFromDatabase(pResult, row);
                Data.StatsCalculator.CalculateAllStats(this);
            }
            else
                new Exception("Failed Character Load exepction Character Stats not Found");
        }

        public void Create()
        {
            using (DatabaseClient pClient = DB.WorldDB.GetClient())
            {
                pClient.RunQuery(string.Format(Query.CreateCharacter,this.AccountInfo.ID,this.Name,this.CharacterSlot,(byte)this.Class.pClassID,this.Look.Male,this.Look.Hair,this.Look.HairColor,this.Look.Face,this.CharacterID));
            }
        }

        public void Delete()
        {
            using (DatabaseClient pClient = DB.WorldDB.GetClient())
            {
                pClient.RunQuery(string.Format(Query.DeleteCharacter, this.CharacterSlot, this.AccountInfo.ID));
            }
        }
    }
}
