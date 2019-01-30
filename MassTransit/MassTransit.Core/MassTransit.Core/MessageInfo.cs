using System;

namespace MassTransit.Core
{
    public class MessageInfo
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }
}
