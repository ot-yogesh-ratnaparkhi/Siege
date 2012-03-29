using System;
using System.Web.Mvc;
using Siege.Security.Providers;
using Siege.ServiceLocator;

namespace Siege.Security.Admin.Security.ModelBinders
{
    public class GroupBinder : DefaultModelBinder
    {
        private readonly IServiceLocator serviceLocator;

        public GroupBinder(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var groupID = controllerContext.HttpContext.Request["GroupID"] ?? String.Empty;

            if (string.IsNullOrEmpty(groupID)) return null;

            var groupProvider = serviceLocator.GetInstance<IGroupProvider>();

            return groupProvider.Find(Convert.ToInt32(groupID));
        }
    }
}