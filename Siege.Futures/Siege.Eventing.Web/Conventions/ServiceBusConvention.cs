using System;
using System.Web;
using Siege.ServiceLocator;
using Siege.ServiceLocator.Extensions.ConditionalAwareness;
using Siege.ServiceLocator.Extensions.ExtendedRegistrationSyntax;
using Siege.ServiceLocator.RegistrationPolicies;

namespace Siege.Eventing.Web.Conventions
{
    public class ServiceBusConvention : ServiceLocator.Extensions.Conventions.IConvention
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
                       .Register<Singleton>(Given<IServiceBus>.Then<ServiceBus>())
                       .Register(Given<TypeFinder>.Then<TypeFinder>())
                       .Register(Given<HandlerContext>.Then<HandlerContext>())
                       .Register(Given<NativeAdapter>.Then<NativeAdapter>());
        }
    }
}