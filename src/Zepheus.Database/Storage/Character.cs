using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Data;

namespace Zepheus.Database.Storage
{
   public class Character
    {

       public int ID { get; set; }
       public int AccountID { get; set; }
       public string Name { get; set; }
       public byte Slot { get; set; }
       public byte CharLevel { get; set; }
       public byte Job { get; set; }
       public int HP { get; set; }
       public int SP { get; set; }
       public int instanzeID { get; set; }
       public short HPStones { get; set; }
       public short SPStones { get; set; }
       public long Exp { get; set; }
       public int Fame { get; set; }
       public long Money { get; set; }
       public LookInfo LookInfo = new LookInfo();
       public byte StatPoints { get; set; }
       public byte UsablePoints { get; set; }
       public byte[] QuickBar { get; set; }
       public byte[] Shortcuts { get; set; }
       public byte[] QuickBarState { get; set; }
       public PositionInfo PositionInfo = new PositionInfo();
       public byte[] GameSettings { get; set; }
       public byte[] ClientSettings { get; set; }
       public CharacterStats CharacterStats = new CharacterStats();
       public int GuildID { get; set; }
       public List<EquipInfo> EquiptetItem = new List<EquipInfo>();
       public List<EquipInfo> Equips = new List<EquipInfo>();
       public List<DatabaseItem> Items = new List<DatabaseItem>();
     //  public List<DatabaseItem> Inventory { get; set; }
     //  public Dictionary<short, EquipInfo> Equips { get; set; }
       public List<DatabaseSkill> SkillList = new List<DatabaseSkill>();

    }

   public class PositionInfo
   {
       public int XPos { get; set; }
       public int YPos { get; set; }
       public ushort Map { get; set; }

       public void ReadFromDatabase(DataRow Row)
       {
           this.Map = (ushort)Row["Map"];
           this.XPos = int.Parse(Row["XPos"].ToString());
           this.YPos =  int.Parse(Row["YPos"].ToString());
       }
   }
   public class CharacterStats
   {
       public byte StrStats { get; set; }
       public byte EndStats { get; set; }
       public byte DexStats { get; set; }
       public byte IntStats { get; set; }
       public byte SprStats { get; set; }

       public void ReadFromDatabase(DataRow Row)
       {
           this.StrStats = byte.Parse(Row["Str"].ToString());
           this.EndStats = byte.Parse(Row["End"].ToString());
           this.DexStats = byte.Parse(Row["Dex"].ToString());
           this.SprStats = byte.Parse(Row["Spr"].ToString());
           this.IntStats = byte.Parse(Row["StrInt"].ToString());
       }
   }
   public class LookInfo
   {
       public byte Hair { get; set; }
       public byte HairColor { get; set; }
       public byte Face { get; set; }
       public bool Male { get; set; }

       public void ReadFromDatabase(DataRow Row)
       {
           this.Male = DataStore.ReadMethods.EnumToBool(Row["Male"].ToString());
           this.Hair = byte.Parse(Row["Hair"].ToString()); ;
           this.HairColor =byte.Parse(Row["HairColor"].ToString());
           this.Face = byte.Parse(Row["Face"].ToString());
       }
   }
}
