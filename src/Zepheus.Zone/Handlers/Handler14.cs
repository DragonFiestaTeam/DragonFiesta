using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Zone.Game;
using Zepheus.Zone.InterServer;
using Zepheus.Zone.Networking;
using Zepheus.InterLib.Networking;
using System.Threading;

namespace Zepheus.Zone.Handlers
{
	public class Handler14
	{
		[PacketHandler(CH14Type.PartyInviteGame)]
		public static void GetPartyListFromCharserer(ZoneClient client, Packet packet)
		{
			RequestGroup(client.Character.Name);
			//if (!client.Character.HealthThreadState)
			//{
			//    ParameterizedThreadStart pts = new ParameterizedThreadStart(PartyHealthThread);
			//    Thread healthThread = new Thread(pts);
			//    healthThread.Start(client);
			//    client.Character.HealthThreadState = true;
			//}
		}

		private static void RequestGroup(string pName)
		{
			using (var packet = new InterPacket(InterHeader.GetParty))
			{
				packet.WriteString(pName, 16);
				WorldConnector.Instance.SendPacket(packet);
			}
		}
		public static void PartyHealthThread(object charname)
		{
			ZoneClient character = charname as ZoneClient;
			uint sp = character.Character.SP;
			uint hp = character.Character.HP;
			byte level = character.Character.Level;
			uint maxHP = character.Character.MaxHP;
			uint maxSP = character.Character.MaxSP;
			ushort mapID = character.Character.MapID;
			while (character.Character.IsInParty && character.Character.MapID == mapID)
			{
                // HP/SP changed
				if (sp != character.Character.SP || hp != character.Character.HP)
				{
					sp = character.Character.SP;
					hp = character.Character.HP;
					level = character.Character.Level;
                    // Announce
					foreach (var partyMember in character.Character.Party)
					{
						ZoneClient memChar = ClientManager.Instance.GetClientByName(partyMember.Key);
						Sector charSector = memChar.Character.Map.GetSectorByPos(memChar.Character.Position);
                        // ?
						if (partyMember.Key != character.Character.Name && charSector == character.Character.MapSector)
						{
                            // Update stats-packet
							using (var packet = new Packet(SH14Type.UpdatePartyMemberStats))
							{
								packet.WriteByte(1);//unk
								packet.WriteString(character.Character.Name, 16);
								packet.WriteUInt(character.Character.HP);
								packet.WriteUInt(character.Character.SP);
								memChar.SendPacket(packet);
							}
						}

					}

				}
                // max hp or level
				else if (maxHP != character.Character.MaxHP || maxSP != character.Character.MaxSP || level != character.Character.Level)
				{
					maxHP = character.Character.MaxHP;
					maxSP = character.Character.MaxSP;
					level = character.Character.Level;
                    // announce
					foreach (var partyMember in character.Character.Party)
					{
						if (partyMember.Key != character.Character.Name)
						{
							ZoneClient memChar = ClientManager.Instance.GetClientByName(partyMember.Key);
							using (var ppacket = new Packet(SH14Type.UpdatePartyMemberStats))
							{
								ppacket.WriteByte(1);//unk
								ppacket.WriteString(character.Character.Name, 16);
								ppacket.WriteUInt(character.Character.HP);
								ppacket.WriteUInt(character.Character.SP);
								memChar.SendPacket(ppacket);
							}
							using (var ppacket = new Packet(SH14Type.SetMemberStats))//when character has levelup in group
							{
								ppacket.WriteByte(1);
								ppacket.WriteString(character.Character.Name, 16);
								ppacket.WriteByte((byte)character.Character.Job);
								ppacket.WriteByte(character.Character.Level);
								ppacket.WriteUInt(character.Character.MaxHP);//maxhp
								ppacket.WriteUInt(character.Character.MaxSP);//MaxSP
								ppacket.WriteByte(1);
								memChar.SendPacket(ppacket);
							}
						}
					}

				}
				Thread.Sleep(3000);
			}
			character.Character.Party.Clear();
			character.Character.HealthThreadState = false;
			character.Character.IsInParty = false;
		}
	}
}