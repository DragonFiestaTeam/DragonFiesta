using System;
using DragonFiesta.InterNetwork;

namespace DragonFiesta.Messages.World
{
    [Serializable]
    public class ClientFromLoginToWorld : IMessage
    {
        public Guid Id { get; set; }

        public int AccountID { get; set; }
        public string UserName { get; set; }
        public byte Access_level { get; set; }
        public string AuthHash { get; set; }
        public string IP { get; set; }
    }
}
