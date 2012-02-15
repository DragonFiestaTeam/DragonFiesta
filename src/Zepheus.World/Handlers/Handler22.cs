using System;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Services.DataContracts;
using Zepheus.Util;
using Zepheus.World.Data;
using Zepheus.World.Networking;

namespace Zepheus.World.Handlers
{
   public class Handler22
    {
       [PacketHandler(CH22Type.GotIngame)]
       public static void GotIngame(WorldClient client, Packet Packet)
       {
           using (var packet = new Packet(0x1097))
           {      // Guild academy info
               packet.Fill(5, 0);
               client.SendPacket(packet);
           }
           using (var packet = new Packet(SH4Type.CharacterGuildinfo))
           {      // Guild info
               packet.WriteInt(client.Character.Character.GuildID);
               client.SendPacket(packet);
           }
           //SendMasterList(pClient);
           using (var packet = new Packet())
           {
               packet.WriteShort(9414);
               //packet.WriteString("", 16); //master name
               packet.Fill(22, 0);
               packet.WriteByte(3);
               packet.WriteShort(0);
               client.SendPacket(packet);

           }
           using (var packet = new Packet())
           {
               packet.WriteShort(0x581C);
               packet.WriteUInt(0x4d0bc167);   // 21h
               client.SendPacket(packet);
           }
           using (var packet = new Packet())
           {
               packet.WriteShort(0x581D);
               packet.WriteShort(0);           //zero kingdom quests!
               client.SendPacket(packet);
           }
           
           using (var packet = new Packet(21, 7))
           {
               packet.WriteByte((byte)0);
               client.Character.WriteFriendData(packet);
               client.SendPacket(packet);
           }
           using (var packet = new Packet(SH2Type.UnkTimePacket))
           {
               packet.WriteShort(256);
               client.SendPacket(packet);
           }
           Handler2.SendClientTime(client,DateTime.Now);
           client.Character.Loadfriends();
           client.Character.FriendOnline();

       }
    }
}
