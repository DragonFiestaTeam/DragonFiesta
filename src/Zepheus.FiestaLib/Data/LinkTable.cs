using System;
using Zepheus.Util;
using System.Data;

namespace Zepheus.FiestaLib.Data
{
 	public sealed class LinkTable
	{
		public String argument { get; private set; }
		public String MapServer { get; private set; }
		public String MapClient { get; private set; }
		public Int32 Coord_X { get; private set; }
		public Int32 Coord_Y { get; private set; }
		public Int16 Direct { get; private set; }
		public Int16 LevelFrom { get; private set; }
		public Int16 LevelTo { get; private set; }
		public Byte Party { get; private set; }

		public static LinkTable Load(DataRow Row)
		{
        
			LinkTable info = new LinkTable
			{
               argument = (string)Row["argument"],
				MapServer = (string)Row["MapServer"],
				MapClient = (string)Row["MapClient"],
				Coord_X =(int)Row["Coord_X"],
				Coord_Y = (int)Row["Coord_Y"],
                Direct = (short)(Int32)Row["Direct"],
				LevelFrom = (short)(Int32)Row["LevelFrom"],
				LevelTo = (short)(Int32)Row["LevelTo"],
				Party = (byte)(sbyte)Row["Party"],
			};
			return info;
		}
	}
}
