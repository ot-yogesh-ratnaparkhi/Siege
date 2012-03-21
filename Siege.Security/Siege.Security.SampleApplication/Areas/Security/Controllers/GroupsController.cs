using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Providers;
using Siege.Security.Web;

namespace Siege.Security.SampleApplication.Areas.Security.Controllers
{
    public class GroupsController : SecurityController<Group, int?, IGroupProvider>
    {
        public GroupsController(IGroupProvider provider) : base(provider)
        {
        }

        public JsonResult List(JqGridConfiguration configuration)
        {
            var user = (User)HttpContext.User.Identity;
            return ForApplication(user.Application, configuration);
        }

        public JsonResult ForApplication(Application application, JqGridConfiguration configuration)
        {
            var groups = provider.GetForApplication(application);

            var jsonData = new
            {
                total = 1,
                page = configuration.PageIndex,
                records = groups.Count,
                rows = groups.Select(group => new
                {
                    id = group.ID,
                    cell = new object[]
                    {
                        group.Name,
                        group.Permissions.Select(p => p.Name),
                        group.IsActive ? "Yes" : "No",
                        group.ID.ToString()
                    }
                })
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult ForUser(User user, JqGridConfiguration configuration)
        {
            IList<Group> groups;
            IList<Group> userGroups = new List<Group>();
            if(user != null)
            {
                groups = provider.GetForApplication(user.Application);
                userGroups = user.Groups;
            }
            else
            {
                user = (User) HttpContext.User;
                groups = provider.GetForApplication(user.Application); 
            }

            var jsonData = new
            {
                total = 1,
                page = configuration.PageIndex,
                records = groups.Count,
                rows = groups.Select(group => new
                {
                    id = group.ID,
                    cell = new object[]
                    {
                           group.ID,
                           userGroups.Any(g => g.ID == group.ID) ? "true" : "false",
                           group.Name,
                           group.Permissions.Select(p => p.Name)
                    }
                })
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(Group item, List<Role> selectedRoles)
        {
            selectedRoles.ForEach(r => item.Roles.Add(r));
            if(item.Application == null)
            {
                var user = (User) HttpContext.User;
                item.Application = user.Application;
            }
            this.provider.Save(item);

            return Json(new { result = true });
        }
    }
}