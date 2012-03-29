using System;
using System.Web.Mvc;
using Siege.Security.Providers;
using Siege.ServiceLocator;

namespace Siege.Security.Admin.Security.ModelBinders
{
    public class UserBinder : DefaultModelBinder
    {
        private readonly IServiceLocator serviceLocator;

        public UserBinder(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string userID = controllerContext.HttpContext.Request["UserID"] ?? String.Empty;

            if (string.IsNullOrEmpty(userID)) return null;

            var userProvider = serviceLocator.GetInstance<IUserProvider>();

            return userProvider.Find(Convert.ToInt32(userID));
        }
    }
}