using System;
using System.Runtime.Serialization;

namespace DragonFiesta.InterNetwork
{
    public interface IMessage
    {
        Guid Id { get; set; }
    }
}