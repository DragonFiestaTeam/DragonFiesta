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
                RegisterDate = DateTime.ParseExact(row["RegisterDate"].ToString(), "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
            };
                Member.pMember = ClientManager.Instance.GetClientByCharname(Member.pMemberName);
                Member.IsOnline = ClientManager.Instance.IsOnline(Member.pMemberName);
            return Member;
        }
        public void AddToDatabase(int CharID)
        {
            //myDate.ToString("yyyy-MM-dd hh:mm");

         Program.DatabaseManager.GetClient().ExecuteQuery("INSERT INTO Masters (CharID,MemberName,Level,RegisterDate) VALUES ('"+CharID+"','"+this.pMemberName+"','"+this.Level+"','"+this.RegisterDate.ToString("yyyy-MM-dd hh:mm")+"')");
        }

        public void RemoveFromDatabase(string pName)
        {
            Program.DatabaseManager.GetClient().ExecuteQuery("DELETE FROM Masters WHERE binary `MemberName` ='" + pName + "')");
        }
        public void SetMemberStatus(bool Status)
        {
            if(Status)
            {
                SetOnline();
            }
            else
            {
                SetOffline();
            }
        }
        #endregion
        #region Packets
        private void SetOffline()
        {
            this.IsOnline = false;
            foreach(var pMemberOffline in this.pMember.Character.MasterList)
            { 
                using (var packet = new Packet(SH37Type.SendMasterMemberOnline))
                {
                    packet.WriteString(this.pMemberName, 16);
                   pMemberOffline.pMember.SendPacket(packet);
                }

            }

        }
        private void SetOnline()
        {
            this.IsOnline = true;
            foreach (var pMemberOnline in this.pMember.Character.MasterList)
            {
                if (pMemberOnline.pMember != null)
                {
                    using (var packet = new Packet(SH37Type.SendMasterMemberOnline))
                    {
                        packet.WriteString(this.pMemberName, 16);
                        pMemberOnline.pMember.SendPacket(packet);
                    }
                }
            }
        }
        #endregion
    }
}
