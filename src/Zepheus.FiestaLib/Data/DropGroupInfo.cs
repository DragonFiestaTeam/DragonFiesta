using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zepheus.Util;

namespace Zepheus.FiestaLib.Data
{
    public sealed class DropGroupInfo
    {
        public string GroupID { get; private set; }
        public byte MinCount { get; private set; }
        public byte MaxCount { get; private set; }
        public List<ItemInfo> Items { get; private set; }

        public static DropGroupInfo Load(DataRow Row)
        {
            DropGroupInfo info = new DropGroupInfo()
            {
                GroupID = (string)Row["GroupID"],
                MinCount = (byte)Row["MinCount"],
                MaxCount = (byte)Row["MaxCount"],
                Items = new List<ItemInfo>()
            };
            return info;
        }
    }
}
