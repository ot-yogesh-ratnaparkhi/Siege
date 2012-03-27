using System;
using System.Web.Mvc;
using Siege.Security.Entities;
using Siege.Security.Providers;
using Siege.Security.Web.Attributes;

namespace Siege.Security.Admin.Security.Controllers
{
    [HandleAjaxErrors]
    [RequiresOneOfThePermissionsAttribute("CanAdministerSecurity", "CanAdministerAllSecurity")]
    public abstract class SecurityController<T, TProvider> : Controller where T : SecurityEntity
                                                                             where TProvider : IProvider<T>
    {
        protected readonly TProvider provider;
        protected readonly IAuthenticationProvider authenticationProvider;

        protected SecurityController(TProvider provider, IAuthenticationProvider authenticationProvider)
        {
            this.provider = provider;
            this.authenticationProvider = authenticationProvider;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        protected Application GetApplication()
        {
            throw new NotImplementedException();
        }

        protected Consumer GetConsumer()
        {
            throw new NotImplementedException();
        }
    }
}