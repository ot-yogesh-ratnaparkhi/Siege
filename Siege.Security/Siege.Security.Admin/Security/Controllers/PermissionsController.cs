using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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

        public JsonResult ForRole(int? id, JqGridConfiguration configuration, IRoleProvider roleProvider)
        {

            var user = (User)HttpContext.User;
            var permissions = provider.All(user.Can("CanAdministerAllSecurity"));
            IList<Permission> rolePermissions = new List<Permission>();

            if (id != null)
            {
                var role = roleProvider.Find(id);
                rolePermissions = role.Permissions;
            }


            var jsonData = new
            {
                total = 1,
                page = configuration.PageIndex,
                records = permissions.Count,
                rows = permissions.Select(role => new
                {
                    id = role.ID,
                    cell = new object[]
                    {
                           role.ID,
                           rolePermissions.Any(r => r.ID == role.ID) ? "true" : "false",
                           role.Name,
                    }
                })
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

    }
}