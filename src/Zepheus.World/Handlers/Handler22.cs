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
           {
               // Guild academy info
               p1.WriteInt(6584);//GuildAcadermyid?
               p1.WriteByte(1);//unk
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
           using (var pack = new Packet(4, 18))
           {
               pack.WriteInt(9507);//GuildID?
               pack.WriteInt(9507);//AcademyID?
               pack.WriteString("gg", 16);
               //this shit later
               pack.WriteHexAsBytes("00 00 00 00 00 00 00 00 00 01 00 00 00 00 01 00 00 00 00 00 00 00 00 00 26 00 64 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 03 2E 72 4E 1F 00 00 00 37 00 00 00 12 00 00 00 0F 00 00 00 08 00 00 00 6F 00 00 00 04 00 00 00 01 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 20 07 B8 4E 10 00 00 00 1C 00 00 00 11 00 00 00 07 00 00 00 0A 00 00 00 6F 00 00 00 01 00 00 00 36 01 00 00 00 00 00 00 4D 59 5F 4B 49 4E 47 00 00 00 00 00 00 00 00 00 53 6F 20 61 62 20 73 6F 66 6F 72 74 20 6B F6 6E 6E 74 20 69 68 72 20 65 69 6E 20 77 65 6E 69 67 20 67 65 6C 64 20 76 65 72 64 69 65 6E 65 6E 21 0D 0A 69 68 72 20 6D FC 73 73 74 20 64 61 66 FC 72 20 6C 76 6C 6E 2E 2E 2E 0D 0A 4D F6 63 68 74 65 20 64 61 6D 69 74 20 61 75 63 68 20 67 6C 65 69 63 68 20 6D 69 74 74 65 69 6C 65 6E 20 64 61 73 20 61 62 20 6C 76 6C 20 31 30 2C 20 32 20 73 69 6C 62 65 72 20 70 72 6F 20 6D 6F 6E 61 74 20 69 6E 20 64 69 65 20 67 69 6C 64 65 6E 6B 61 73 73 65 20 65 69 6E 67 65 7A 61 68 6C 74 20 77 65 72 64 65 6E 20 6D FC 73 73 74 65 2E 0D 0A 77 FC 72 64 65 20 6D 69 63 68 20 66 72 65 75 65 6E 20 77 65 6E 6E 20 64 61 73 20 46 75 6E 6B 74 69 6F 6E 69 65 72 65 6E 20 77 FC 72 64 65 2E 0D 0A 0D 0A 4D 66 67 20 4D 59 5F 4B 49 4E 47 00 20 74 75 6E 20 68 61 62 65 6E 20 73 69 6E 64 20 6C 65 69 64 65 72 20 6E 75 72 20 7A 75 6D 20 41 6B 61 64 65 6D 69 65 62 65 73 75 63 68 20 62 65 72 65 63 68 74 69 67 74 2E 00 65 72 20 61 6C 6C 65 6D 21 20 57 65 72 62 74 20 66 FC 72 20 64 69 65 20 41 6B 61 64 65 6D 69 65 21 20 44 61 6E 6B 65 2C 20 49 63 65 6D 61 6E 00 61 73 73 65 6E 20 6B 61 6E 6E 2E 20 20 00 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 6E 69 72 69 31 32 33 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
               client.SendPacket(pack);
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
           
         if (!client.Character.IsIngame)
           {
               client.Character.IsIngame = true;
               client.Character.OneIngameLoginLoad();
               MasterManager.Instance.SendMasterList(client);
               //SendMasterList(pClient);
           }
           client.Character.OnGotIngame();
       }
    }
}
