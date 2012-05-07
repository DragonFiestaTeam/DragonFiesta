
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.World.Data;
using Zepheus.World.Networking;

namespace Zepheus.World.Handlers
{
    public sealed class Handler29
    {
        [PacketHandler(CH29Type.GuildNameRequest)]
        public static void GuildNameRequest(WorldClient client, Packet packet)
        {
            int id;
            if (!packet.TryReadInt(out id)) {
                Log.WriteLine(LogLevel.Warn, "Failed reading Guild Name Request packet {0}", client.Character.Character.Name);
                return;
            }
            var guild = WorldGuild.GetGuild(id);
            if (guild != null)
            {
                SendGuildNameResult(client, id, guild.Name);
            }
        }

        public static void SendGuildNameResult(WorldClient client, int pID, string pName)
        {
            using (var packet = new Packet(SH29Type.GuildNameResult))
            {
                packet.WriteInt(pID);
                packet.WriteString(pName, 16);
                client.SendPacket(packet);
            }
        }
        [PacketHandler(CH29Type.GuildListReqest)]
        public static void GuildListReqest(WorldClient client, Packet packet)
        {
            using (var Ppacket = new Packet(SH29Type.GuildList))
            {
                
                Ppacket.WriteInt(20);
                Ppacket.WriteUShort(20);
                for (int i = 0; i < 20; i++)
                {
                Ppacket.WriteString("charname", 16);
                Ppacket.WriteByte(6);//rank
                Ppacket.WriteInt(0);

                Ppacket.WriteUShort(9000);//korp
                Ppacket.WriteByte(0);//unk
                Ppacket.WriteUShort(0xffff);//unk
                Ppacket.WriteUShort(0xffff);//unk
                Ppacket.WriteByte(0);//unk
                Ppacket.WriteInt(32);//isonline=
                Ppacket.WriteInt(32);
                Ppacket.Fill(50, 0x00);//unk
                bool isonline = true;
                Ppacket.WriteByte(isonline ? (byte)0xB9 : (byte)0x00);//onlinestatus
                Ppacket.Fill(3, 0x00);//unk
                Ppacket.WriteByte(3);//job
                Ppacket.WriteByte(255);//unk
                Ppacket.WriteByte(0);//unk
                Ppacket.WriteString("RouCos01",12);//charmapname
                }
                client.SendPacket(Ppacket);
            }
         
        }
        [PacketHandler(CH29Type.CreateGuild)]
        public static void CreateGuildt(WorldClient client, Packet packet)
        {
            int unk = 0;
            bool GuildWar;
            string GuildName = string.Empty;
            string GuildPassword = string.Empty;
            if (!packet.TryReadString(out GuildName, 16))
                return;
            if (!packet.TryReadString(out GuildPassword, 8))
                return;
            if (!packet.TryReadInt(out unk))
                return;
            if (!packet.TryReadBool(out GuildWar))
                return;
            {
                using (var ppacket = new Packet(29, 6))
                {
                    if(client.Character.Character.CharLevel < 20)
                    {
                        ppacket.WriteUShort(3266);
                        ppacket.WriteUInt(0);//unk
                    }
                    else if (client.Character.Character.Money < 1000000)
                    {
                        ppacket.WriteUShort(3228);
                        ppacket.WriteUInt(0);//unk
                    }
                    else
                    {
                        ppacket.WriteUShort(3137);
                        ppacket.WriteUInt(32);//unk
                        //:TODO create Guild shit
                    }
                    ppacket.WriteString(GuildName, 16);
                    ppacket.WriteString(GuildPassword, 8);
                    ppacket.WriteBool(GuildWar);
                    client.SendPacket(ppacket);
                }
            }
        }
    }
}
