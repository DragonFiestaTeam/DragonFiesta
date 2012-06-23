using System;
using System.Data;
using Zepheus.World.Networking;
using Zepheus.Database.DataStore;
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

        #endregion
        #region Methods
        public MasterMember LoadFromDatabase(DataRow row)
        {
            MasterMember Member = new MasterMember()
            {
                pMemberName = row["MemberName"].ToString(),
                Level = GetDataTypes.GetByte(row["Level"]),
                RegisterDate = DateTime.Parse(row["RegisterDate"].ToString()),
                IsOnline = ClientManager.Instance.IsOnline(pMemberName),
                pMember = ClientManager.Instance.GetClientByCharname(pMemberName),
            };
            return Member;
        }
        public void SetMemberStatus(bool Status)
        {
            if(Status)
            {
                //Todo: Online
            }
            else
            {
                //Todo: Offline
            }
        }
        #endregion
    }
}
