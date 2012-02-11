using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.World.Networking;
using Zepheus.Database.Storage;
using MySql.Data.MySqlClient;
using Zepheus.InterLib.Networking;
using Zepheus.World.InterServer;
using Zepheus.World.Handlers;
using Zepheus.World.Data;
using Zepheus.World.Security;
using Zepheus.Database;

namespace Zepheus.World.Handlers
{
    public sealed class Handler14
    {
        [PacketHandler(CH14Type.PartyReqest)]
        public static void PartyReqest(WorldClient client, Packet packet)
        {

//            ZoneConnection z = Program.GetZoneByMap(client.Character.Character.PositionInfo.Map);
            // client.Character.IsInParty = false;

            /*if (Program.Zones.TryGetValue(zoneid, out z))
            {
            }*/
            string InviteChar;
            if (packet.TryReadString(out InviteChar, 0x10))
            {
                WorldClient InvideClient = ClientManager.Instance.GetClientByCharname(InviteChar);//use latter for members handling
                using (var ppacket = new Packet(SH14Type.PartyInvide))
                {
                    ppacket.WriteString(client.Character.Character.Name, 0x10);
                    //client.SendPacket(ppacket);
                    InvideClient.SendPacket(ppacket);
                }
            }
        }
        [PacketHandler(CH14Type.PartyLeave)]
        public static void PartyLeave(WorldClient client, Packet packet)
        {
                if (client.Character.Party.Count > 2)
                {
                    WorldClient ClearClient = null;
                    List<WorldClient> Membersclients = new List<WorldClient>();
                    List<string> NewPartyList = new List<string>();
                    foreach (string Memb in client.Character.Party)
                    {
                        WorldClient Client = ClientManager.Instance.GetClientByCharname(Memb);
                        if (Client.Character.Character.Name != client.Character.Character.Name)
                        {
                            Membersclients.Add(Client);
                            NewPartyList.Add(Client.Character.Character.Name);
                        }
                        else
                        {
                            ClearClient = Client;
                        }
                    }
                    try
                    {
                        if (client.Character.IsPartyMaster == true)
                        {
                            client.Character.IsPartyMaster = false;
                            WorldClient newMaster =  Membersclients[0];
                            newMaster.Character.IsPartyMaster = true;
                            foreach (WorldClient clientm in Membersclients)
                            {
                                using (var ppacket = new Packet(SH14Type.ChangePartyMaster))
                                {
                                    ppacket.WriteString(newMaster.Character.Character.Name, 16);
                                    ppacket.WriteUShort(1352);
                                   clientm.SendPacket(ppacket);
                                }
                            }
                        }
                        switch (ClearClient.Character.Party.Count)
                        {
                            case 3:
                                Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE groups SET member1='" + NewPartyList[1] + "', member2='' WHERE master='" + NewPartyList[0] + "'");
                                break;
                            case 4:
                                Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE groups SET member2='" + NewPartyList[2] + "', member3='' WHERE master='" + NewPartyList[0] + "'");
                                break;
                            case 5:
                                Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE groups SET member3='" + NewPartyList[3] + "', member4='' WHERE master='" + NewPartyList[0] + "'");
                                break;
                            default:
                                Exception Eror = new Exception("Our of Partyrange Members");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Zepheus.Util.Log.WriteLine(Util.LogLevel.Exception, ex.ToString());
                    }
                    foreach (WorldClient clientm in Membersclients)
                    {
                        //todo Packet
                        using (var ppacket = new Packet(SH14Type.KickPartyMember))
                        {
                            ppacket.WriteString(client.Character.Character.Name, 16);
                            ppacket.WriteUShort(1345);
                            clientm.SendPacket(ppacket);
                        }
                        clientm.Character.Party = NewPartyList;
                    }
                    using (var ppacket = new Packet(SH14Type.KickPartyMember))
                    {
                        ppacket.WriteString(client.Character.Character.Name, 16);
                        ppacket.WriteUShort(1345);
                        ClearClient.SendPacket(ppacket);
                    }
                    ZoneConnection z = Program.GetZoneByMap(ClearClient.Character.Character.PositionInfo.Map);
                    using (var interleave = new InterPacket(InterHeader.RemovePartyMember))
                    {
                        interleave.WriteString(ClearClient.Character.Character.Name, 16);
                        interleave.WriteString(ClearClient.Character.Character.Name, 16);
                        z.SendPacket(interleave);
                    }
                    ClearClient.Character.Party.Clear();
                }
                else
                {
                    WorldClient mClient1 = ClientManager.Instance.GetClientByCharname(client.Character.Party[0]);
                    WorldClient mClient2 = ClientManager.Instance.GetClientByCharname(client.Character.Party[1]);
                    using (var ppacket = new Packet(SH14Type.KickPartyMember))
                    {
                        ppacket.WriteString(mClient1.Character.Character.Name, 16);
                        ppacket.WriteUShort(1345);
                        mClient2.SendPacket(ppacket);
                    }

                    using (var ppacket = new Packet(SH14Type.KickPartyMember))
                    {
                        ppacket.WriteString(mClient2.Character.Character.Name, 16);
                        ppacket.WriteUShort(1345);
                        mClient1.SendPacket(ppacket);
                    }
                    ZoneConnection z = Program.GetZoneByMap(mClient1.Character.Character.PositionInfo.Map);
                    using (var interleave = new InterPacket(InterHeader.RemovePartyMember))
                    {
                        interleave.WriteString(mClient1.Character.Character.Name, 16);
                        interleave.WriteString(mClient1.Character.Character.Name, 16);
                        z.SendPacket(interleave);
                    }
                    ZoneConnection z2 = Program.GetZoneByMap(mClient2.Character.Character.PositionInfo.Map);
                    using (var interleave = new InterPacket(InterHeader.RemovePartyMember))
                    {
                        interleave.WriteString(mClient2.Character.Character.Name, 16);
                        interleave.WriteString(mClient2.Character.Character.Name, 16);
                        z2.SendPacket(interleave);
                    }
                    if (mClient1.Character.IsPartyMaster == true)
                    {
                        Program.DatabaseManager.GetClient().ExecuteQuery("DELETE FROM groups WHERE master='" + mClient1.Character.Character.Name+ "'");
                    }
                    else if (mClient2.Character.IsPartyMaster == true)
                    {
                        Program.DatabaseManager.GetClient().ExecuteQuery("DELETE FROM groups WHERE master='" + mClient2.Character.Character.Name + "'");
                    }
                    ///*:todo inter server
                    //
                    //*/
                    mClient1.Character.Party.Clear();
                    mClient2.Character.Party.Clear();
                }
            }
        
        [PacketHandler(CH14Type.PartyDecline)]
        public static void PartyDecline(WorldClient client, Packet packet)
        {
            string InviteChar;
            if (packet.TryReadString(out InviteChar, 0x10))
            {
                /*WorldClient InvideClient = ClientManager.Instance.GetClientByCharname(InviteChar);
                packet.WriteString(InvideClient.Character.Character.Name);
                packet.WriteUShort(1217);
                InvideClient.SendPacket(packet);*/
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
                    ppacket.WriteHexAsBytes("");
                    ppacket.WriteString("haha", 16);
                    ppacket.WriteString("1234567890123456789012345678901234567890123456", 46);
                    ppacket.WriteHexAsBytes("00 00 00 00 44 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 8C 8E CD 00 88 49 DF 4E B3 08 4C 00 78 26 43 00 01 00 00 00 5A 68 42 00 18 FE 64 02 40 55 DF 4E 08 27 4D 00 94 FF 64 02 24 00 00 00 BD 68 42 00 87 BE");
                }
                /*ppacket.Fill(1, 0);
          ppacket.WriteUShort(5120);
          ppacket.WriteInt(list.Count);
          ppacket.Fill(4, 0);
          foreach (var stat in list)
          {
              ppacket.WriteString(stat.Key, 16);
              ppacket.WriteString(stat.Value, 46);
              ppacket.Fill(85, 0);
               
          }*/
                list.Clear();
                client.SendPacket(ppacket);
            }


        }
        [PacketHandler(CH14Type.KickPartyMember)]
        public static void KickPartyMember(WorldClient client, Packet packet)
        {
            if (client.Character.IsPartyMaster == true)
            {
                string RemoveName;
                if (packet.TryReadString(out RemoveName, 16))
                {
                    if (client.Character.Party.Count > 2)
                    {
                        WorldClient ClearClient = null;
                        List<WorldClient> Membersclients = new List<WorldClient>();
                        List<string> NewPartyList = new List<string>();
                        foreach (string Memb in client.Character.Party)
                        {
                            WorldClient Client = ClientManager.Instance.GetClientByCharname(Memb);
                            if (Client.Character.Character.Name != RemoveName)
                            {
                                Membersclients.Add(Client);
                                NewPartyList.Add(Client.Character.Character.Name);
                            }
                            else
                            {
                                ClearClient = Client;
                            }
                        }
                        using (var ppacket = new Packet(SH14Type.KickPartyMember))
                        {
                            ppacket.WriteString(RemoveName, 16);
                            ppacket.WriteUShort(1345);
                            ClearClient.SendPacket(ppacket);
                        }
                        try
                        {
                            switch (ClearClient.Character.Party.Count)
                            {
                                case 3:
                                    Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE groups SET member1='" + NewPartyList[1] + "', member2='' WHERE master='" + client.Character.Character.Name + "'");
                                    break;
                                case 4:
                                    Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE groups SET member2='" + NewPartyList[2] + "', member3='' WHERE master='" + client.Character.Character.Name + "'");
                                    break;
                                case 5:
                                    Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE groups SET member3='" + NewPartyList[3] + "', member4='' WHERE master='" + client.Character.Character.Name + "'");
                                    break;
                                default:
                                 Exception Eror =   new Exception("Our of Partyrange Members");
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Zepheus.Util.Log.WriteLine(Util.LogLevel.Exception, ex.ToString());
                        }
                        foreach (WorldClient clientm in Membersclients)
                        {
                            //todo Packet
                            using (var ppacket = new Packet(SH14Type.KickPartyMember))
                            {
                                ppacket.WriteString(RemoveName, 16);
                                ppacket.WriteUShort(1345);
                                clientm.SendPacket(ppacket);
                            }
                            clientm.Character.Party = NewPartyList;
                        }
                        //:todo Interserver Packet
                        //
                        //*/
                        ZoneConnection Member1 = Program.GetZoneByMap(client.Character.Character.PositionInfo.Map);
                        using (var inter = new InterPacket(InterHeader.RemovePartyMember))
                        {
                            inter.WriteString(ClearClient.Character.Character.Name, 16);
                            inter.WriteString(ClearClient.Character.Character.Name, 16);
                            Member1.SendPacket(inter);
                        }
                        ClearClient.Character.Party.Clear();
                    }
                    else
                    {
                        WorldClient mClient1 = ClientManager.Instance.GetClientByCharname(client.Character.Party[0]);
                        WorldClient mClient2 = ClientManager.Instance.GetClientByCharname(client.Character.Party[1]);
                        using (var ppacket = new Packet(SH14Type.KickPartyMember))
                        {
                            ppacket.WriteString(mClient1.Character.Character.Name, 16);
                            ppacket.WriteUShort(1345);
                            mClient2.SendPacket(ppacket);
                        }
                        ZoneConnection z1 = Program.GetZoneByMap(mClient1.Character.Character.PositionInfo.Map);
                        ZoneConnection z2 = Program.GetZoneByMap(mClient2.Character.Character.PositionInfo.Map);
                    
                        using (var ppacket = new Packet(SH14Type.KickPartyMember))
                        {
                           
                            ppacket.WriteString(mClient2.Character.Character.Name, 16);
                            ppacket.WriteUShort(1345);
                            mClient1.SendPacket(ppacket);
                        }
                        using (var interleave = new InterPacket(InterHeader.RemovePartyMember))
                        {
                            interleave.WriteString(mClient1.Character.Character.Name,16);
                            interleave.WriteString(mClient1.Character.Character.Name, 16);
                            z1.SendPacket(interleave);
                        }
                        using (var interleave = new InterPacket(InterHeader.RemovePartyMember))
                        {
                            interleave.WriteString(mClient2.Character.Character.Name, 16);
                            interleave.WriteString(mClient2.Character.Character.Name, 16);
                            z2.SendPacket(interleave);
                        }
                        Program.DatabaseManager.GetClient().ExecuteQuery("DELETE FROM groups WHERE master='" + client.Character.Character.Name + "'");
                        ///*:todo inter server
                        //
                        //*/
                        mClient1.Character.Party.Clear();
                        mClient2.Character.Party.Clear();
                    }
                }
            
            }
            }
        [PacketHandler(CH14Type.ChangePartyDrop)]
        public static void ChangeDropMode(WorldClient client, Packet packet)
        {
            byte DropState;
            if (packet.TryReadByte(out DropState))
            {
                if (client.Character.IsPartyMaster == true)
                {
                    foreach (var m in client.Character.Party)
                    {
                        WorldClient MemberClient = ClientManager.Instance.GetClientByCharname(m);
                        using (var ppacket = new Packet(SH14Type.ChangePartyDrop))
                        {
                            ppacket.WriteByte(DropState);
                            MemberClient.SendPacket(ppacket);
                        }
                    }
                }
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
                    Console.WriteLine("change master faled");
                }
            }
        }
        [PacketHandler(CH14Type.PartyAccept)]
        public static void AcceptParty(WorldClient client, Packet packet)
        {
            string InviteChar;
            if (packet.TryReadString(out InviteChar, 0x10))
            {
                WorldClient InvideClient = ClientManager.Instance.GetClientByCharname(InviteChar);
                DataTable Data = null;
                using (DatabaseClient dbClient = Program.DatabaseManager.GetClient())
                {
                    Data = dbClient.ReadDataTable("SELECT * FROM groups WHERE binary `Master` = '" + InvideClient.Character.Character.Name + "'");
                }

                if (Data != null)
                {
                    if (Data.Rows.Count > 0)
                    {
                        foreach (DataRow Row in Data.Rows)
                        {
                            string Member1 = (string)Row["Member1"];
                            string Member2 = (string)Row["Member2"];
                            string Member3 = (string)Row["Member3"];
                            string Member4 = (string)Row["Member4"];
                            List<string> Members = new List<string>();
                            if (InvideClient.Character.IsPartyMaster == true && Member1 != "")
                            {
                                Members.Insert(0, InvideClient.Character.Character.Name);
                                Members.Insert(1, Member1);
                                if (Member2 != "")
                                {
                                    Members.Insert(2, Member2);
                                }
                                else
                                {
                                    string member = Members.Find(m => m == client.Character.Character.Name);
                                    if (member == null)
                                    {
                                        Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE groups SET Member2='" + client.Character.Character.Name + "' WHERE binary `Master` = '" + InvideClient.Character.Character.Name + "'");
                                        Members.Insert(2, client.Character.Character.Name);
                                    }
                                }
                                if (Member3 != "")
                                {
                                    Members.Insert(3, Member3);
                                }
                                else
                                {
                                    string member = Members.Find(m => m == client.Character.Character.Name);
                                    if (member == null)
                                    {
                                        Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE groups SET Member3='" + client.Character.Character.Name + "' WHERE binary `Master` = '" + InvideClient.Character.Character.Name + "'");
                                        Members.Insert(3, client.Character.Character.Name);
                                    }
                                }
                                if (Member4 != "")
                                {
                                    Members.Insert(4, Member4);
                                }
                                else
                                {
                                    string member = Members.Find(m => m == client.Character.Character.Name);
                                    if (member == null)
                                    {
                                        Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE groups SET Member4='" + client.Character.Character.Name + "' WHERE binary `Master` = '" + InvideClient.Character.Character.Name + "'");
                                        Members.Insert(4, client.Character.Character.Name);
                                    }
                                }
                                ZoneConnection zone = Program.GetZoneByMap(client.Character.Character.PositionInfo.Map);
                                using (var inter = new InterPacket(InterHeader.AddPartyMember))
                                {
                                    inter.WriteString(InvideClient.Character.Character.Name, 16);
                                    inter.WriteString(client.Character.Character.Name,16);
                                    zone.SendPacket(inter);
                                }
                                using (var ppacket = new Packet(SH14Type.PartyList))
                                {
                                    foreach (string Member in Members)//senpacket all members
                                    {
                                        WorldClient MemberClient = ClientManager.Instance.GetClientByCharname(Member);
                                        ppacket.WriteByte((byte)Members.Count);
                                        foreach (string partym in Members)
                                        {

                                            ppacket.WriteString(partym, 16);
                                            ppacket.WriteByte(1);//unk

                                        }
                                        MemberClient.Character.Party = Members;
                                        MemberClient.SendPacket(ppacket);
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        Program.DatabaseManager.GetClient().ExecuteQuery("INSERT INTO groups (Master,Member1) VALUES ('" + InvideClient.Character.Character.Name + "','" + client.Character.Character.Name + "')");
                        List<string> Members = new List<string>();
                        ZoneConnection Master = Program.GetZoneByMap(InvideClient.Character.Character.PositionInfo.Map);
                        ZoneConnection Member1 = Program.GetZoneByMap(client.Character.Character.PositionInfo.Map);
                        using (var inter = new InterPacket(InterHeader.AddPartyMember))
                        {
                            inter.WriteString(InvideClient.Character.Character.Name, 16);
                            inter.WriteString(client.Character.Character.Name, 16);
                            Master.SendPacket(inter);
                        }
                        using (var inter = new InterPacket(InterHeader.AddPartyMember))
                        {
                            inter.WriteString(InvideClient.Character.Character.Name, 16);
                            inter.WriteString(InvideClient.Character.Character.Name, 16);
                            Master.SendPacket(inter);
                        }
                        using (var inter = new InterPacket(InterHeader.AddPartyMember))
                        {
                            inter.WriteString(client.Character.Character.Name, 16);
                            inter.WriteString(InvideClient.Character.Character.Name,16);
                            Member1.SendPacket(inter);
                        }
                        using (var inter = new InterPacket(InterHeader.AddPartyMember))
                        {
                            inter.WriteString(client.Character.Character.Name, 16);
                            inter.WriteString(client.Character.Character.Name, 16);
                            Member1.SendPacket(inter);
                        }
                        Members.Insert(0, InvideClient.Character.Character.Name);
                        InvideClient.Character.IsPartyMaster = true;
                        Members.Insert(1, client.Character.Character.Name);
                        InvideClient.Character.Party = Members;
                        client.Character.Party = Members;
                        using (var ppacket = new Packet(SH14Type.PartyInvideAsMaster))
                        {
                            ppacket.WriteString(client.Character.Character.Name, 16);
                            ppacket.WriteHexAsBytes("C1 04");
                            InvideClient.SendPacket(ppacket);
                        }
                        using (var ppacket = new Packet(SH14Type.PartyList))
                        {
                            ppacket.WriteByte(2);
                            ppacket.WriteString(InvideClient.Character.Character.Name, 16);
                            ppacket.WriteByte(1);//unk
                            ppacket.WriteString(client.Character.Character.Name, 16);
                            ppacket.WriteByte(1);//unk
                            client.SendPacket(ppacket);
                        }
                    }
                }
            }
        }
    }
}