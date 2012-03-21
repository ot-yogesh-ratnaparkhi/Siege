using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Providers;
using Siege.Security.Web;

namespace Siege.Security.SampleApplication.Areas.Security.Controllers
{
    public class UsersController : SecurityController<User, Guid?, IUserProvider>
    {
        public UsersController(IUserProvider provider) : base(provider)
        {
        }

        public JsonResult ForApplication(Application application, JqGridConfiguration configuration)
        {
            var users = provider.GetForApplication(application);

            var jsonData = new
            {
                total = 1, 
                page = configuration.PageIndex,
                records = users.Count, 
                rows = users.Select(user => new
                {
                    id = user.ID, 
                    cell = new object[]
                    {
                        user.Name,
                        user.Permissions.Select(p => p.Name),
                        user.IsActive ? "Yes" : "No",
                        user.IsLockedOut ? "Yes" : "No",
                        user.ID.ToString()
                    }
                })
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult List(JqGridConfiguration configuration)
        {
            var user = (User) HttpContext.User.Identity;
            return ForApplication(user.Application, configuration);
        }

        public ActionResult Save(User item, List<Role> selectedRoles, List<Group> selectedGroups)
        {
            selectedRoles.ForEach(r => item.Roles.Add(r));
            selectedGroups.ForEach(g => item.Groups.Add(g));
            if(item.Application == null)
            {
                var user = (User) HttpContext.User;
                item.Application = user.Application;
            }
            this.provider.Save(item);

            return Json(new {result = true});
        }
    }
}