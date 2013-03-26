using System;

namespace DragonFiesta.InterNetwork
{
    public interface IExpectAnAnswer : IMessage
    {
        Action<IMessage> Callback { get; }
    }
}