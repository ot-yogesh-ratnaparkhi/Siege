using System;
using System.Web;
using System.Web.Routing;
using Siege.Eventing.Messages;
using Siege.Eventing.Subscribers;
using Siege.Eventing.Web.Conventions;
using Siege.Eventing.Web.Responses;
using Siege.ServiceLocator.Extensions.Conventions;
using Siege.ServiceLocator.Extensions.ExtendedRegistrationSyntax;
using Siege.ServiceLocator.RegistrationPolicies;
using Siege.ServiceLocator.Web;

namespace Siege.Eventing.Web
{
    public abstract class EventfulHttpApplication : ServiceLocatorHttpApplication
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