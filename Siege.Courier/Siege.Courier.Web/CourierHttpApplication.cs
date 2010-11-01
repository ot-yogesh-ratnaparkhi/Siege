using System.Web.Routing;
using Siege.Courier.Messages;
using Siege.Courier.Processors;
using Siege.Courier.Subscribers;
using Siege.Courier.Web.Conventions;
using Siege.Courier.Web.Responses;
using Siege.Requisitions.Extensions.Conventions;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.Web;

namespace Siege.Courier.Web
{
    public abstract class CourierHttpApplication : ServiceLocatorHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            ServiceLocator
                .Register(Using.Convention<AspNetMvcConvention>())
                .Register(Using.Convention<ServiceBusConvention>());

            AddResponse<ViewResponse>("view");
            AddResponse<JsonResponse>("json");

            RouteTable.Routes.Add(ServiceLocator.GetInstance<ServiceBusRoute>());
            base.OnApplicationStarted();
        }

        protected void AddSubcriber<TSubscriber, TMessage>() 
            where TSubscriber : Subscriber.For<TMessage>
            where TMessage : IMessage
        {
            ServiceLocator
                .Register(Given<TSubscriber>.Then<TSubscriber>())
                .Register(Given<IServiceBus>.InitializeWith(bus => bus.Subscribe(ServiceLocator.GetInstance<TSubscriber>())));
        }

        protected void AddPreProcessor<TPreProcessor, TMessage>()
            where TPreProcessor : PreProcessor.For<TMessage>
            where TMessage : IMessage
        {
            ServiceLocator.Register(Given<PreProcessor.For<TMessage>>.Then<TPreProcessor>());
            ServiceLocator.Register(Given<TPreProcessor>.Then<TPreProcessor>());
        }

        protected void AddPostProcessor<TPostProcessor, TMessage>()
            where TPostProcessor : PostProcessor.For<TMessage>
            where TMessage : IMessage
        {
            ServiceLocator.Register(Given<PostProcessor.For<TMessage>>.Then<TPostProcessor>());
            ServiceLocator.Register(Given<TPostProcessor>.Then<TPostProcessor>());
        }

        protected void AddResponse<TResponse>(string name) where TResponse : Response
        {
            ServiceLocator.Register(Given<Response>.Then<TResponse>(name.ToLower()));
        }
    }
}