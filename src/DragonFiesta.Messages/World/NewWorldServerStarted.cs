using System;
using DragonFiesta.InterNetwork;

namespace DragonFiesta.Messages.World
{
    [Serializable]
	public class NewWorldServerStarted : IMessage, IExpectAnAnswer
	{
		public Guid Id { get; set; }
		public string Ip { get; set; }
		public ushort Port { get; set; }
        [NonSerialized] private Action<IMessage> callback;
        public Action<IMessage> Callback { 
            get { return callback; }
            set { callback = value; }
        }
	}
}