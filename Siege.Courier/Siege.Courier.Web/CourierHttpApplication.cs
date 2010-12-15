using System;
using System.Web;
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
            if (!ServiceLocator.HasTypeRegistered(typeof(Subscriber.For<TMessage>)))
            {
                ServiceLocator.Register<Singleton>(Given<Subscriber.For<TMessage>>.Then<TSubscriber>());
                ServiceLocator.Register(Given<IServiceBus>.InitializeWith(bus => bus.Subscribe(ServiceLocator.GetInstance<TSubscriber>())));
            }
        }

        protected void AddResponse<TResponse>(string name) where TResponse : Response
        {
            ServiceLocator.Register(Given<Response>.Then<TResponse>(name.ToLower()));
        }

        public void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.BufferOutput = true;
            HttpContext.Current.Response.Buffer = true;
        }
    }
}