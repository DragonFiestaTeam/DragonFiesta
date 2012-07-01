using System;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.World.Networking;

namespace Zepheus.World.Handlers
{
    public sealed class Handler38
    {
           [PacketHandler(CH38Type.GetGuildAcademyDetails)]
        public static void GetGuildAcademyDetails(WorldClient client, Packet packet)
        {
            var pack = new Packet(38,8);
            pack.WriteUShort(6584);//GuildAcadmyID
            pack.WriteString("Acadmmy", 16);//Master
            pack.WriteUShort(3);//membercounts
            pack.WriteUShort(50);//max member count
            pack.WriteInt(10);//Academy :
         //   pack.WriteUShort(0xffff);
            pack.WriteInt(10);//weeks
            pack.WriteInt(100);
            pack.Fill(640, 0x00);//GuildBuff?
            client.SendPacket(pack);
            using (var pack2 = new Packet(4, 18))
            {
                pack2.WriteInt(9507);//GuildID?
                pack2.WriteInt(9507);//AcademyID?
                pack2.WriteString("1234567891234567", 16);
                //this shit later
                pack2.Fill(24, 0x00);//unk
                pack2.WriteUShort(38);
                pack2.WriteInt(100);
                pack2.Fill(243, 0x00);//unk
                pack2.WriteUShort(11779);
                pack2.WriteUShort(20082);
                pack2.WriteInt(31);
                pack2.WriteInt(55);
                pack2.WriteInt(18);//unk
                pack2.WriteInt(15); 
                pack2.WriteInt(8);//unk
                pack2.WriteInt(111);//unk
                pack2.WriteInt(4);
                pack2.WriteHexAsBytes("01 01 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ");
                pack2.WriteUShort(1824);
                pack2.WriteUShort(20152);
                pack2.WriteInt(16);
                pack2.WriteInt(28);
                pack2.WriteInt(11);
                pack2.WriteInt(17); 
                pack2.WriteInt(7);
                pack2.WriteInt(10);
                pack2.WriteInt(111);
                pack2.WriteInt(1);//unk
                pack2.WriteLong(310);
                pack2.WriteString("GuildMaster",16);
                pack2.WriteHexAsBytes(" 53 6F 21 61 62 20 73 6F 66 6F 72 74 20 6B F6 6E 6E 74 20 69 68 72 20 65 69 6E 20 77 65 6E 69 67 20 67 65 6C 64 20 76 65 72 64 69 65 6E 65 6E 21 0D 0A 69 68 72 20 6D FC 73 73 74 20 64 61 66 FC 72 20 6C 76 6C 6E 2E 2E 2E 0D 0A 4D F6 63 68 74 65 20 64 61 6D 69 74 20 61 75 63 68 20 67 6C 65 69 63 68 20 6D 69 74 74 65 69 6C 65 6E 20 64 61 73 20 61 62 20 6C 76 6C 20 31 30 2C 20 32 20 73 69 6C 62 65 72 20 70 72 6F 20 6D 6F 6E 61 74 20 69 6E 20 64 69 65 20 67 69 6C 64 65 6E 6B 61 73 73 65 20 65 69 6E 67 65 7A 61 68 6C 74 20 77 65 72 64 65 6E 20 6D FC 73 73 74 65 2E 0D 0A 77 FC 72 64 65 20 6D 69 63 68 20 66 72 65 75 65 6E 20 77 65 6E 6E 20 64 61 73 20 46 75 6E 6B 74 69 6F 6E 69 65 72 65 6E 20 77 FC 72 64 65 2E 0D 0A 0D 0A 4D 66 67 20 4D 59 5F 4B 49 4E 47 00 20 74 75 6E 20 68 61 62 65 6E 20 73 69 6E 64 20 6C 65 69 64 65 72 20 6E 75 72 20 7A 75 6D 20 41 6B 61 64 65 6D 69 65 62 65 73 75 63 68 20 62 65 72 65 63 68 74 69 67 74 2E 00 65 72 20 61 6C 6C 65 6D 21 20 57 65 72 62 74 20 66 FC 72 20 64 69 65 20 41 6B 61 64 65 6D 69 65 21 20 44 61 6E 6B 65 2C 20 49 63 65 6D 61 6E 00 61 73 73 65 6E 20 6B 61 6E 6E 2E 20 20 00 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 6E 69 72 69 31 32 33 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                client.SendPacket(pack2);
            }
        }
    }
}
