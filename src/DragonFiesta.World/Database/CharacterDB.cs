using System;
using DragonFiesta.Util;
using DragonFiesta.Database;
using DragonFiesta.World.Game;
using DragonFiesta.World.Networking;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace DragonFiesta.World.Database
{
    public  class CharacterDB
    {
        public static Dictionary<int, WorldCharacter> GetCharacterListFromDatabase(int AccountID)
        {
            Dictionary<int, WorldCharacter> CharacterList = new Dictionary<int, WorldCharacter>();

            using (DatabaseClient DBClient = DB.WorldDB.GetClient())
            {
                SQLResult pResult = DBClient.Select(Query.GetCharactersByAccountID, AccountID);
                if(pResult.Count <= 5)//new character max is 5
                {
                    for (int r = 0; r < pResult.Count; r++)
                    {
                       Byte  slot = pResult.Read<Byte>(r, "Slot");
                       WorldCharacter pChar = new WorldCharacter(pResult, r);
                       CharacterList.Add(slot, pChar);
                    }
                }
            }
            return CharacterList;
        }

        public static bool IsNameInUse(string Name)
        {
            using (DatabaseClient DBClient = DB.WorldDB.GetClient())
            {
                SQLResult pResult = DBClient.Select(string.Format(Query.CheckNameInUse,Name));
                if (pResult.Count == 1)
                    return true;
            }
            return false;
        }

        public static int GenerateCharacterID()//todo make optimal
        {
            using (DatabaseClient pClient = DB.WorldDB.GetClient())
            {
                Dictionary<int, int> key = new Dictionary<int, int>();
                SQLResult res = pClient.Select(Query.GetAllCharID);
                IEnumerable<int> keyRange = Enumerable.Range(1, int.MaxValue);

                for (int r = 0; r < res.Count; r++)
                {
                    key.Add(res.Read<Int32>(r,"CharacterID"),0);
                }
                var freeKeys = keyRange.Except(key.Keys);

                return freeKeys.First();
            }
        }
        public static byte GenerateCharacterSlot(WorldClient pClient)
        {
            IEnumerable<int> keyRange = Enumerable.Range(0, 4);

            var freekeys = keyRange.Except(pClient.CharacterList.Keys);
            return (byte)freekeys.First();
        }
    }
}
