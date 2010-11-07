using Siege.Courier.Messages;
using Siege.Courier.Subscribers;

namespace Siege.Courier
{
    public interface IProtocolAdapter : ISubscriber
    {
        void Receive(IMessage message);
    }
}