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
            if (client.Character.Party.Count > 1)
            {
                byte messageLenght;
                string message = string.Empty;
                if (packet.TryReadByte(out messageLenght))
                {
                    if (packet.TryReadString(out message, messageLenght))
                    {
                        foreach (var member in client.Character.Party)
                        {
                            WorldClient memberClient = ClientManager.Instance.GetClientByCharname(member);
                            using (var ppacket = new Packet(SH8Type.PartyChat))
                            {
                                ppacket.WriteString(client.Character.Character.Name, 16);
                                ppacket.WriteByte(messageLenght);
                                ppacket.WriteString(message,messageLenght);
                                memberClient.SendPacket(ppacket);
                            }
                        }
                    }
                }
            }
        }
    }
}
