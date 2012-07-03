using System;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.World.Networking;
using Zepheus.World.Data;
using Zepheus.World.Managers;

namespace Zepheus.World.Handlers
{
    public sealed class Handler38
    {
        [PacketHandler(CH38Type.GuildAcademyReuqest)]
        public static void GuildAcademyRequest(WorldClient client, Packet packet)
        {
            string AcademyName;
            if (!packet.TryReadString(out AcademyName, 16))
                return;
            if (client.Character.Character.CharLevel > 60)
                return;

             Academy Academy =  AcademyManager.Instance.GetAcademyByName(AcademyName);

             if (Academy == null)
             {
                 AcademyManager.Instance.SendAcademyRequest(client, AcademyRequestCode.NotFound, AcademyName);
                 return;
             }
             else if (Academy.AcademyMembers.Count >= 50)
             {
                 AcademyManager.Instance.SendAcademyRequest(client, AcademyRequestCode.AcademyFull, AcademyName);
             }
             else if (Academy.GetMemberByName(client.Character.Character.Name) != null)
             {
                 AcademyManager.Instance.SendAcademyRequest(client, AcademyRequestCode.AlreadyExists, AcademyName);
             }
             try
             {
                 AcademyMember pMember = new AcademyMember
                 {
                     CharID = client.Character.ID,
                     isOnline = true,
                     Level = client.Character.Character.CharLevel,
                     Rank = 0,//rank unkown for member
                     pClient = client,
                     MapName = DataProvider.Instance.GetMapShortNameFromMapid(client.Character.Character.PositionInfo.Map),
                     pMemberName = client.Character.Character.Name,
                     pMemberJob = client.Character.Character.Job,

                 };
                 pMember.AddToDatabase();
                 Academy.AcademyMembers.Add(pMember);
             }
             catch
             {
                 AcademyManager.Instance.SendAcademyRequest(client, AcademyRequestCode.dbEror, AcademyName);
             }
             AcademyManager.Instance.SendAcademyRequest(client, AcademyRequestCode.Sucess, AcademyName);
        }
        [PacketHandler(CH38Type.GuildAcademyRequestList)]
        public static void GuildAcademyRequestList(WorldClient client, Packet packet)
        {
            var pp = new Packet(38, 14);
            pp.WriteHexAsBytes("03 00 00 00 03 00 4A 6F 6B 65 72 6D 61 6E 00 00 00 00 00 00 00 00 00 00 58 00 00 00 00 00 01 00 00 00 00 00 00 00 FF FF FF 7F 00 20 00 00 00 20 00 00 00 10 00 00 01 00 00 00 E9 03 00 00 66 00 02 00 08 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 B8 6F D7 02 15 03 00 52 6F 75 00 00 00 00 00 00 00 00 00 B8 5F 03 6E 03 44 61 72 6B 65 73 74 44 72 65 61 6D 00 00 00 00 00 00 58 00 00 00 00 00 01 00 00 00 00 00 00 00 FF FF FF 7F 00 20 00 00 00 20 00 00 00 10 00 00 01 00 00 00 E9 03 00 00 66 00 02 00 08 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 B8 2F BD 00 02 2E 00 52 6F 75 56 61 6C 30 31 00 00 00 00 B8 2F 14 6E 2E 4C 75 78 5A 69 66 65 72 00 00 00 00 00 00 00 00 00 00 58 00 00 00 00 00 01 00 00 00 00 00 00 00 FF FF FF 7F 00 20 00 00 00 20 00 00 00 10 00 00 01 00 00 00 E9 03 00 00 66 00 02 00 08 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 B9 6F 9E 02 15 08 00 52 6F 75 00 00 00 00 00 00 00 00 00 B8 4F 77 6E 07");
            client.SendPacket(pp);
        }
           [PacketHandler(CH38Type.GetGuildAcademyDetails)]
        public static void GetGuildAcademyDetails(WorldClient client, Packet packet)
        {
        /*    var pp = new Packet(38, 14);
            pp.WriteInt(3);//count
            pp.WriteUShort(3);//Academy Member count
            for (int i = 1; i < 3; i++)
            {
                pp.WriteString("charname"+i, 16);
                pp.WriteShort(10);//unk
                pp.WriteInt(100);
                pp.WriteUShort(0);
                pp.WriteLong(1);
                pp.WriteUShort(0xffff);//unk
                pp.WriteUShort(0xFF7F);
                pp.WriteByte(0);//0 unk
                pp.WriteInt(32);//unk
                pp.WriteInt(32);//unk
                pp.WriteByte(16);//Job?
                // packet.WriteUShort(0xff7F);//unk
                pp.WriteUShort(0);//unk
                pp.WriteInt(1);//unk
                pp.WriteUShort(1001);//kp
                pp.WriteUShort(0);//kp
                pp.WriteUShort(102);//kp
                pp.Fill(22, 0);
                pp.WriteUShort(2600);//pos x
                pp.WriteUShort(700);//pos y
                pp.WriteByte(0);//unk
                pp.WriteByte(16);//job
                pp.WriteByte(255);
                pp.WriteByte(1);//unk
                pp.WriteString("Rou", 12);//mapname
                pp.WriteHexAsBytes("B9 5F");
                pp.WriteByte(12);//day
                pp.WriteByte(112);
                pp.WriteByte(0);//unk
            }*/
            //client.SendPacket(pp);
           var pack = new Packet(38,8);
            pack.WriteUShort(6584);//GuildAcadmyID
            pack.WriteString("Acadmmy", 16);//Master
            pack.WriteUShort(22);//membercounts
            pack.WriteUShort(50);//max member count
            pack.WriteInt(5300);//Academy :
         //   pack.WriteUShort(0xffff);
            pack.WriteInt(10);//weeks
            pack.WriteInt(100);
            pack.Fill(128, 0x00);//GuildBuff?
            pack.WriteString("Wilkommen", 512);
            client.SendPacket(pack);
          /*  using (var pack2 = new Packet(4, 18))
            {
                pack2.WriteInt(9507);//GuildID?
                pack2.WriteInt(9507);//AcademyID?
                pack2.WriteString("1234567891234567", 16);
                //this shit later
                pack2.Fill(24, 0x00);//unk
                pack2.WriteUShort(38);
                pack2.WriteInt(100);
                pack2.Fill(233, 0x00);//unk
                pack2.WriteUShort(11779);
                pack2.WriteUShort(20082);
                pack2.WriteInt(31);
                pack2.WriteInt(55);
                pack2.WriteInt(18);//unk
                pack2.WriteInt(15); 
                pack2.WriteInt(8);//unk
                pack2.WriteInt(111);//unk
                pack2.WriteInt(4);
                pack2.Fill(136,0);//buff or string
                pack2.WriteUShort(1824);
                pack2.WriteUShort(20152);
                pack2.WriteInt(16);
                pack2.WriteInt(28);
                pack2.WriteInt(12);//createDetails Guild Minutes Date
                pack2.WriteInt(9); //create Details Guild Hours Date
                pack2.WriteInt(8);//create details Guild Day Date
                pack2.WriteInt(11);//create details Month
                pack2.WriteInt(112);//creae details year 1900- 2012
                pack2.WriteInt(1);//unk
                pack2.WriteUShort(2);
                pack2.Fill(6, 0);//unk
                pack2.WriteString("GuildMaster",16);
                pack2.WriteString("testmessage", 512);//details message
                client.SendPacket(pack2);
            }/*/
        }
    }
}
