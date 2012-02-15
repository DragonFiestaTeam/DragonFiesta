using System;
using System.Collections.Generic;
using System.Data;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Data;
using Zepheus.InterLib.Networking;
using Zepheus.Services.DataContracts;
using Zepheus.Util;
using Zepheus.Zone.Networking;
using Zepheus.FiestaLib.Networking;
using Zepheus.Database;
using System.Threading;

namespace Zepheus.Zone.InterServer
{
    public sealed class InterHandler
    {
        [InterPacketHandler(InterHeader.ASSIGNED)]
        public static void HandleAssigned(WorldConnector lc, InterPacket packet)
        {
            string name;
            byte id;
            ushort port;
            int mapidcout;
            if (!packet.TryReadByte(out id) || !packet.TryReadString(out name) ||
                !packet.TryReadUShort(out port) || !packet.TryReadInt(out mapidcout))
            {
                return;
            }

            Program.serviceInfo = new ZoneData
            {
          
                ID = id,
                Port = port,
                MapsToLoad = new List<FiestaLib.Data.MapInfo>()
            };

            for (int i = 0; i < mapidcout; i++)
            {
                ushort mapid, viewrange;
                string shortname, fullname;
                int regenx, regeny;
                byte kingdom;
                if (!packet.TryReadUShort(out mapid) || !packet.TryReadString(out shortname) || !packet.TryReadString(out fullname) || !packet.TryReadInt(out regenx) || !packet.TryReadInt(out regeny) || !packet.TryReadByte(out kingdom) || !packet.TryReadUShort(out viewrange))
                {
                    break;
                }
                Program.serviceInfo.MapsToLoad.Add(new MapInfo(mapid, shortname, fullname, regenx, regeny, kingdom, viewrange)); ;
            }

            Console.Title = "Zepheus.Zone[" + id + "]";
            Log.WriteLine(LogLevel.Info, "Successfully linked with worldserver. [Zone: {0} | Port: {1}]", id, port);
            ZoneAcceptor.Load();
        }
        [InterPacketHandler(InterHeader.SendParty)]
        public static void HandlePartyNames(WorldConnector lc, InterPacket packet)
        {
            byte count;
            if (packet.TryReadByte(out count))
            {
                string charname;
                if (packet.TryReadString(out charname, 16))
                {
                    for (int j = 0; j < count; j++)
                    {
                        string Member;
                        if (packet.TryReadString(out Member, 16))
                        {
                            ZoneClient MemberClient = ClientManager.Instance.GetClientByName(Member);
                            if(!MemberClient.Character.Party.ContainsKey(Member))
                            {
                            MemberClient.Character.Party.Add(Member, MemberClient);
                            MemberClient.Character.IsInParty = true;
                            ParameterizedThreadStart pts = new ParameterizedThreadStart(Handlers.Handler14.PartyHealthThread);
                            Thread HealthThread = new Thread(pts);
                            HealthThread.Start(MemberClient);
                            MemberClient.Character.HealthThreadState = true;
                            }
                        }
                    }
                }
            }
        }
        [InterPacketHandler(InterHeader.ZONECLOSED)]
        public static void HandleZoneClosed(WorldConnector lc, InterPacket packet)
        {
            byte id;
            if (!packet.TryReadByte(out id))
            {
                return;
            }
            ZoneData zd;
            if (Program.Zones.TryRemove(id, out zd))
            {
                Log.WriteLine(LogLevel.Info, "Removed zone {0} from zones (disconnected)", id);
            }
        }

        [InterPacketHandler(InterHeader.ZONEOPENED)]
        public static void HandleZoneOpened(WorldConnector lc, InterPacket packet)
        {
            byte id;
            string ip;
            ushort port;
            int mapcount;
            if (!packet.TryReadByte(out id) || !packet.TryReadString(out ip) || !packet.TryReadUShort(out port) || !packet.TryReadInt(out mapcount))
            {
                return;
            }

            List<MapInfo> maps = new List<MapInfo>();
            for (int j = 0; j < mapcount; j++)
            {
                ushort mapid, viewrange;
                string shortname, fullname;
                int regenx, regeny;
                byte kingdom;
                if (!packet.TryReadUShort(out mapid) || !packet.TryReadString(out shortname) || !packet.TryReadString(out fullname) || !packet.TryReadInt(out regenx) || !packet.TryReadInt(out regeny) || !packet.TryReadByte(out kingdom) || !packet.TryReadUShort(out viewrange))
                {
                    break;
                }
                maps.Add(new MapInfo(mapid, shortname, fullname, regenx, regeny, kingdom, viewrange)); ;
            }

            ZoneData zd;
            if (!Program.Zones.TryGetValue(id, out zd))
            {
                zd = new ZoneData();
            }
            zd.ID = id;
            zd.IP = ip;
            zd.Port = port;
            zd.MapsToLoad = maps;
            Program.Zones[id] = zd;
            Log.WriteLine(LogLevel.Info, "Added zone {0} to zonelist. {1}:{2}", zd.ID, zd.IP, zd.Port);
        }
        [InterPacketHandler(InterHeader.AddPartyMember)]
        public static void AddPartyMember(WorldConnector lc, InterPacket packet)
        {
            string charname;
            string AddName;
            if (packet.TryReadString(out charname, 16))
            {
                if (packet.TryReadString(out AddName, 16))
                {
                    ZoneClient Memberclient = ClientManager.Instance.GetClientByName(charname);
                    if (!Memberclient.Character.Party.ContainsKey(AddName))
                    {
                        ZoneClient Addclient = ClientManager.Instance.GetClientByName(AddName);
                        Memberclient.Character.Party.Add(AddName, Addclient);
                        Addclient.Character.IsInParty = true;
                        if (Addclient.Character.HealthThreadState == false)
                        {
                            foreach (var cl in Addclient.Character.Party.Values)
                            {

                                if (Addclient.Character.MapSector == cl.Character.MapSector)
                                {
                                    if (cl == Addclient)
                                    {
                                        using (var ppacket = new Packet(SH14Type.UpdatePartyMemberStats))
                                        {
                                            ppacket.WriteByte(1);//unk
                                            ppacket.WriteString(cl.Character.Name, 16);
                                            ppacket.WriteUInt(cl.Character.HP);
                                            ppacket.WriteUInt(cl.Character.SP);
                                            Addclient.Character.Client.SendPacket(ppacket);
                                        }
                                        using (var ppacket = new Packet(SH14Type.SetMemberStats))//when character has levelup in group
                                        {
                                            ppacket.WriteByte(1);
                                            ppacket.WriteString(cl.Character.Name, 16);
                                            ppacket.WriteByte((byte)cl.Character.Job);
                                            ppacket.WriteByte(cl.Character.Level);
                                            ppacket.WriteUInt(cl.Character.MaxHP);//maxhp
                                            ppacket.WriteUInt(cl.Character.MaxSP);//MaxSP
                                            ppacket.WriteByte(1);
                                            Addclient.Character.Client.SendPacket(ppacket);
                                        }
                                    }
                                    else
                                    {

                                        using (var ppacket = new Packet(SH14Type.UpdatePartyMemberStats))
                                        {
                                            ppacket.WriteByte(1);//unk
                                            ppacket.WriteString(Addclient.Character.Name, 16);
                                            ppacket.WriteUInt(Addclient.Character.HP);
                                            ppacket.WriteUInt(Addclient.Character.SP);
                                            cl.Character.Client.SendPacket(ppacket);
                                        }
                                        using (var ppacket = new Packet(SH14Type.SetMemberStats))//when character has levelup in group
                                        {
                                            ppacket.WriteByte(1);
                                            ppacket.WriteString(Addclient.Character.Name, 16);
                                            ppacket.WriteByte((byte)cl.Character.Job);
                                            ppacket.WriteByte(Addclient.Character.Level);
                                            ppacket.WriteUInt(Addclient.Character.MaxHP);//maxhp
                                            ppacket.WriteUInt(Addclient.Character.MaxSP);//MaxSP
                                            ppacket.WriteByte(1);
                                            cl.Character.Client.SendPacket(ppacket);
                                        }
                                    }
                                }
                            }
                            Addclient.Character.HealthThreadState = true;
                            ParameterizedThreadStart pts = new ParameterizedThreadStart(Handlers.Handler14.PartyHealthThread);
                            Thread HealthThread = new Thread(pts);
                            HealthThread.Start(Addclient);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Member Fehler");
                }
            }

        }
        [InterPacketHandler(InterHeader.RemovePartyMember)]
        public static void RemovePartyMember(WorldConnector lc, InterPacket packet)
        {
            string charname;
            string Removename;
            if (packet.TryReadString(out charname, 16))
            {
                if (packet.TryReadString(out Removename, 16))
                {
                    ZoneClient Memberclient = ClientManager.Instance.GetClientByName(charname);
                    ZoneClient mm = ClientManager.Instance.GetClientByName(Removename);
                    if (charname != Removename)
                    {
                        if (Memberclient.Character.Party.ContainsKey(Removename))
                        {
                            if (Memberclient.Character.Party.Count > 2)
                            {

                                Memberclient.Character.Party.Remove(Removename);
                                mm.Character.IsInParty = false;
                                mm.Character.HealthThreadState = false;
                            }
                        }
                    }
                    else
                    {
                   
                        Memberclient.Character.Party.Clear();
                        Memberclient.Character.IsInParty = false;
                        Memberclient.Character.HealthThreadState = false;

                    }
                }
            }
        }
        [InterPacketHandler(InterHeader.ZONELIST)]
        public static void HandleZoneList(WorldConnector lc, InterPacket packet)
        {
            int amount;
            if (!packet.TryReadInt(out amount))
            {
                return;
            }

            for (int i = 0; i < amount; i++)
            {
                byte id;
                string ip;
                ushort port;
                int mapcount;
                if (!packet.TryReadByte(out id) || !packet.TryReadString(out ip) || !packet.TryReadUShort(out port) || !packet.TryReadInt(out mapcount))
                {
                    return;
                }
                var maps = new List<MapInfo>();
                for (int j = 0; j < mapcount; j++)
                {
                    ushort mapid, viewrange;
                    string shortname, fullname;
                    int regenx, regeny;
                    byte kingdom;
                    if (!packet.TryReadUShort(out mapid) || !packet.TryReadString(out shortname) || !packet.TryReadString(out fullname) || !packet.TryReadInt(out regenx) || !packet.TryReadInt(out regeny) || !packet.TryReadByte(out kingdom) || !packet.TryReadUShort(out viewrange))
                    {
                        break;
                    }
                    maps.Add(new MapInfo(mapid, shortname, fullname, regenx, regeny, kingdom, viewrange)); ;
                }

                ZoneData zd;
                if (!Program.Zones.TryGetValue(id, out zd))
                {
                    zd = new ZoneData();
                }
                zd.ID = id;
                zd.IP = ip;
                zd.Port = port;
                zd.MapsToLoad = maps;
                Program.Zones[id] = zd;
                Log.WriteLine(LogLevel.Info, "Added zone {0} to zonelist. {1}:{2}", zd.ID, zd.IP, zd.Port);
            }
        }


        [InterPacketHandler(InterHeader.CLIENTTRANSFER)]
        public static void HandleTransfer(WorldConnector lc, InterPacket packet)
        {
            byte v;
            if (!packet.TryReadByte(out v))
            {
                return;
            }

            if (v == 0)
            {
                byte admin;
                int accountid;
                string username, hash, hostip;
                if (!packet.TryReadInt(out accountid) || !packet.TryReadString(out username) || !packet.TryReadString(out hash) || !packet.TryReadByte(out admin) || !packet.TryReadString(out hostip))
                {
                    return;
                }
                ClientTransfer ct = new ClientTransfer(accountid, username, admin, hostip, hash);
                ClientManager.Instance.AddTransfer(ct);
            }
            else if (v == 1)
            {
                byte admin;
                int accountid;
                string username, charname, hostip;
                ushort randid;
                if (!packet.TryReadInt(out accountid) || !packet.TryReadString(out username) || !packet.TryReadString(out charname) ||
                    !packet.TryReadUShort(out randid) || !packet.TryReadByte(out admin) || !packet.TryReadString(out hostip))
                {
                    return;
                }
                ClientTransfer ct = new ClientTransfer(accountid, username, charname, randid, admin, hostip);
                ClientManager.Instance.AddTransfer(ct);
            }
        }

        public static void TryAssiging(WorldConnector lc)
        {
            using (var p = new InterPacket(InterHeader.ASSIGN))
            {
                p.WriteStringLen(Settings.Instance.IP);
                lc.SendPacket(p);
            }
        }

        public static void TransferClient(byte zoneID,ushort mapid, int accountID, string userName, string charName, ushort randid, byte admin, string hostIP)
        {
            using (var packet = new InterPacket(InterHeader.CLIENTTRANSFERZONE))
            {
                packet.WriteByte(zoneID);
                packet.WriteInt(accountID);
                packet.WriteInt(mapid);
                packet.WriteStringLen(userName);
                packet.WriteStringLen(charName);
                packet.WriteUShort(randid);
                packet.WriteByte(admin);
                packet.WriteStringLen(hostIP);
                WorldConnector.Instance.SendPacket(packet);
            }
        }

        public static void SendWorldMessage(WorldMessageTypes type, string message, string to = "")
        {
            using (var packet = new InterPacket(InterHeader.WORLDMSG))
            {
                packet.WriteStringLen(message);
                packet.WriteByte((byte)type);
                packet.WriteBool(to != "");
                if (to != "")
                {
                    packet.WriteStringLen(to);
                }
                WorldConnector.Instance.SendPacket(packet);
            }
        }
    }
}
