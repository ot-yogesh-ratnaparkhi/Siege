using System.ServiceModel;
using System.ServiceProcess;
using Siege.Eventing.Messages;
using Siege.Eventing.Subscribers;
using Siege.ServiceLocator;
using Siege.ServiceLocator.Extensions.ExtendedRegistrationSyntax;
using Siege.ServiceLocator.Native;

namespace Siege.Eventing.WindowsServices
{
    public abstract class CourierService : ServiceBase
    {
        public ServiceHost ServiceHost;
        protected readonly IServiceLocator ServiceLocator = new ThreadedServiceLocator(new SiegeAdapter());
        protected MessageMap MessageMap = new MessageMap();

        protected override void OnStart(string[] args)
        {
            ServiceLocator
                .Register(Given<MessageMap>.Then(this.MessageMap))
                .Register(Given<DelegateManager>.Then<DelegateManager>())
                .Register(Given<NativeAdapter>.Then<NativeAdapter>())
                .Register(Given<IServiceBus>.Then<ServiceBus>())
                .Register(Given<IMessageTracker>.Then<ThreadedMessageTracker>())
                .Register(Given<IMessageBucket>.Then<ThreadedMessageBucket>());

            if (ServiceHost != null)
            {
                ServiceHost.Close();
            }
        }

        protected void AddSubscriber<TSubscriber, TMessage>() where TSubscriber : Subscriber.For<TMessage> where TMessage : IMessage
        {
            ServiceLocator 
                .Register(Given<TSubscriber>.Then<TSubscriber>())
                .Register(Given<IServiceBus>.InitializeWith(bus => bus.Subscribe(ServiceLocator.GetInstance<TSubscriber>())));
        }

        protected void MapMessage<TMessage, TAdapter>() where TMessage : IMessage where TAdapter : IProtocolAdapter
        {
            MessageMap.Map<TMessage>(ServiceLocator.GetInstance<TAdapter>());
        }

        protected void StartService<TService>()
        {
            ServiceHost = new ServiceHost(ServiceLocator.GetInstance<TService>());
            ServiceHost.Open();
        }

        protected override void OnStop()
        {
            if (ServiceHost != null)
            {
                ServiceHost.Close();
                ServiceHost = null;
            }
        }
    }
}
