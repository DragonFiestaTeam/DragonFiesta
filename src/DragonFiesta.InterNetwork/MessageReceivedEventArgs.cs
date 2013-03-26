using System;
using System.Messaging;

namespace DragonFiesta.InterNetwork
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public Message Message { get; set; } 
    }
}