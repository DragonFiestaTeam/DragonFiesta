using System;
using DragonFiesta.InterNetwork;

namespace DragonFiesta.Messages.Main
{
    [Serializable]
    public class RegisterListener : IMessage
    {
        public Guid Id { get; set; }
        public string QueuePath { get; set; }
    }
}