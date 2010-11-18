using Siege.Courier.Messages;

namespace Siege.Courier.Subscribers
{
    public class Subscriber
    {
        public interface For<in TMessage> : ISubscriber where TMessage : IMessage
        {
            void Receive(TMessage message);
        }
    }

    public interface ISubscriber
    {
    }
}