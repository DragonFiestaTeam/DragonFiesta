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
			using (var ppacket = new InterPacket(InterHeader.GetParty))
			{
				ppacket.WriteString(client.Character.character.Name, 16);
				WorldConnector.Instance.SendPacket(ppacket);
			}
			if (!client.Character.HealthThreadState)
			{
				ParameterizedThreadStart pts = new ParameterizedThreadStart(PartyHealthThread);
				Thread healthThread = new Thread(pts);
				healthThread.Start(client);
				client.Character.HealthThreadState = true;
			}
		}
		public static void PartyHealthThread(object charname)
		{
			ZoneClient @char = charname as ZoneClient;
			uint sp = @char.Character.SP;
			uint hp = @char.Character.HP;
			byte level = @char.Character.Level;
			uint maxHP = @char.Character.MaxHP;
			uint maxSP = @char.Character.MaxSP;
			ushort mapID = @char.Character.MapID;
			while (@char.Character.IsInParty && @char.Character.MapID == mapID)
			{
				if (sp != @char.Character.SP || hp != @char.Character.HP)
				{
					sp = @char.Character.SP;
					hp = @char.Character.HP;
					level = @char.Character.Level;
					foreach (var partyMember in @char.Character.Party)
					{
						ZoneClient memChar = ClientManager.Instance.GetClientByName(partyMember.Key);
						Sector charSector = memChar.Character.Map.GetSectorByPos(memChar.Character.Position);
						if (partyMember.Key != @char.Character.Name && charSector == @char.Character.MapSector)
						{
							using (var packet = new Packet(SH14Type.UpdatePartyMemberStats))
							{
								packet.WriteByte(1);//unk
								packet.WriteString(@char.Character.Name, 16);
								packet.WriteUInt(@char.Character.HP);
								packet.WriteUInt(@char.Character.SP);
								memChar.SendPacket(packet);
							}
						}

					}

				}
				else if (maxHP != @char.Character.MaxHP || maxSP != @char.Character.MaxSP || level != @char.Character.Level)
				{
					maxHP = @char.Character.MaxHP;
					maxSP = @char.Character.MaxSP;
					level = @char.Character.Level;
					foreach (var partyMember in @char.Character.Party)
					{
						if (partyMember.Key != @char.Character.Name)
						{
							ZoneClient memChar = ClientManager.Instance.GetClientByName(partyMember.Key);
							using (var ppacket = new Packet(SH14Type.UpdatePartyMemberStats))
							{
								ppacket.WriteByte(1);//unk
								ppacket.WriteString(@char.Character.Name, 16);
								ppacket.WriteUInt(@char.Character.HP);
								ppacket.WriteUInt(@char.Character.SP);
								memChar.SendPacket(ppacket);
							}
							using (var ppacket = new Packet(SH14Type.SetMemberStats))//when character has levelup in group
							{
								ppacket.WriteByte(1);
								ppacket.WriteString(@char.Character.Name, 16);
								ppacket.WriteByte((byte)@char.Character.Job);
								ppacket.WriteByte(@char.Character.Level);
								ppacket.WriteUInt(@char.Character.MaxHP);//maxhp
								ppacket.WriteUInt(@char.Character.MaxSP);//MaxSP
								ppacket.WriteByte(1);
								memChar.SendPacket(ppacket);
							}
						}
					}

				}
				Thread.Sleep(3000);
			}
			@char.Character.Party.Clear();
			@char.Character.HealthThreadState = false;
			@char.Character.IsInParty = false;
		}
	}
}