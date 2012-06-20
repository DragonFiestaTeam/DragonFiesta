using System;
using Zepheus.World.Networking;
namespace Zepheus.World.Data
{
    public class CommercialReqest
    {
        #region .ctor
        public CommercialReqest(WorldClient pFrom, string pToClient)
		{

			this.CrationTimeStamp = DateTime.Now;
			this.pToCommercialClient = ClientManager.Instance.GetClientByCharname(pToClient);
			this.pFromCommercialClient = pFrom;

		}
		#endregion
        #region Properties
		public DateTime CrationTimeStamp { get; private set; }
		public WorldClient pToCommercialClient { get; private set; }
		public WorldClient pFromCommercialClient { get; private set; }
		#endregion

    }
}
