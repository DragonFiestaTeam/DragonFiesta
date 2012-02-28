using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Zepheus.FiestaLib.Data
{
    public sealed class Teleportnpc
    {
        public ushort AnswerMap0 { get; private set; }
        public ushort AnswerMap1 { get; private set; }
        public ushort AnswerMap2 { get; private set; }
        public ushort AnswerMap3 { get; private set; }
        public ushort AnswerMap0X { get; private set; }
        public ushort AnswerMap0Y { get; private set; }
        public ushort AnswerMap1X { get; private set; }
        public ushort AnswerMap1Y { get; private set; }
        public ushort AnswerMap2X { get; private set; }
        public ushort AnswerMap2Y { get; private set; }
        public ushort AnswerMap3X { get; private set; }
        public ushort AnswerMap3Y { get; private set; }

        public static Teleportnpc Load(DataRow Row)
        {
            Teleportnpc Tele = new Teleportnpc
            {
                AnswerMap0 = ushort.Parse(Row["AnswerMap0"].ToString()),
                AnswerMap0X = ushort.Parse(Row["AnswerMap0X"].ToString()),
                AnswerMap0Y = ushort.Parse(Row["AnswerMap0Y"].ToString()),
                AnswerMap1 = ushort.Parse(Row["AnswerMap1"].ToString()),
                AnswerMap1X = ushort.Parse(Row["AnswerMap1X"].ToString()),
                AnswerMap1Y = ushort.Parse(Row["AnswerMap1Y"].ToString()),
                AnswerMap2 = ushort.Parse(Row["AnswerMap2"].ToString()),
                AnswerMap2X = ushort.Parse(Row["AnswerMap2X"].ToString()),
                AnswerMap2Y = ushort.Parse(Row["AnswerMap2Y"].ToString()),
                AnswerMap3 = ushort.Parse(Row["AnswerMap3"].ToString()),
                AnswerMap3X = ushort.Parse(Row["AnswerMap3X"].ToString()),
                AnswerMap3Y = ushort.Parse(Row["AnswerMap3Y"].ToString()),

            };
            return Tele;
        }
    }
}