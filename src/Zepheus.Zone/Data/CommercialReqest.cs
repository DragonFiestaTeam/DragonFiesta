using System;
using Zepheus.Zone.Game;
using Zepheus.Zone.Networking;
namespace Zepheus.Zone.Data
{
    public class CommercialReqest
    {
        #region .ctor
        public CommercialReqest(ZoneCharacter pFrom, ushort ToMapObjectID)
        {
            if (pFrom.SelectedObject.MapObjectID == ToMapObjectID)
            {
                this.CrationTimeStamp = DateTime.Now;
                this.pToCommercialClient = pFrom.SelectedObject as ZoneCharacter;
                this.pFromCommercialClient = pFrom;
                this.MapID = pFrom.MapID;
            }
        }
		#endregion
        #region Properties
		public DateTime CrationTimeStamp { get; private set; }
		public ZoneCharacter pToCommercialClient { get; private set; }
		public ZoneCharacter pFromCommercialClient { get; private set; }
        public ushort MapID { get; private set; }
		#endregion

    }
}
