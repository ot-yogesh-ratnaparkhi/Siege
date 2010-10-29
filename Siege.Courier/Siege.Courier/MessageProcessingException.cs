using System;
using System.Runtime.Serialization;

namespace Siege.Courier
{
    [Serializable]
    public class MessageProcessingException : Exception
    {
        public MessageProcessingException()
        {
        }

        public MessageProcessingException(string message, Type messageType) : base(message)
        {
            Data["MessageType"] = messageType != null ? messageType.FullName : null;
        }

        public MessageProcessingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MessageProcessingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}