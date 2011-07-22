using Siege.Eventing.Messages;
using Siege.Eventing.Subscribers;

namespace Siege.Eventing
{
    public interface IProtocolAdapter : ISubscriber
    {
        void Receive(IMessage message);
    }
}