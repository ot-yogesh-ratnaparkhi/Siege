using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Admin.Security.Models;
using Siege.Security.Providers;
using Siege.Security.Web;

namespace Siege.Security.Admin.Security.Controllers
{
    public class GroupsController : SecurityController<Group, IGroupProvider>
    {
        public GroupsController(IGroupProvider provider, IAuthenticationProvider authenticationProvider) : base(provider, authenticationProvider)
        {
        }

        public JsonResult List(JqGridConfiguration configuration, IApplicationProvider applicationProvider)
        {
            return ForConsumerAndApplication(GetApplication(), GetConsumer(), configuration, applicationProvider);
        }

        public JsonResult ForConsumerAndApplication(Application application, Consumer consumer, JqGridConfiguration configuration, IApplicationProvider applicationProvider)
        {
            var loggedInUser = (User)HttpContext.User;

            var groups = provider.GetForApplicationAndConsumer(application, consumer, loggedInUser.Can("CanAdministerAllSecurity"));

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

        
        public JsonResult ForUser(int? id, JqGridConfiguration configuration, IUserProvider userProvider)
        {
            var loggedInUser = (User)HttpContext.User;
            IList<Group> groups;
            IList<Group> userGroups = new List<Group>();
            if (id != null)
            {
                var user = userProvider.Find(id);
                groups = provider.GetForApplicationAndConsumer(GetApplication(), GetConsumer(), loggedInUser.Can("CanAdministerAllSecurity"));
                userGroups = user.Groups;
            }
            else
            {
                groups = provider.GetForApplicationAndConsumer(GetApplication(), GetConsumer(), loggedInUser.Can("CanAdministerAllSecurity"));
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

        public ActionResult Save(GroupModel model, List<Role> selectedRoles)
        {
            var group = model.IsNew ? new Group() : this.provider.Find(model.GroupID);

            group.Name = model.Name;
            group.Description = model.Description;
            group.IsActive = model.IsActive;

            group.Roles.Clear();
            selectedRoles.ForEach(r => group.Roles.Add(r));

            if (model.IsNew)
            {
                var localUser = (User)HttpContext.User;
                group.Consumer = localUser.Consumer;
            }

            this.provider.Save(group);

            return Json(new { result = true });
        }

        public ActionResult Edit(int? id)
        {
            var group = this.provider.Find(id);

            var model = new GroupModel
            {
                GroupID = group.ID,
                IsActive = group.IsActive,
                Name = group.Name,
                ConsumerID = group.Consumer.ID,
                Description = group.Description
            };

            return View(model);
        }
    }
}