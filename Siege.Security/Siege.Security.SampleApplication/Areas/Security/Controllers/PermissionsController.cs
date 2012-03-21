using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Providers;
using Siege.Security.Web;

namespace Siege.Security.SampleApplication.Areas.Security.Controllers
{
    public class PermissionsController : SecurityController<Permission, int?, IPermissionProvider>
    {
        public PermissionsController(IPermissionProvider provider) : base(provider)
        {
        }

        public JsonResult List(JqGridConfiguration configuration)
        {
            var user = (User) HttpContext.User;
            var permissions = provider.All(user.Can("CanAdministerAllSecurity"));

            var jsonData = new
            {
                total = 1,
                page = configuration.PageIndex,
                records = permissions.Count,
                rows = permissions.Select(permission => new
                {
                    id = permission.ID,
                    cell = new object[]
                    {
                        permission.Name,
                        permission.Description,
                        permission.ID.ToString()
                    }
                })
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}