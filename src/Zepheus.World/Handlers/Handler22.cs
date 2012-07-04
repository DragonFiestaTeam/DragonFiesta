﻿using System;
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
           if (!client.Character.IsIngame)
           {
               client.Character.IsIngame = true;
               client.Character.OneIngameLoginLoad();
               MasterManager.Instance.SendMasterList(client);
               //SendMasterList(pClient);
           }
           using (var p1 = new Packet(SH4Type.CharacterGuildacademyinfo))
           {
           if(client.Character.Academy != null)
           {
               client.Character.Academy.Details.WriteMessageAsGuildAcadmyler(p1,client.Character.Academy);
              
           }
           else
           {
               p1.Fill(5, 0);
           }
           client.SendPacket(p1);
           }
          /*using (var p2 = new Packet(SH4Type.CharacterGuildinfo))
           {      // Guild info
               p2.WriteInt(client.Character.Character.GuildID);
               client.SendPacket(p2);
           }*/
           // dafuq no op code..
           using (var p = new Packet(0x581C))
           {
             //p.WriteShort();
               p.WriteUInt(0x4d0bc167);   // 21h
               client.SendPacket(p);
           }
           // dafuq no op code..
           using (var p3 = new Packet(0x581D))
           {
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
           
           client.Character.OnGotIngame();
       }
    }
}
