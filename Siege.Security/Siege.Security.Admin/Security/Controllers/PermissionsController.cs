using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Principals;
using Siege.Security.Providers;
using Siege.Security.Web;

namespace Siege.Security.Admin.Security.Controllers
{
    public class PermissionsController : SecurityController<Permission, IPermissionProvider>
    {
        public PermissionsController(IPermissionProvider provider, IAuthenticationProvider authenticationProvider) : base(provider, authenticationProvider)
        {
        }

        public JsonResult List(JqGridConfiguration configuration)
        {
            var user = (ISecurityPrincipal) HttpContext.User;
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

        public JsonResult ForRole(Role role, Application application, JqGridConfiguration configuration)
        {
            var user = (ISecurityPrincipal)HttpContext.User;
            var permissions = provider.ForApplication(application, user.Can("CanAdministerAllSecurity"));
            IList<Permission> rolePermissions = new List<Permission>();

            if (role != null)
            {
                rolePermissions = role.Permissions;
            }


            var jsonData = new
            {
                total = 1,
                page = configuration.PageIndex,
                records = permissions.Count,
                rows = permissions.Select(r => new
                {
                    id = r.ID,
                    cell = new object[]
                    {
                           r.ID,
                           rolePermissions.Any(p => p.ID == r.ID) ? "true" : "false",
                           r.Name,
                    }
                })
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

    }
}