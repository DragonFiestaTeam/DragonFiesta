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
            return true;
        }
        #endregion
        #region Properties
        public static MasterManager Instance { get; private set; }
        private readonly List<MasterRequest> pMasterRequests;
        #endregion
        #region Methods
        public void AddMasterRequest(WorldClient pClient)
        {

        }
        #endregion
        #region Packets
        public void SendMasterList(WorldClient pClient)
        {
            using(var packet = new Packet(99,99))
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