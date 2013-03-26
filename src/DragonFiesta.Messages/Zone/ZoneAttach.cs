using System;
using DragonFiesta.InterNetwork;
namespace DragonFiesta.Messages.Zone
{
     [Serializable]
    public class ZoneAttach : IMessage
    {
         public Guid Id { get; set; }
         public string IP { get; set; }
         public ushort Port { get; set; }
         public string Password { get; set; }
         public string QueueName { get; set; }

         [NonSerialized]
         private Action<IMessage> callback;
         public Action<IMessage> Callback
         {
             get { return callback; }
             set { callback = value; }
         }

    }
}
