using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragonFiestaDatatbaseConverter.Data;
using DragonFiestaDatatbaseConverter.SHN;

namespace DragonFiestaDatatbaseConverter.Handlers
{
    public class ItemInfoConverter
    {
        public void ConvertItemInfo()
        {
        }
        public void LoadItemInfo()
        {


            /*   using (var file = new SHNFile(Program.folder + @"\ItemInfo.shn"))
               {
                   using (DataTableReaderEx reader = new DataTableReaderEx(file))
                   {
                       while (reader.Read())
                       {
                           ItemInfo info = ItemInfo.Load(reader);
                           if (ItemsByID.ContainsKey(info.ItemID) || ItemsByName.ContainsKey(info.InxName))
                           {
                               Log.WriteLine(LogLevel.Warn, "Duplicate item found ID: {0} ({1}).", info.ItemID, info.InxName);

                               continue;
                           }
                           ItemsByID.Add(info.ItemID, info);
                           ItemsByName.Add(info.InxName, info);

                           if (effectcache.ContainsKey(info.InxName))
                           {
                               if (info.Type != ItemType.Useable)
                               {
                                   Log.WriteLine(LogLevel.Warn, "Invalid useable item: {0} ({1})", info.ItemID, info.InxName);
                                   continue;
                               }
                               ItemUseEffectInfo effectinfo = effectcache[info.InxName];
                               effectinfo.ID = info.ItemID;
                               ItemUseEffects.Add(effectinfo.ID, effectinfo);
                           }
                       }
                   }
               }
               effectcache.Clear();
               Log.WriteLine(LogLevel.Info, "Loaded {0} items.", ItemsByID.Count);
           }*/

        }
    }
}