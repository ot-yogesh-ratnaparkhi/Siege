using System.Web.Mvc;
using Siege.Security.SampleApplication.Areas.Security.Controllers;

namespace Siege.Security.SampleApplication.Areas.Security
{
    public class SecurityAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Security";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Security_default",
                "Security/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { typeof(HomeController).Namespace }
            );
        }
    }
}
