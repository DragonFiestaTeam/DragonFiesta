using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Zone.Networking;
using System;

namespace Zepheus.Zone.Handlers
{
	public class Handler14
	{
		[PacketHandler(CH14Type.PartyInviteGame)]
		public static void GetPartyListFromCharserer(ZoneClient client, Packet packet)
		{
            //TODO: Sniff and implement!
            throw new NotImplementedException();
		}
	}
}