using System.Collections.Generic;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.InterLib.Networking;
using Zepheus.Util;
using Zepheus.World.Data;
using Zepheus.World.Networking;
using System;

namespace Zepheus.World
{
    [ServerModule(InitializationStage.Clients)]
    public class MasterManager
    {
        #region .ctor

        public MasterManager()
        {
            pMasterRequests = new List<MasterRequest>();
        }
        [InitializerMethod]
        public static bool Initialize()
        {
            Instance = new MasterManager();
            return true;
        }
        #endregion
        #region Properties
        public static MasterManager Instance { get; private set; }
        private readonly List<MasterRequest> pMasterRequests;
        #endregion
        #region Methods
        public void AddMasterRequest(WorldClient pClient,string target)
        {
            MasterRequest Request = new MasterRequest(target,pClient);
            SendMasterRequest(Request);
            pMasterRequests.Add(Request);
        }
        public void RemoveMasterRequest(WorldClient pClient)
        {
            MasterRequest Request = pMasterRequests.Find(d => d.InvitedClient == pClient);
            SendMasterRequestDecline(Request.InviterClient);
            pMasterRequests.Remove(Request);

        }
        public void RemoveMasterMember(WorldCharacter pChar,string name)
        {
            MasterMember pMember = pChar.MasterList.Find(d => d.pMemberName == name);
            pChar.MasterList.Remove(pMember);
            pChar.UpdateMasterJoin();
        }
        public void ApprenticeLevelUP(WorldCharacter pChar)
        {

        }
        public void MasterRequestAccept(string requestername, string TargetName)
        {
            WorldClient target = ClientManager.Instance.GetClientByCharname(TargetName);
            WorldClient requester = ClientManager.Instance.GetClientByCharname(requestername);
            MasterMember ReqMember = new MasterMember(requester);
            MasterMember TargetM = new MasterMember(target);
            ReqMember.AddToDatabase(target.Character.ID);
            TargetM.AddToDatabase(requester.Character.ID);
            target.Character.MasterList.Add(ReqMember);
            requester.Character.MasterList.Add(TargetM);
            SendMasterRequestAccept(requester,TargetName);
        }
        #endregion
        #region Packets
         private void SendMasterRequestDecline(WorldClient pClient)
        {
          //Todo
        }
         private void SendMasterRequestAccept(WorldClient pClient,string TargetName)
        {
            using(var packet = new Packet(SH37Type.SendMasterRequestAccept))
            {
                packet.WriteString(TargetName, 16);
                pClient.SendPacket(packet);
            }
        }
         private void SendMasterRequest(MasterRequest pRequest)
        {
            using (var packet = new Packet(SH37Type.SendMasterRequest))
            {
                packet.WriteString(pRequest.InviterClient.Character.Character.Name, 16);
                packet.WriteString(pRequest.InvitedClient.Character.Character.Name, 16);
                pRequest.InvitedClient.SendPacket(packet);
            }
        }
        public void SendMasterList(WorldClient pClient)
        {
            using(var packet = new Packet(SH37Type.SendMasterList))
            {
                int nowyear = (DateTime.Now.Year - 1920 << 1) | 1;
                int nowmonth = (DateTime.Now.Month << 4) | 0x0F;
                packet.WriteString(pClient.Character.Character.Name);
                packet.WriteByte((byte)nowyear);
                packet.WriteByte((byte)nowmonth);
                packet.WriteByte((byte)DateTime.Now.Day);
                packet.WriteByte(0);//unk
                packet.WriteByte(pClient.Character.Character.CharLevel);
                packet.WriteByte(0);//unk
                packet.WriteByte(0x03);//unk
                packet.WriteUShort((ushort)pClient.Character.MasterList.Count);
                foreach(var Member in pClient.Character.MasterList)
                {
                    packet.WriteString(Member.pMemberName);
                    int year = (Member.RegisterDate.Year - 1920 << 1) | Convert.ToUInt16(Member.IsOnline);
                    int month = (Member.RegisterDate.Month << 4) | 0x0F;
                    packet.WriteByte((byte)year);
                    packet.WriteByte((byte)month);
                    packet.WriteByte((byte)Member.RegisterDate.Day);
                    packet.WriteByte(0);//unk
                    packet.WriteByte(Member.Level);
                    packet.WriteByte(0);//unk

                }
            }
        }
        #endregion 
    }

}