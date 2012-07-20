using System.Linq;
using System.Web.Mvc;
using Siege.Security.Admin.Security.Models;
using Siege.Security.Principals;
using Siege.Security.Providers;
using Siege.Security.Web.Attributes;

namespace Siege.Security.Admin.Security.Controllers
{
    [RequiresOneOfThePermissions("CanAdministerSecurity", "CanAdministerAllSecurity")]
    public class HomeController : Controller
    {
        private readonly IUserProvider userProvider;

        public HomeController(IUserProvider userProvider)
        {
            this.userProvider = userProvider;
        }

        public ActionResult Index()
        {
            var principal = (SecurityPrincipal) HttpContext.User;
            var user = userProvider.FindByUserName(principal.Name);
            var model = new SecurityModel
            {
                UserID = user.ID,
                ApplicationID = user.Consumer.Applications.First().ID,
                ConsumerID = user.Consumer.ID
            };

            return View(model);
        }
    }
}