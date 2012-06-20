using System;
using Zepheus.Zone.Networking;
namespace Zepheus.Zone.Data
{
    public class CommercialReqest
    {
        #region .ctor
        public CommercialReqest(ZoneClient pFrom, string pToClient)
		{

			this.CrationTimeStamp = DateTime.Now;
			this.pToCommercialClient = ClientManager.Instance.GetClientByCharName(pToClient);
			this.pFromCommercialClient = pFrom;

		}
		#endregion
        #region Properties
		public DateTime CrationTimeStamp { get; private set; }
		public ZoneClient pToCommercialClient { get; private set; }
		public ZoneClient pFromCommercialClient { get; private set; }
		#endregion

    }
}
