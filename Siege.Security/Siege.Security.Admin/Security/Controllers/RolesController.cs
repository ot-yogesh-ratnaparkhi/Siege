using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Admin.Security.Models;
using Siege.Security.Principals;
using Siege.Security.Providers;
using Siege.Security.Web;

namespace Siege.Security.Admin.Security.Controllers
{
    public class RolesController : SecurityController<Role, IRoleProvider>
    {
        public RolesController(IRoleProvider provider, IAuthenticationProvider authenticationProvider) : base(provider, authenticationProvider)
        {
        }

        public JsonResult ForConsumerAndApplication(Consumer consumer, JqGridConfiguration configuration)
        {
            var loggedInUser = (SecurityPrincipal)HttpContext.User;

            var roles = provider.GetForConsumer(consumer, loggedInUser.Can("CanAdministerAllSecurity"));

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


        public JsonResult ForUser(User user, Application application, Consumer consumer, JqGridConfiguration configuration)
        {
            var loggedInUser = (SecurityPrincipal)HttpContext.User;
            IList<Role> roles;
            IList<Role> userRoles = new List<Role>();
            
            if (user != null)
            {
                roles = provider.GetForConsumer(consumer, loggedInUser.Can("CanAdministerAllSecurity"));
                userRoles = user.Roles;
            }
            else
            {
                roles = provider.GetForApplicationAndConsumer(application, consumer, loggedInUser.Can("CanAdministerAllSecurity"));
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

        public JsonResult ForGroup(Group group, Application application, Consumer consumer, JqGridConfiguration configuration)
        {
            var loggedInUser = (SecurityPrincipal)HttpContext.User;
            IList<Role> roles;
            IList<Role> groupRoles = new List<Role>();
            
            if (group != null)
            {
                roles = provider.GetForApplicationAndConsumer(application, consumer, loggedInUser.Can("CanAdministerAllSecurity"));
                groupRoles = group.Roles;
            }
            else
            {
                roles = provider.GetForApplicationAndConsumer(application, consumer, loggedInUser.Can("CanAdministerAllSecurity"));
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

        public ActionResult Save(Consumer consumer, RoleModel model, List<Permission> selectedPermissions)
        {
            var role = model.IsNew ? new Role() : this.provider.Find(model.RoleID);

            role.Name = model.Name;
            role.Description = model.Description;
            role.IsActive = model.IsActive;

            role.Permissions.Clear();
            selectedPermissions.ForEach(p => role.Permissions.Add(p));

            if (model.IsNew)
            {
                role.Consumer = consumer;
            }

            this.provider.Save(role);

            return Json(new { result = true });
        }

        public ActionResult Edit(Role role)
        {
            var model = new RoleModel
            {
                RoleID = role.ID,
                IsActive = role.IsActive,
                Name = role.Name,
                ConsumerID = role.Consumer.ID,
                Description = role.Description
            };

            return View(model);
        }

        public JsonResult List(Consumer consumer, JqGridConfiguration configuration)
        {
            return ForConsumerAndApplication(consumer, configuration);
        }
    }
}