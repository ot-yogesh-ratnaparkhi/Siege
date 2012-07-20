using System.Web.Mvc;
using MvcContrib.PortableAreas;

namespace Siege.Security.Admin.Security
{
    public class SecurityRegistration : PortableAreaRegistration
    {
        public override string AreaName
        {
            get { return "Security"; }
        }

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
        {
            base.RegisterArea(context, bus);

            context.MapRoute(
                "Security_Default",
                AreaName + "/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });

        }
    }
}
