using System;
using Siege.Courier.Messages;

namespace Siege.Courier.WCF
{
    public class WCFAdapter : IProtocolAdapter
    {
        private readonly WCFProxy<IWCFProtocol> protocol;
        private readonly Func<IServiceBus> serviceBus;
        
        public WCFAdapter(WCFProxy<IWCFProtocol> protocol, Func<IServiceBus> serviceBus)
        {
            this.protocol = protocol;
            this.serviceBus = serviceBus;
        }

        public void Receive(IMessage message)
        {
            var results = protocol.Perform(x => x.Send(message));

            foreach(IMessage result in results)
            {
                serviceBus().Publish(result);
            }
        }
    }
}