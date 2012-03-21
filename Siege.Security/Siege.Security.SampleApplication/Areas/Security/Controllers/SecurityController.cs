using System.Web.Mvc;
using Siege.Security.Entities;
using Siege.Security.Providers;

namespace Siege.Security.SampleApplication.Areas.Security.Controllers
{
    public abstract class SecurityController<T, TID, TProvider> : Controller where T : SecurityEntity<TID>
                                                                             where TProvider : IProvider<T, TID>
    {
        protected readonly TProvider provider;

        protected SecurityController(TProvider provider)
        {
            this.provider = provider;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Edit(T item)
        {
            return View(item);
        }
    }
}