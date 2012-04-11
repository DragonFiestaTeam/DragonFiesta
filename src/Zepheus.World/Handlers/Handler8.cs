using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.World.Networking;

namespace Zepheus.World.Handlers
{
	public class Handler8
	{
		[PacketHandler(CH8Type.ChatParty)]
		public static void PartyChat(WorldClient client, Packet packet)
		{
			if (client.Character.Group == null)
				return;
			
			byte msgLen;
			string msg = "";
			
			if(!packet.TryReadByte(out msgLen))
			if(!packet.TryReadString(out msg, msgLen))
				return;

			client.Character.Group.Chat(client, msg);

		}
	}
}
