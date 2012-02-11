using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.World.Networking;
using Zepheus.Database.Storage;
using MySql.Data.MySqlClient;

namespace Zepheus.World.Handlers
{
    public class Handler8
    {
        [PacketHandler(CH8Type.ChatParty)]
        public static void PartyChat(WorldClient client, Packet packet)
        {
            if (client.Character.Party.Count > 1)
            {
                byte MessageLenght;
                string Message = string.Empty;
                if (packet.TryReadByte(out MessageLenght))
                {
                    if (packet.TryReadString(out Message, MessageLenght))
                    {
                        foreach (var member in client.Character.Party)
                        {
                            WorldClient MemberClient = ClientManager.Instance.GetClientByCharname(member);
                            using (var ppacket = new Packet(SH8Type.PartyChat))
                            {
                                ppacket.WriteString(client.Character.Character.Name, 16);
                                ppacket.WriteByte(MessageLenght);
                                ppacket.WriteString(Message,MessageLenght);
                                MemberClient.SendPacket(ppacket);
                            }
                        }
                    }
                }
            }
        }
    }
}
