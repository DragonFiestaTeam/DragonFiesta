using System.Collections.Generic;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.InterLib.Networking;
using Zepheus.Util;
using Zepheus.World.Data;
using Zepheus.FiestaLib.Data;
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
              MasterRequestResponse response = new MasterRequestResponse(Request);
            if(response.responseAnswer)
            {
             response.SendMasterRequest();
             pMasterRequests.Add(Request);
            }
        }
        public void RemoveMasterRequest(WorldClient pClient)
        {
            MasterRequest Request = pMasterRequests.Find(d => d.InvitedClient == pClient);
            pMasterRequests.Remove(Request);

        }
        public void RemoveMasterMember(WorldClient pClient)
        {
            MasterMember pMember = pClient.Character.MasterList.Find(d => d.IsMaster == true);
            if(pMember.pMember != null)
            {
                SendApprenticeRemoveMaster(pMember.pMember, pClient.Character.Character.Name);
            }
            pMember.RemoveFromDatabase(pMember.pMemberName, pClient.Character.ID);
            pMember.RemoveFromDatabase(pMember.pMemberName);
            pClient.Character.MasterList.Remove(pMember);
            pClient.Character.UpdateMasterJoin();
            SendMasterRemoveResponse(pClient);
             
        }
        public void ApprenticeLevelUP(WorldCharacter pChar)
        {
            MasterRewardItem reward = DataProvider.Instance.MasterRewards.Find(d => (byte)d.Job == pChar.Character.Job && d.Level == pChar.Character.CharLevel);
  
        }
        public void RemoveMasterMember(WorldCharacter pChar,string name)
        {
            MasterMember pMember = pChar.MasterList.Find(d => d.pMemberName == name);
            WorldClient pClient = ClientManager.Instance.GetClientByCharname(name);
            if (pClient != null)
            {
                //Todo Send break

                pClient.Character.MasterList.Remove(pMember);
            }
            pMember.RemoveFromDatabase(name,pChar.ID);
            pMember.RemoveFromDatabase(name);
            pChar.MasterList.Remove(pMember);
            pChar.UpdateMasterJoin();
        
        }

        public void MasterRequestAccept(string requestername, string TargetName)
        {
            WorldClient target = ClientManager.Instance.GetClientByCharname(TargetName);
            WorldClient requester = ClientManager.Instance.GetClientByCharname(requestername);
            MasterRequestResponse Reponse = new MasterRequestResponse(target, requester);
            if (Reponse.responseAnswer)
            {
                MasterMember ReqMember = new MasterMember(requester);
                MasterMember TargetM = new MasterMember(target);
                ReqMember.AddToDatabase(target.Character.ID);
                TargetM.IsMaster = true;
                TargetM.AddToDatabase(requester.Character.ID);
                target.Character.MasterList.Add(ReqMember);
                requester.Character.MasterList.Add(TargetM);
                SendMasterRequestAccept(requester, TargetName);
            }
            else
            {
                MasterRequest rRequest = pMasterRequests.Find(d => d.InvitedClient == requester);
                this.pMasterRequests.Remove(rRequest);
            }
        }
 
        #endregion
        #region private Methods

        private void SendMasterRemove(WorldClient pClient)
        {
          using(var packet = new Packet(SH37Type.SendRemoveMember))
           {
                /*packet.WriteByte(
               pClient.SendPacket(packet);*/
           }
        }
         private void SendMasterRemoveResponse(WorldClient pClient)
        {
            using (var packet = new Packet(SH37Type.SendMasterResponseRemove))
            {
                packet.WriteByte(0);
                packet.WriteUShort(0x1740);
                pClient.SendPacket(packet);
            }
        }
        
        private void SendApprenticeRemoveMaster(WorldClient pClient,string name)
         {
           using(var packet = new Packet(SH37Type.SendApprenticeRemoveMaster))
           {
               packet.WriteString(name, 16);
               packet.WriteByte(0);//isonline?
           }

         }
         private void SendApprenticeLevelUp(WorldClient pClient,string pName,byte level)
         {
             using (var packet = new Packet(SH37Type.SendApprenticeLevelUp))
             {
                 packet.WriteString(pName, 16);
                 packet.WriteByte(level);
                 pClient.SendPacket(packet);
             }
         }
         private void SendMasterRequestAccept(WorldClient pClient,string TargetName)
        {
            using(var packet = new Packet(SH37Type.SendMasterRequestAccept))
            {
                packet.WriteString(TargetName, 16);
                pClient.SendPacket(packet);
            }
        }

        public void SendMasterList(WorldClient pClient)
        {
            if(pClient.Character.MasterList.Count== 0)
                return;

            using(var packet = new Packet(SH37Type.SendMasterList))
            {
                MasterMember Master = pClient.Character.MasterList.Find(d => d.IsMaster == true);
                if (Master != null)
                {
                    int nowyear = (Master.RegisterDate.Year - 1920 << 1) | Convert.ToByte(Master.IsOnline);
                    int nowmonth = (Master.RegisterDate.Month << 4) | 0x0F;
                    packet.WriteString(Master.pMemberName, 16);
                    packet.WriteByte((byte)nowyear);
                    packet.WriteByte((byte)nowmonth);
                    packet.WriteByte((byte)DateTime.Now.Day);
                    packet.WriteByte(0x01);//unk
                    packet.WriteByte(Master.Level);
                    packet.WriteByte(0);//unk
                    packet.WriteByte(0x03);//unk
                    int count = pClient.Character.MasterList.Count - 1;
                    packet.WriteUShort((ushort)count);
                }
                else
                {
                    DateTime now = DateTime.Now;
                    int nowyear = (now.Year - 1920 << 1) | 1;
                    int nowmonth = (now.Month << 4) | 0x0F;
                    packet.WriteString("", 16);
                    packet.WriteByte((byte)nowyear);
                    packet.WriteByte((byte)nowmonth);
                    packet.WriteByte((byte)now.Day);
                    packet.WriteByte(0x01);//unk
                    packet.WriteByte(1);
                    packet.WriteByte(0);//unk
                    packet.WriteByte(0x03);//unk
                    packet.WriteUShort((ushort)pClient.Character.MasterList.Count);
                    //tODO when master null
                }
                foreach(var Member in pClient.Character.MasterList)
                {
                    if (Member.pMember != Master.pMember)
                    {
                        packet.WriteString(Member.pMemberName, 16);
                        int year = (Member.RegisterDate.Year - 1920 << 1) | Convert.ToUInt16(Member.IsOnline);
                        int month = (Member.RegisterDate.Month << 4) | 0x0F;
                        packet.WriteByte((byte)year);
                        packet.WriteByte((byte)month);
                        packet.WriteByte(0xB9);
                        packet.WriteByte(0x11);//unk
                        packet.WriteByte(Member.Level);
                        packet.WriteByte(0);//unk
                    }

                }
                pClient.SendPacket(packet);
            }
        }
        #endregion 
        
    }

}