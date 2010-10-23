using System.Web.Routing;
using Courier_Sample.Controllers;
using Siege.Courier;
using Siege.Requisitions;
using Siege.Requisitions.Extensions.Conventions;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.SiegeAdapter;
using Siege.Requisitions.Web;
using Siege.Requisitions.Web.Conventions;

namespace Courier_Sample
{
    public class MvcApplication : ServiceLocatorHttpApplication
    {
        protected override IServiceLocatorAdapter GetServiceLocatorAdapter()
        {
            return new SiegeAdapter();
        }

        protected override void OnApplicationStarted()
        {
            ServiceLocator
                .Register(Using.Convention<ControllerConvention<HomeController>>())
                .Register(Given<ServiceBusRoute>.Then<ServiceBusRoute>())
                .Register(Given<ServiceBusRouteHandler>.Then<ServiceBusRouteHandler>())
                .Register(Given<IServiceBus>.Then<SimpleServiceBus>());

            RouteTable.Routes.Add(ServiceLocator.GetInstance<ServiceBusRoute>());
        }
    }
}