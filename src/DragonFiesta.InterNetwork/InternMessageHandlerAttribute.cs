using System;

namespace DragonFiesta.InterNetwork
{
    public class InternMessageHandlerAttribute : Attribute
    {
        public InternMessageHandlerAttribute(Type pType)
        {
            this.Type = pType;
        }

        public Type Type { get; private set; } 
    }
}