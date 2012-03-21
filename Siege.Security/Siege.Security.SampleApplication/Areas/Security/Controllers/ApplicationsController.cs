using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Providers;
using Siege.Security.Web;

namespace Siege.Security.SampleApplication.Areas.Security.Controllers
{
    public class ApplicationsController : SecurityController<Application, Guid?, IApplicationProvider>
    {
        public ApplicationsController(IApplicationProvider provider) : base(provider)
        {
        }

        public JsonResult List(JqGridConfiguration configuration)
        {
            IList<Application> applications = provider.GetAllApplications();

            var jsonData = new
            {
                total = 1,
                page = configuration.PageIndex,
                records = applications.Count,
                rows = applications.Select(application => new
                {
                    id = application.ID,
                    cell = new object[]
                    {
                        application.Name,
                        application.Description,
                        application.IsActive ? "Yes" : "No",
                        application.ID.ToString()
                    }
                })
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(Application item)
        {
            this.provider.Save(item);

            return Json(new { result = true });
        }
    }
}