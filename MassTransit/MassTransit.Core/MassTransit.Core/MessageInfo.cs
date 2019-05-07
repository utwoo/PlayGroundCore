using System;

namespace MassTransit.Core
{
    public class MessageInfo: IMessageInfo
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }
}
