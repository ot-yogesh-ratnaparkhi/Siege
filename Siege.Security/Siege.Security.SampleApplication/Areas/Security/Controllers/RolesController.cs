using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Providers;
using Siege.Security.Web;

namespace Siege.Security.SampleApplication.Areas.Security.Controllers
{
    public class RolesController : SecurityController<Role, int?, IRoleProvider>
    {
        public RolesController(IRoleProvider provider) : base(provider)
        {
        }

        public JsonResult ForApplication(Application application, JqGridConfiguration configuration)
        {
            var roles = provider.GetForApplication(application);

            var jsonData = new
            {
                total = 1,
                page = configuration.PageIndex,
                records = roles.Count,
                rows = roles.Select(role => new
                {
                    id = role.ID,
                    cell = new object[]
                    {
                        role.Name,
                        role.Permissions.Where(p => !string.IsNullOrEmpty(p.Name)).Select(p => p.Name),
                        role.IsActive ? "Yes" : "No",
                        role.ID.ToString()
                    }
                })
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ForUser(User user, JqGridConfiguration configuration)
        {
            IList<Role> roles;
            IList<Role> userRoles = new List<Role>();
            if (user != null)
            {
                roles = provider.GetForApplication(user.Application);
                userRoles = user.Roles;
            }
            else
            {
                user = (User)HttpContext.User;
                roles = provider.GetForApplication(user.Application);
            }

            var jsonData = new
            {
                total = 1,
                page = configuration.PageIndex,
                records = roles.Count,
                rows = roles.Select(role => new
                {
                    id = role.ID,
                    cell = new object[]
                    {
                           role.ID,
                           userRoles.Any(r => r.ID == role.ID) ? "true" : "false",
                           role.Name,
                           role.Permissions.Select(p => p.Name)
                    }
                })
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ForGroup(Group group, JqGridConfiguration configuration)
        {
            IList<Role> roles;
            IList<Role> groupRoles = new List<Role>();
            if (group != null)
            {
                roles = provider.GetForApplication(group.Application);
                groupRoles = group.Roles;
            }
            else
            {
                var user = (User)HttpContext.User;
                roles = provider.GetForApplication(user.Application);
            }

            var jsonData = new
            {
                total = 1,
                page = configuration.PageIndex,
                records = roles.Count,
                rows = roles.Select(role => new
                {
                    id = role.ID,
                    cell = new object[]
                    {
                           role.ID,
                           groupRoles.Any(r => r.ID == role.ID) ? "true" : "false",
                           role.Name,
                           roles.Where(r => r.Permissions.Count > 0).Select(r => r.Permissions.Where(p => !string.IsNullOrEmpty(p.Name)).Select(p => p.Name))
                    }
                })
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(Role item, List<Permission> selectedPermissions)
        {
            selectedPermissions.ForEach(p => item.Permissions.Add(p));
            if (item.Application == null)
            {
                var user = (User)HttpContext.User;
                item.Application = user.Application;
            }
            this.provider.Save(item);

            return Json(new { result = true });
        }

        public JsonResult List(JqGridConfiguration configuration)
        {
            var user = (User)HttpContext.User.Identity;
            return ForApplication(user.Application, configuration);
        }
    }
}