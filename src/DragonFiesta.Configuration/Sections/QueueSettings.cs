using System.Collections.Generic;

namespace DragonFiesta.Configuration.Sections
{
    public class QueueSettings
    {
        public Queue MyQueue = new Queue() {Name = "name", Path = @".\PRIVATE$\name"};

        public List<Queue> SubscribeTo = new List<Queue>();
    }
    public class Queue
    {
        public string Name;
        public string Path;
    }
}