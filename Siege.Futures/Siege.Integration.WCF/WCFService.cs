using System;
using System.Collections.Generic;
using System.ServiceModel;
using Siege.Eventing;
using Siege.Eventing.Messages;

namespace Siege.Integration.WCF
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class WCFService : IWCFProtocol
    {
        private readonly Func<IServiceBus> serviceBus;
        private readonly IMessageTracker tracker;
        private readonly IMessageBucket messages;
        private readonly DelegateManager manager;

        public WCFService(Func<IServiceBus> serviceBus, IMessageTracker tracker, IMessageBucket messages, DelegateManager manager)
        {
            this.serviceBus = serviceBus;
            this.tracker = tracker;
            this.messages = messages;
            this.manager = manager;
        }

        public List<IMessage> Send(IMessage message)
        {
            messages.Clear();
            tracker.Track(message);

            manager.CreateDelegate(message, serviceBus).DynamicInvoke(message);

            tracker.Clear();

            return messages.All();
        }
    }
}