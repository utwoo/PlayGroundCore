using System;

namespace MassTransit.Core
{
    public interface IMessageRequest
    {
        Guid Id { get; set; }
        string Message { get; set; }
    }
}
