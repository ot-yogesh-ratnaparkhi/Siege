using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Admin.Security.Models;
using Siege.Security.Providers;
using Siege.Security.Web;

namespace Siege.Security.Admin.Security.Controllers
{
    public class UsersController : SecurityController<User, IUserProvider>
    {
        public UsersController(IUserProvider provider, IAuthenticationProvider authenticationProvider) : base(provider, authenticationProvider)
        {
        }

        public JsonResult ForApplicationAndConsumer(Application application, Consumer consumer, JqGridConfiguration configuration, IApplicationProvider applicationProvider)
        {
            var loggedInUser = (User)HttpContext.User;

            var users = provider.GetForApplicationAndConsumer(GetApplication(), GetConsumer(), loggedInUser.Can("CanAdministerAllSecurity"));
            
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

        //public JsonResult List(JqGridConfiguration configuration, IApplicationProvider applicationProvider)
        //{
        //    var user = (User) HttpContext.User.Identity;
        //    return ForApplicationAndConsumer(GetApplication(), GetConsumer(), configuration, applicationProvider);
        //}

        public ActionResult Save(UserModel model, List<Role> selectedRoles, List<Group> selectedGroups)
        {
            var user = model.IsNew ? new User() : this.provider.Find(model.UserID);

            user.Name = model.Name;
            user.IsActive = model.IsActive;

            user.Roles.Clear();
            user.Groups.Clear();

            selectedRoles.ForEach(r => user.Roles.Add(r));
            selectedGroups.ForEach(g => user.Groups.Add(g));

            if (model.IsNew)
            {
                var localUser = (User) HttpContext.User;
                //user.Application = localUser.Application;
                user.Password = model.Password;
            }

            this.provider.Save(user);

            return Json(new {result = true});
        }

        public ActionResult Edit(int? id)
        {
            var user = this.provider.Find(id);

            var model = new UserModel
            {
                UserID = user.ID,
                IsActive = user.IsActive,
                Name = user.Name,
                //ApplicationID = user.Application.ID
            };

            return View(model);
        }
    }
}