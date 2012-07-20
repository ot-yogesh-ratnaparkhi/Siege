using System;
using Siege.Eventing.Subscribers;

namespace Siege.Eventing.Messages
{
    public class FailureMessage : IMessage
    {
        
    }

    [Serializable]
    public class ExceptionMessage : FailureMessage
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