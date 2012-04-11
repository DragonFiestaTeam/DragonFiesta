using Zepheus.Zone.Game;

namespace Zepheus.Zone.Data
{
    public class GroupMember
    {
        #region .ctor
        #endregion
        #region Properties
        public bool IsMaster { get; internal set; }
        public bool IsOnline { get; set; }
        public Group Group { get; internal set; }
        public string Name { get; internal set; }
        public ZoneCharacter Character { get; internal set; }
        #endregion
        #region Methods
        #endregion
    }
}
