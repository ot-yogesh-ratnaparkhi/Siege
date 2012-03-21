using System.Web.Mvc;

namespace Siege.Security.SampleApplication.Areas.Security.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}