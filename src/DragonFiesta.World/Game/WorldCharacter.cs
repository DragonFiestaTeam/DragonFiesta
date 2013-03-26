using System;
using DragonFiesta.Database;
using DragonFiesta.Networking;
using DragonFiesta.World.Networking;
using DragonFiesta.Networking.Handler.Server;
using DragonFiesta.Data;

namespace DragonFiesta.World.Game
{
    public class WorldCharacter : Character // CharacterHelper : Character
    {
        public new WorldClient pClient { get; set; }
        public WorldCharacter() { }
        public WorldCharacter(SQLResult result, int row)
        {

            base.ReadFromDatabase(result, row);//Read base Variables
        }

        #region Blob
        public void SetQuickBarData(byte[] pData)
        {
            this.QuickBar = pData;

        }
        public void SetQuickBarStateData(byte[] pData)
        {
            this.QuickBarState = pData;
        }
        public void SetGameSettingsData(byte[] pData)
        {
            this.GameSettings = pData;
        }
        public void SetClientSettingsData(byte[] pData)
        {
            this.ClientSettings = pData;
        }
        public void SetShortcutsData(byte[] pData)
        {
            this.Shortcuts = pData;
        }
        #endregion//todo save
    }
}
