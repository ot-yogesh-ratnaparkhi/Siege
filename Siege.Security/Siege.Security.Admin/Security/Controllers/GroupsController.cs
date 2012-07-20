using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Admin.Security.Models;
using Siege.Security.Principals;
using Siege.Security.Providers;
using Siege.Security.Web;

namespace Siege.Security.Admin.Security.Controllers
{
    public class GroupsController : SecurityController<Group, IGroupProvider>
    {
        public GroupsController(IGroupProvider provider, IAuthenticationProvider authenticationProvider) : base(provider, authenticationProvider)
        {
        }

        public JsonResult List(Application application, Consumer consumer, JqGridConfiguration configuration)
        {
            return ForConsumerAndApplication(application, consumer, configuration);
        }

        public JsonResult ForConsumerAndApplication(Application application, Consumer consumer, JqGridConfiguration configuration)
        {
            var loggedInUser = (SecurityPrincipal)HttpContext.User;

            var groups = provider.GetForConsumer(consumer, loggedInUser.Can("CanAdministerAllSecurity"));

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

        
        public JsonResult ForUser(User user, Consumer consumer, Application application, JqGridConfiguration configuration, IUserProvider userProvider, IApplicationProvider applicationProvider)
        {
            var loggedInUser = (SecurityPrincipal)HttpContext.User;

            IList<Group> groups;
            IList<Group> userGroups = new List<Group>();
            
            if (user != null)
            {
                groups = provider.GetForApplicationAndConsumer(application, consumer, loggedInUser.Can("CanAdministerAllSecurity"));
                userGroups = user.Groups;
            }
            else
            {
                groups = provider.GetForApplicationAndConsumer(application, consumer, loggedInUser.Can("CanAdministerAllSecurity"));
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

        public ActionResult Save(Consumer consumer, GroupModel model, List<Role> selectedRoles)
        {
            var group = model.IsNew ? new Group() : this.provider.Find(model.GroupID);

            group.Name = model.Name;
            group.Description = model.Description;
            group.IsActive = model.IsActive;

            group.Roles.Clear();
            selectedRoles.ForEach(r => group.Roles.Add(r));

            if (model.IsNew)
            {
                group.Consumer = consumer;
            }

            this.provider.Save(group);

            return Json(new { result = true });
        }

        public ActionResult Edit(Group group)
        {
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