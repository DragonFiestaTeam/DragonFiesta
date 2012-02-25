using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zepheus.FiestaLib.Data;

namespace Zepheus.Zone.Game
{
    class RandomDrop
    {
        private Random ran = new Random();
        private Mob Monster { get; set; }
        public RandomDrop(Mob mob)
        {
            Monster = mob;
            GenerateDropGroups();
        }
        void GenerateDropGroups()
        {
            int[] frequency = new int[this.Monster.Info.Drops.Count];
            foreach (var DropInfo in Monster.Info.Drops)
            {
                int groupcounter = 0;
                for (int i = 0; i < this.Monster.Info.Drops.Count; ++i)
                {
                    int index = (int)(ran.NextDouble() * this.Monster.Info.Drops.Count);
                    frequency[index]++;
                    float RandomRate = frequency[i] * 100.0f / this.Monster.Info.Drops.Count;
                    if (RandomRate <= DropInfo.Rate && RandomRate != 0)
                    {
                      DropItems(DropInfo.Group.Items,DropInfo.Rate,DropInfo.Group.MinCount,DropInfo.Group.MaxCount);
                      groupcounter++;
                    }
                }
            }
        }
        void DropItems(List<ItemInfo> Items,float Rate,byte Mincount,byte MaxCount)
        {
            int itemCounter = 0;
            foreach (var Item in Items)
            {
                int[] frequency = new int[Items.Count];
                for (int i = 0; i < Items.Count; ++i)
                {
                    int index = (int)(ran.NextDouble() * Items.Count);
                    frequency[index]++;
                    float RandomRate = frequency[i] * 100.0f / Items.Count;
                    if (RandomRate <= Rate && RandomRate != 0 || itemCounter > MaxCount)
                    {
                        Console.WriteLine(Item.ItemID);
                        itemCounter++;
                        //Todo new drop
                    }
                    else
                    {
                        itemCounter = 0;
                       return;
                    }
                }
            }
            itemCounter = 0;
        }
    }
}
