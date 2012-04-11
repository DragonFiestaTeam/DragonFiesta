using System.Collections.Generic;
using Zepheus.Zone.Data;

namespace Zepheus.Zone
{
    [Util.ServerModule(Util.InitializationStage.Clients)]
    public class GroupManager
    {
        #region .ctor
        private GroupManager()
        {
            this.groups = new List<Group>();
            this.groupsById = new Dictionary<long, Group>();
            this.groupsByMaster = new Dictionary<string, Group>();
        }
        #endregion
        #region Properties
        public static GroupManager Instance { get; private set; }

        private List<Group> groups;
        private Dictionary<string, Group> groupsByMaster;
        private Dictionary<long, Group> groupsById;
        #endregion
        #region Methods
        #endregion
    }
}
