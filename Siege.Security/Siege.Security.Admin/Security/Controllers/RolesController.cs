using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Admin.Security.Models;
using Siege.Security.Providers;
using Siege.Security.Web;

namespace Siege.Security.Admin.Security.Controllers
{
    public class RolesController : SecurityController<Role, IRoleProvider>
    {
        public RolesController(IRoleProvider provider, IAuthenticationProvider authenticationProvider) : base(provider, authenticationProvider)
        {
        }

        public JsonResult ForApplicationAndConsumer(Application application, Consumer consumer, JqGridConfiguration configuration, IApplicationProvider applicationProvider)
        {
            var loggedInUser = (User) HttpContext.User;
            

            var roles = provider.GetForApplicationAndConsumer(application, consumer, loggedInUser.Can("CanAdministerAllSecurity"));

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


        public JsonResult ForUser(int? id, JqGridConfiguration configuration, IUserProvider userProvider)
        {
            var loggedInUser = (User)HttpContext.User;
            IList<Role> roles;
            IList<Role> userRoles = new List<Role>();
            if (id != null)
            {
                var user = userProvider.Find(id);
                roles = provider.GetForApplicationAndConsumer(GetApplication(), GetConsumer(), loggedInUser.Can("CanAdministerAllSecurity"));
                userRoles = user.Roles;
            }
            else
            {
                roles = provider.GetForApplicationAndConsumer(GetApplication(), GetConsumer(), loggedInUser.Can("CanAdministerAllSecurity"));
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

        public JsonResult ForGroup(int? id, JqGridConfiguration configuration, IGroupProvider groupProvider)
        {
            var loggedInUser = (User)HttpContext.User;
            IList<Role> roles;
            IList<Role> groupRoles = new List<Role>();
            if (id != null)
            {
                var group = groupProvider.Find(id);
                roles = provider.GetForApplicationAndConsumer(GetApplication(), GetConsumer(), loggedInUser.Can("CanAdministerAllSecurity"));
                groupRoles = group.Roles;
            }
            else
            {
                roles = provider.GetForApplicationAndConsumer(GetApplication(), GetConsumer(), loggedInUser.Can("CanAdministerAllSecurity"));
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
                           role.Permissions.Where(p => !string.IsNullOrEmpty(p.Name)).Select(p => p.Name)
                    }
                })
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(RoleModel model, List<Permission> selectedPermissions)
        {
            var role = model.IsNew ? new Role() : this.provider.Find(model.RoleID);

            role.Name = model.Name;
            role.Description = model.Description;
            role.IsActive = model.IsActive;

            role.Permissions.Clear();
            selectedPermissions.ForEach(p => role.Permissions.Add(p));

            if (model.IsNew)
            {
                var localUser = (User)HttpContext.User;
                role.Consumer = localUser.Consumer;
            }

            this.provider.Save(role);

            return Json(new { result = true });
        }

        public ActionResult Edit(int? id)
        {
            var role = this.provider.Find(id);

            var model = new RoleModel
            {
                RoleID = role.ID,
                IsActive = role.IsActive,
                Name = role.Name,
                ApplicationID = GetApplication().ID,
                Description = role.Description
            };

            return View(model);
        }

        public JsonResult List(JqGridConfiguration configuration, IApplicationProvider applicationProvider)
        {
            var user = (User)HttpContext.User.Identity;
            return ForApplicationAndConsumer(GetApplication(), GetConsumer(), configuration, applicationProvider);
        }
    }
}