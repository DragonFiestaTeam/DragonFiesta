using System;
using System.Collections.Generic;
using System.Data;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.World.Networking;
using Zepheus.InterLib.Networking;
using Zepheus.World.InterServer;
using Zepheus.Database;

namespace Zepheus.World.Handlers
{
	public sealed class Handler14
	{
		[PacketHandler(CH14Type.PartyReqest)]
		public static void PartyReqest(WorldClient client, Packet packet)
		{
			string invitedChar;
			if (packet.TryReadString(out invitedChar, 16))
			{
				GroupManager.Instance.Invite(client, invitedChar);
			}
		}
		[PacketHandler(CH14Type.PartyLeave)]
		public static void PartyLeave(WorldClient client, Packet packet)
		{
			GroupManager.Instance.LeaveParty(client);
		}
		[PacketHandler(CH14Type.PartyDecline)]
		public static void PartyDecline(WorldClient client, Packet packet)
		{
			string InviteChar;
			if (packet.TryReadString(out InviteChar, 0x10))
			{
				GroupManager.Instance.DeclineInvite(client, InviteChar);
			}
		}
		[PacketHandler(CH14Type.PartyMaster)]
		public static void MasterList(WorldClient client, Packet packet)
		{
			Dictionary<string, string> list = new Dictionary<string, string>();
			list.Add("Char1", "hier ist Char1");
			list.Add("Char2", "hier ist Char2");
			using (var ppacket = new Packet(SH14Type.GroupList))
			{
				ppacket.WriteHexAsBytes("00 00 14 01 01 00 01 00 00 00");
				ppacket.WriteInt(list.Count);
				foreach (var stat in list)
				{
					// Note - teh fuck?
					ppacket.WriteHexAsBytes("");
					ppacket.WriteString("haha", 16);
					ppacket.WriteString("1234567890123456789012345678901234567890123456", 46);
					ppacket.WriteHexAsBytes("00 00 00 00 44 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 8C 8E CD 00 88 49 DF 4E B3 08 4C 00 78 26 43 00 01 00 00 00 5A 68 42 00 18 FE 64 02 40 55 DF 4E 08 27 4D 00 94 FF 64 02 24 00 00 00 BD 68 42 00 87 BE");
				}

				list.Clear();
				client.SendPacket(ppacket);
			}


		}
		[PacketHandler(CH14Type.KickPartyMember)]
		public static void KickPartyMember(WorldClient client, Packet packet)
		{
			string RemoveName;
			if (packet.TryReadString(out RemoveName, 16))
			{
				if(!client.Character.Group.HasMember(RemoveName))
					return;

				GroupManager.Instance.KickMember(client, RemoveName);
			}
		}
		[PacketHandler(CH14Type.ChangePartyDrop)]
		public static void ChangeDropMode(WorldClient client, Packet packet)
		{
			byte dropState;
			if (packet.TryReadByte(out dropState)) {
				client.Character.Group.ChangeDropType(client.Character, dropState);
			}
		}
		[PacketHandler(CH14Type.ChangePartyMaster)]
		public static void ChangePartyMaster(WorldClient client, Packet packet)
		{
			string Mastername;
			if (packet.TryReadString(out Mastername, 16))
			{
				client.Character.IsPartyMaster = false;
				List<string> NewMember = new List<string>();
				try
				{
					NewMember.Add(Mastername);
					foreach (string Memb in client.Character.Party)
					{
						if (Memb != Mastername)
						{
							NewMember.Add(Memb);
						}
						else
						{
						}
					}
					switch (NewMember.Count)
					{
						case 2:
							Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE groups SET Member1='" + NewMember[0] + "'  WHERE binary `Master` = '" + client.Character.Character.Name + "'");
							break;
						case 3:
							Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE groups SET Member1='" + NewMember[1] + "', Member2='"+NewMember[2]+"', master='"+Mastername+"'  WHERE binary `Master` = '" + client.Character.Character.Name+ "'");
							break;
						case 4:
							Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE groups SET Member1='" + NewMember[1] + "', Member2='" + NewMember[2] + "', Member3='"+NewMember[3]+"', master='" + Mastername + "'  WHERE binary `Master` = '" + client.Character.Character.Name + "'");
							break;
						case 5:
							Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE groups SET Member1='" + NewMember[1] + "', Member2='"+NewMember[2]+"',Member3='"+NewMember[3]+"',Member4='"+NewMember[4]+"' master='"+Mastername+"'  WHERE binary `Master` = '" + client.Character.Character.Name+ "'");
						 break;

					}
					foreach (string Memb2 in NewMember)
					{
						WorldClient MemberClient = ClientManager.Instance.GetClientByCharname(Memb2);
						if (MemberClient.Character.Character.Name == Mastername)
						{
							MemberClient.Character.IsPartyMaster = true;
						}
						
						MemberClient.Character.Party = NewMember;
	 
						using (var ppacket = new Packet(SH14Type.ChangePartyMaster))
						{
							ppacket.WriteString(Mastername, 16);
							ppacket.WriteUShort(1352);
							MemberClient.SendPacket(ppacket);
						}

					}
				}
				catch
				{
					Console.WriteLine("change master failed");
				}
			}
		}
		[PacketHandler(CH14Type.PartyAccept)]
		public static void AcceptParty(WorldClient client, Packet packet)
		{
			string InviteChar;
			if (packet.TryReadString(out InviteChar, 16))
			{
				GroupManager.Instance.AcceptInvite(client, InviteChar);
			}
		}
	}
}