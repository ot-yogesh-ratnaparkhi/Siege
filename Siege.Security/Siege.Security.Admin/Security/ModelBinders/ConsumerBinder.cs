using System;
using System.Web.Mvc;
using Siege.Security.Providers;
using Siege.ServiceLocator;

namespace Siege.Security.Admin.Security.ModelBinders
{
    public class ConsumerBinder : DefaultModelBinder
    {
        private readonly IServiceLocator serviceLocator;

        public ConsumerBinder(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var consumerID = controllerContext.HttpContext.Request["ConsumerID"] ?? String.Empty;

            if (string.IsNullOrEmpty(consumerID)) return null;

            var provider = serviceLocator.GetInstance<IConsumerProvider>();

            return provider.Find(Convert.ToInt32(consumerID));
        }
    }
}