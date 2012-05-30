using System;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.World.Networking;

namespace Zepheus.World.Handlers
{
   public class Handler22
    {
       [PacketHandler(CH22Type.GotIngame)]
       public static void GotIngame(WorldClient client, Packet packet)
       {
           
           using (var p1 = new Packet(0x1097))
           {      // Guild academy info
               p1.Fill(5, 0);
               client.SendPacket(p1);
           }
           using (var p2 = new Packet(SH4Type.CharacterGuildinfo))
           {      // Guild info
               p2.WriteInt(client.Character.Character.GuildID);
               client.SendPacket(p2);
           }
           //SendMasterList(pClient);
           using (var pp = new Packet())
           {
               pp.WriteShort(9414);
               //packet.WriteString("", 16); //master name
               pp.Fill(22, 0);
               pp.WriteByte(3);
               pp.WriteShort(0);
               client.SendPacket(pp);

           }
           // dafuq no op code..
           using (var p = new Packet())
           {
               p.WriteShort(0x581C);
               p.WriteUInt(0x4d0bc167);   // 21h
               client.SendPacket(p);
           }
           // dafuq no op code..
           using (var p3 = new Packet())
           {
               p3.WriteShort(0x581D);
               p3.WriteShort(0);           //zero kingdom quests!
               client.SendPacket(p3);
           }
           
           using (var p4 = new Packet(21, 7))
           {
               p4.WriteByte((byte)client.Character.Friends.Count);
               client.Character.WriteFriendData(p4);
               client.SendPacket(p4);
           }
           using (var p5 = new Packet(SH2Type.UnkTimePacket))
           {
               p5.WriteShort(256);
               client.SendPacket(p5);
           }
           
         if (!client.Character.IsIngame)
           {
               client.Character.IsIngame = true;
               client.Character.OneIngameLoginLoad();
           }
          // client.Character.UpdateFriendStates(client);
           client.Character.OnGotIngame();
       }
    }
}
