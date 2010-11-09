using Siege.Courier.Messages;

namespace Siege.Courier.Web
{
    public class MessageValidationFailedMessage<TMessage> : FailureMessage where TMessage : IMessage
    {
        private readonly TMessage innerMessage;

        public MessageValidationFailedMessage(TMessage innerMessage)
        {
            this.innerMessage = innerMessage;
        }

        public TMessage InnerMessage
        {
            get { return innerMessage; }
        }
    }
}