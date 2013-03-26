using System;
using System.Net.Sockets;
using DragonFiesta.Networking;

namespace DragonFiesta.World.Networking
{
    public class WorldClient : ClientBase
    {
        public WorldClient(Socket Socket) : base(Socket)
        { }

        public DateTime LastPong { get; set; }
        public DateTime LastPing { get; set; }
    }
}