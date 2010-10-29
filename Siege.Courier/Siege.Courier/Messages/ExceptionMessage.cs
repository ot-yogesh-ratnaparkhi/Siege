using System;
using Siege.Courier.Subscribers;

namespace Siege.Courier.Messages
{
    [Serializable]
    public class ExceptionMessage : IMessage
    {
        public ExceptionMessage(IMessage message, ISubscriber subscriber, Exception exception)
        {
            Message = message;
            Exception = exception;
            Subscriber = subscriber != null ? subscriber.GetType() : null;
        }

        public IMessage Message { get; set; }
        public Exception Exception { get; set; }
        public Type Subscriber { get; set; }
    }
}