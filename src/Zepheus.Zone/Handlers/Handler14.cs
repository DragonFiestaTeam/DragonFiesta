using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zepheus.Database;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Data;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.Zone.Game;
using Zepheus.Zone.Data;
using Zepheus.Zone.Handlers;
using Zepheus.Zone.InterServer;
using Zepheus.Zone.Networking;
using Zepheus.Zone.Security;
using Zepheus.Database.Storage;
using MySql.Data.MySqlClient;
using Zepheus.InterLib.Networking;
using System.Threading;

namespace Zepheus.Zone.Handlers
{
 public  class Handler14
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
             Thread HealthThread = new Thread(pts);
             HealthThread.Start(client);
             client.Character.HealthThreadState = true;
         }
     }
     public static void PartyHealthThread(object charname)
     {
      ZoneClient Char = charname as ZoneClient;
       uint SP = Char.Character.SP;
       uint  HP = Char.Character.HP;
       byte Level = Char.Character.Level;
       uint MaxHP = Char.Character.MaxHP;
         uint MaxSP = Char.Character.MaxSP;
         ushort MapID = Char.Character.MapID;
        while (Char.Character.IsInParty && Char.Character.MapID == MapID)
        {
          
            if (SP != Char.Character.SP || HP != Char.Character.HP)
            {
                SP = Char.Character.SP;
                HP = Char.Character.HP;
                Level = Char.Character.Level;
                foreach (var PartyMember in Char.Character.Party)
                {
                    ZoneClient MemChar = ClientManager.Instance.GetClientByName(PartyMember.Key);
                    Sector CharSector = MemChar.Character.Map.GetSectorByPos(MemChar.Character.Position);
                    if (PartyMember.Key != Char.Character.Name && CharSector == Char.Character.MapSector)
                    {

                        using (var packet = new Packet(SH14Type.UpdatePartyMemberStats))
                        {
                            packet.WriteByte(1);//unk
                            packet.WriteString(Char.Character.Name, 16);
                            packet.WriteUInt(Char.Character.HP);
                            packet.WriteUInt(Char.Character.SP);
                            MemChar.SendPacket(packet);
                        }
                    }

                }

            }
            else if (MaxHP != Char.Character.MaxHP || MaxSP != Char.Character.MaxSP ||Level != Char.Character.Level)
            {
                MaxHP = Char.Character.MaxHP;
                MaxSP = Char.Character.MaxSP;
                Level = Char.Character.Level;
                foreach (var PartyMember in Char.Character.Party)
                {
                    if (PartyMember.Key != Char.Character.Name)
                    {
                        ZoneClient MemChar = ClientManager.Instance.GetClientByName(PartyMember.Key);
                        using (var ppacket = new Packet(SH14Type.UpdatePartyMemberStats))
                        {
                            ppacket.WriteByte(1);//unk
                            ppacket.WriteString(Char.Character.Name, 16);
                            ppacket.WriteUInt(Char.Character.HP);
                            ppacket.WriteUInt(Char.Character.SP);
                            MemChar.SendPacket(ppacket);
                        }
                        using (var ppacket = new Packet(SH14Type.SetMemberStats))//when character has levelup in group
                        {
                            ppacket.WriteByte(1);
                            ppacket.WriteString(Char.Character.Name, 16);
                            ppacket.WriteByte((byte)Char.Character.Job);
                            ppacket.WriteByte((byte)Char.Character.Level);
                            ppacket.WriteUInt(Char.Character.MaxHP);//maxhp
                            ppacket.WriteUInt(Char.Character.MaxSP);//MaxSP
                            ppacket.WriteByte(1);
                            MemChar.SendPacket(ppacket);
                        }
                    }
                }

            }
            Thread.Sleep(3000);
        }
        Char.Character.Party.Clear();
        Char.Character.HealthThreadState = false;
        Char.Character.IsInParty = false;
     }
    }
}