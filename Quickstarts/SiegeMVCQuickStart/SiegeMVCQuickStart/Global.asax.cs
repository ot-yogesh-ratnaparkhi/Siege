using System.Web.Mvc;
using System.Web.Routing;
using Siege.Container.NinjectAdapter;
using Siege.ServiceLocation;
using Siege.ServiceLocation.HttpIntegration;
using SiegeMVCQuickStart.Controllers;
using SiegeMVCQuickStart.SampleClasses;

namespace SiegeMVCQuickStart
{
    public class MvcApplication : SiegeHttpApplication
    {
        protected override IServiceLocatorAdapter GetServiceLocatorAdapter()
        {
            return new NinjectAdapter();
        }

        public override void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
        }

        protected override void OnApplicationStarted()
        {
            ServiceLocator
                .Register(Given<HomeController>.Then<HomeController>())
                .Register(Given<HomeController>
                            .When<User>(user => user.Role == UserRoles.SuperUser)
                            .Then<SuperUserHomeController>());

            base.OnApplicationStarted();
        }
    }
}