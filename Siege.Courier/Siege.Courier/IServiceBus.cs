using Siege.Courier.Messages;
using Siege.Courier.Subscribers;

namespace Siege.Courier
{
    public interface IServiceBus
    {
        void Subscribe<TMessage>(Subscriber.For<TMessage> subscriber) where TMessage : IMessage;
        void Unsubscribe<TMessage>(Subscriber.For<TMessage> subscriber) where TMessage : IMessage;
        void Publish<TMessage>(TMessage message) where TMessage : IMessage;
    }
}