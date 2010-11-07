using System;
using System.Web;
using Siege.Requisitions;
using Siege.Requisitions.Extensions.ConditionalAwareness;
using Siege.Requisitions.Extensions.Conventions;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.RegistrationPolicies;

namespace Siege.Courier.Web.Conventions
{
    public class ServiceBusConvention : IConvention
    {
        public Action<IServiceLocator> Build()
        {
            return serviceLocator =>
                   serviceLocator
                       .Register(Awareness.Of(serviceLocator.GetInstance<HandlerContext>))
                       .Register(Given<ServiceBusRoute>.Then<ServiceBusRoute>())
                       .Register(Given<IHttpHandler>.When<HandlerContext>(ctx => ctx.Type == null).Then<MvcControllerHandler>())
                       .Register(Given<IHttpHandler>.Then<ServiceBusHandler>())
                       .Register(Given<ServiceBusRouteHandler>.Then<ServiceBusRouteHandler>())
                       .Register<Singleton>(Given<IServiceBus>.Then<SimpleServiceBus>())
                       .Register(Given<TypeFinder>.Then<TypeFinder>())
                       .Register(Given<HandlerContext>.Then<HandlerContext>())
                       .Register(Given<NativeAdapter>.Then<NativeAdapter>());
        }
    }
}