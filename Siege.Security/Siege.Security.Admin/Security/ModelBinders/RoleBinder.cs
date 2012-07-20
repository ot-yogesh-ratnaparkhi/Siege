using System;
using System.Web.Mvc;
using Siege.Security.Providers;
using Siege.ServiceLocator;

namespace Siege.Security.Admin.Security.ModelBinders
{
    public class RoleBinder : DefaultModelBinder
    {
        private readonly IServiceLocator serviceLocator;

        public RoleBinder(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var roleID = controllerContext.HttpContext.Request["RoleID"] ?? String.Empty;

            if (string.IsNullOrEmpty(roleID)) return null;

            var provider = serviceLocator.GetInstance<IRoleProvider>();

            return provider.Find(Convert.ToInt32(roleID));
        }
    }
}