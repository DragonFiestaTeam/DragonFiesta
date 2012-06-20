using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.Util;
using Zepheus.Zone.Game;
using Zepheus.Zone.Networking;

namespace Zepheus.Zone.Handlers
{
    public sealed class Handler19
    {
        [PacketHandler(CH19Type.CommercialReqest)]
        public static void CommercialReqest(ZoneClient pClient, Packet pPacket)
        {
            ushort unk;
            if (!pPacket.TryReadUShort(out unk))
                return;
            System.Console.WriteLine(unk);
            System.Console.WriteLine(pClient.Character.SelectedObject);
            SendTestReqest(pClient);
        }
        public static void SendTestReqest(ZoneClient pClient)
        {
            if(pClient.Character.SelectedObject != null && pClient.Character.SelectedObject is ZoneCharacter)
            {
                ZoneCharacter Target = pClient.Character.SelectedObject as ZoneCharacter;
            using(var pPacket = new Packet(SH19Type.SendCommercialReqest))
            {
                pPacket.WriteUShort(45);
                Target.Client.SendPacket(pPacket);
            }
            }
        }
    }
}
