using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Zone.Networking;
using System;

namespace Zepheus.Zone.Handlers
{
    public sealed class Handler38
    {
        [PacketHandler(CH38Type.GuildExtraRequest)]
        public static void GuildExtraRequest(ZoneClient client, Packet packet)
        {
            if (client.Character.Guild == null && client.Character.GuildAcademy == null)
                return;
            byte response;
            if (!packet.TryReadByte(out response))
                return;
            switch (response)
            {
                case 0:
                    Console.WriteLine("Gilden Lager");
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
            if (!client.Character.IsInaAcademy)
            {
                using (var pack = new Packet(SH38Type.GuildExtraResponse))
                {
                    pack.WriteUShort(6104);//Responsecode
                    pack.WriteLong(client.Character.Guild.GuildMoney);//guildmoney
                    pack.WriteByte(0);//unk
                    client.SendPacket(pack);
                }
            }
            else
            {
                using (var pack = new Packet(SH38Type.GuildExtraResponse))
                {
                    pack.WriteUShort(6104);//Responsecode
                    pack.WriteLong(client.Character.GuildAcademy.Guild.GuildMoney);//guildmoney
                    pack.WriteByte(0);//unk
                    client.SendPacket(pack);
                }
            }
        }
    }
}
