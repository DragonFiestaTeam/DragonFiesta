using System;
using System.Collections.Generic;
using System.Net.Sockets;
using DragonFiesta.Networking;
using DragonFiesta.World.Game;
using DragonFiesta.Data;
using DragonFiesta.Util;

namespace DragonFiesta.World.Networking
{
    public class WorldClient : ClientBase
    {
        public WorldClient(Socket Socket) : base(Socket)
        {
            this.OnDisconnect += new EventHandler<EventArgs>(pOnDisconnect);
        }
        public bool Authed { get; set; }

        public DateTime LastPong { get; set; }
        public DateTime LastPing { get; set; }
        public Acount AccountInfo { get; set; }
        public ushort RandomID { get; set; }
        public WorldCharacter pCharacter { get; set; }
        public Dictionary<int, WorldCharacter> CharacterList = new Dictionary<int, WorldCharacter>();

        void pOnDisconnect(object sender, EventArgs e)
        {

            Logs.Main.Debug("disconnect " + base.IP+base.Port + "");
            WorldClientManager.Instance.RemoveClient(this);
            base.Dispose();

        }

    }
}