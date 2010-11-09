using System;
using Siege.Courier.Messages;

namespace Siege.Courier.WCF
{
    public class WCFAdapter : IProtocolAdapter
    {
        private readonly WCFProxy<IWCFProtocol> protocol;
        private readonly Func<IServiceBus> serviceBus;
        private readonly DelegateManager manager;
        
        public WCFAdapter(WCFProxy<IWCFProtocol> protocol, Func<IServiceBus> serviceBus, DelegateManager manager)
        {
            this.protocol = protocol;
            this.serviceBus = serviceBus;
            this.manager = manager;
        }

        public void Receive(IMessage message)
        {
            var results = protocol.Perform(x => x.Send(message));

            foreach(IMessage result in results)
            {
                manager.CreateDelegate(result, serviceBus).DynamicInvoke(result);
            }
        }
    }
}