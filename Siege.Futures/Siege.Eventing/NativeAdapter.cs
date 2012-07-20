using System;
using Siege.Eventing.Messages;

namespace Siege.Eventing
{
    public class NativeAdapter : IProtocolAdapter
    {
        private readonly Func<IServiceBus> serviceBus;
        private readonly DelegateManager manager;

        public NativeAdapter(Func<IServiceBus> serviceBus, DelegateManager manager)
        {
            this.serviceBus = serviceBus;
            this.manager = manager;
        }

        public void Receive(IMessage message)
        {
            manager.CreateDelegate(message, serviceBus).DynamicInvoke(message);
        }
    }
}