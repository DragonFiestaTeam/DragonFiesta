using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Zepheus.FiestaLib.Data
{
    public class AbStateInfo
    {
        public ushort ID { get; set; }
        public string InxName { get; set; }

        public static AbStateInfo LoadFromDatabase(DataRow row)
        {
			// todo: load
	        return null;
        }
    }
}
