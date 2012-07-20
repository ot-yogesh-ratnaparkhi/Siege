using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Admin.Security.Models;
using Siege.Security.Principals;
using Siege.Security.Providers;
using Siege.Security.Web;

namespace Siege.Security.Admin.Security.Controllers
{
    public class UsersController : SecurityController<User, IUserProvider>
    {
        public UsersController(IUserProvider provider, IAuthenticationProvider authenticationProvider) : base(provider, authenticationProvider)
        {
        }

        public JsonResult ForConsumerAndApplication(Consumer consumer, Application application, JqGridConfiguration configuration)
        {
            var loggedInUser = (SecurityPrincipal)HttpContext.User;

            var users = provider.GetForApplicationAndConsumer(application, consumer, loggedInUser.Can("CanAdministerAllSecurity"));
            
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

        public JsonResult List(Application application, Consumer consumer, JqGridConfiguration configuration)
        {
            return ForConsumerAndApplication(consumer, application, configuration);
        }

        public ActionResult Save(UserModel model, Consumer consumer, List<Role> selectedRoles, List<Group> selectedGroups, List<Application> selectedApplications)
        {
            var user = model.IsNew ? new User() : this.provider.Find(model.UserID);

            user.Name = model.Name;
            user.IsActive = model.IsActive;

            user.Roles.Clear();
            user.Groups.Clear();
            user.Applications.Clear();

            selectedRoles.ForEach(r => user.Roles.Add(r));
            selectedGroups.ForEach(g => user.Groups.Add(g));
            selectedApplications.ForEach(a => user.Applications.Add(a));

            if (model.IsNew)
            {
                user.Password = model.Password;
                user.Consumer = consumer;
            }

            this.provider.Save(user);

            return Json(new {result = true});
        }

        public ActionResult Edit(User user)
        {
            var model = new UserModel
            {
                UserID = user.ID,
                IsActive = user.IsActive,
                Name = user.Name
            };

            return View(model);
        }
    }
}