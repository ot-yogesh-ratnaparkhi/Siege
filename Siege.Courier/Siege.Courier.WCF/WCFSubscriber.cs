using Siege.Courier.Messages;
using Siege.Courier.Subscribers;

namespace Siege.Courier.WCF
{
    public class WCFSubscriber : Subscriber.For<IMessage>
    {
        private readonly WCFProxy<IWCFProtocol> protocol;

        public WCFSubscriber(WCFProxy<IWCFProtocol> protocol)
        {
            this.protocol = protocol;
        }

        public void Receive(IMessage message)
        {
            protocol.Perform(x => x.Send(message));
        }
    }
}