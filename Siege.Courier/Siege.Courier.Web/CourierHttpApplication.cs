using System.Web.Routing;
using Siege.Courier.Messages;
using Siege.Courier.Subscribers;
using Siege.Courier.Web.Conventions;
using Siege.Courier.Web.Responses;
using Siege.Requisitions.Extensions.Conventions;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.RegistrationPolicies;
using Siege.Requisitions.Web;

namespace Siege.Courier.Web
{
    public abstract class CourierHttpApplication : ServiceLocatorHttpApplication
    {
        protected MessageMap Map = new MessageMap();

        protected override void OnApplicationStarted()
        {
            ServiceLocator
                .Register(Using.Convention<AspNetMvcConvention>())
                .Register(Using.Convention<ServiceBusConvention>())
                .Register(Given<MessageMap>.Then(Map));

            AddResponse<ViewResponse>("view");
            AddResponse<JsonResponse>("json");

            MapMessage<ExceptionMessage, NativeAdapter>();

            RouteTable.Routes.Add(ServiceLocator.GetInstance<ServiceBusRoute>());
            base.OnApplicationStarted();
        }

        protected void MapMessage<TMessage, TAdapter>()
            where TMessage : IMessage
            where TAdapter : IProtocolAdapter
        {
            Map.Map<TMessage>(ServiceLocator.GetInstance<TAdapter>());
        }

        protected void AddSubscriber<TSubscriber, TMessage>()
            where TSubscriber : Subscriber.For<TMessage>
            where TMessage : IMessage
        {
            if(!ServiceLocator.HasTypeRegistered(typeof(TSubscriber))) ServiceLocator.Register<Singleton>(Given<TSubscriber>.Then<TSubscriber>());
            ServiceLocator.Register(Given<IServiceBus>.InitializeWith(bus => bus.Subscribe(ServiceLocator.GetInstance<TSubscriber>())));
        }

        protected void AddSubscriber<TSubscriber>()
            where TSubscriber : Subscriber.All
        {
            ServiceLocator
                .Register<Singleton>(Given<TSubscriber>.Then<TSubscriber>())
                .Register(Given<IServiceBus>.InitializeWith(bus => bus.Subscribe(ServiceLocator.GetInstance<TSubscriber>())));
        }

        protected void AddResponse<TResponse>(string name) where TResponse : Response
        {
            ServiceLocator.Register(Given<Response>.Then<TResponse>(name.ToLower()));
        }
    }
}