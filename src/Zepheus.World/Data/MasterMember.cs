using System;
using System.Data;
using Zepheus.World.Networking;
using Zepheus.Database.DataStore;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace Zepheus.World.Data
{
    public class MasterMember
    {  
        #region .ctor
        #endregion
        #region Properties
        public WorldClient pMember { get; private set; }
        public string pMemberName { get; private set; }
        public DateTime RegisterDate { get; private set; }

        public bool IsOnline { get; private set; }
        public bool IsMaster { get; set; }

        public byte Level { get; private set; }
        public MasterMember()
        {
        }
        public MasterMember(WorldClient pClient)
        {
            this.IsOnline = true;
            this.Level = pClient.Character.Character.CharLevel;
            this.RegisterDate = DateTime.Now;
            this.pMemberName = pClient.Character.Character.Name;
            this.pMember = pClient;
        }
        #endregion
        #region Methods
        public static MasterMember LoadFromDatabase(DataRow row)
        {
            MasterMember Member = new MasterMember()
            {
                pMemberName = row["MemberName"].ToString(),
                Level = GetDataTypes.GetByte(row["Level"]),
                IsMaster = GetDataTypes.GetBool(row["isMaster"]),
                RegisterDate = DateTime.ParseExact(row["RegisterDate"].ToString(), "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
            };
                Member.pMember = ClientManager.Instance.GetClientByCharname(Member.pMemberName);
                Member.IsOnline = ClientManager.Instance.IsOnline(Member.pMemberName);
            return Member;
        }
        public void AddToDatabase(int CharID)
        {
            //myDate.ToString("yyyy-MM-dd hh:mm");

            Program.DatabaseManager.GetClient().ExecuteQuery("INSERT INTO Masters (CharID,MemberName,Level,RegisterDate,isMaster) VALUES ('" + CharID + "','" + this.pMemberName + "','" + this.Level + "','" + this.RegisterDate.ToString("yyyy-MM-dd hh:mm") + "','"+Convert.ToByte(this.IsMaster)+"')");
        }
        public void RemoveFromDatabase(string name,int CharID)
        {
            Program.DatabaseManager.GetClient().ExecuteQuery("DELETE FROM Masters WHERE `MemberName` ='" + name + "' AND CharID ='"+CharID+"'");
        }
        public void RemoveFromDatabase(string name)
        {
            Program.DatabaseManager.GetClient().ExecuteQuery("DELETE FROM Masters WHERE binary `MemberName` ='" + this.pMemberName + "' AND isMaster ='1'");
        }
        public void UpdateLevel()
        {
            Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE  Masters SET Level='"+this.Level+"'WHERE binary `MemberName` ='" + this.pMemberName + "'");
        }
        public void SetMemberStatus(bool Status,WorldClient pClient)
        {
            if(Status)
            {
                SetOnline(pClient);
            }
            else
            {
                SetOffline(pClient);
            }
        }
        #endregion
        #region Packets
        private void SetOffline(WorldClient pClient)
        {
            this.IsOnline = false;
            foreach (var pMemberOffline in pClient.Character.MasterList)
            { 
                using (var packet = new Packet(SH37Type.SendMasterMemberOnline))
                {
                    packet.WriteString(pClient.Character.Character.Name, 16);
                   pMemberOffline.pMember.SendPacket(packet);
                }

            }

        }
        private void SetOnline(WorldClient pClient)
        {
            this.IsOnline = true;
            foreach (var pMemberOnline in pClient.Character.MasterList)
            {
                if (pMemberOnline.pMember != null)
                {
                    using (var packet = new Packet(SH37Type.SendMasterMemberOnline))
                    {
                        packet.WriteString(pClient.Character.Character.Name, 16);
                        pMemberOnline.pMember.SendPacket(packet);
                    }
                }
            }
        }
        #endregion
    }
}
