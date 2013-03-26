using System;
using System.Collections.Generic;
using System.Data;

namespace Zepheus.FiestaLib.Data
{
    public class MasterRewardItem : MasterRewardState
    {
        public byte Level { get; private set; }
        public Job Job { get; private set; }

        public MasterRewardItem()
        {
        }
        public MasterRewardItem(DataRow row)
        {
			// TODO: Load
	        return;
        }
    }
}
