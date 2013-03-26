using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonFiesta.Data;
using DragonFiesta.FiestaLib;

namespace DragonFiesta.DataProvider
{
    public partial class DataProvider
    {
        public Dictionary<ushort, Map> maps = new Dictionary<ushort, Map>();
        public Dictionary<ClassID,List<CharacterClass>> CharacterBaseStats = new Dictionary<ClassID,List<CharacterClass>>();
        public List<StonePowerInfo> StoneList = new List<StonePowerInfo>();
    }
}
