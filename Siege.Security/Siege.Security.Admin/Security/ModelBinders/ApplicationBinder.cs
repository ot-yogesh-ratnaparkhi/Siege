using System;
using System.Web.Mvc;
using Siege.Security.Providers;
using Siege.ServiceLocator;

namespace Siege.Security.Admin.Security.ModelBinders
{
    public class ApplicationBinder : DefaultModelBinder
    {
        private readonly IServiceLocator serviceLocator;

        public ApplicationBinder(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var applicationID = controllerContext.HttpContext.Request["ApplicationID"] ?? String.Empty;

            if (string.IsNullOrEmpty(applicationID)) return null;

            var provider = serviceLocator.GetInstance<IApplicationProvider>();

            return provider.Find(Convert.ToInt32(applicationID));
        }
    }
}