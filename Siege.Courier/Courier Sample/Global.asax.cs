using System.Web.Routing;
using Courier_Sample.Controllers;
using Siege.Courier;
using Siege.Courier.WCF;
using Siege.Courier.Web;
using Siege.Courier.Web.Conventions;
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
                .Register(Using.Convention<AspNetMvcConvention>())
                .Register(Using.Convention<ServiceBusConvention>())
                .Register(Using.Convention<ControllerConvention<HomeController>>())
                .Register(Given<IServiceBus>.InitializeWith(bus => bus.Subscribe(new WCFSubscriber())));

            RouteTable.Routes.Add(ServiceLocator.GetInstance<ServiceBusRoute>());
        }
    }
}