using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Siege.Security.Providers;
using Siege.ServiceLocator;

namespace Siege.Security.Admin.Security.ModelBinders
{
    public class ApplicationsBinder : DefaultModelBinder
    {
        private readonly IServiceLocator serviceLocator;

        public ApplicationsBinder(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var selectedApplicationIdsAsString = controllerContext.HttpContext.Request["SelectedApplications"] ?? String.Empty;
            var selectedApplicationIds = selectedApplicationIdsAsString.Split(',');

            var applicationProvider = serviceLocator.GetInstance<IApplicationProvider>();
            var selectedApplications = new List<Application>();

            foreach (string selectedApplicationId in selectedApplicationIds)
            {
                if (!String.IsNullOrEmpty(selectedApplicationId))
                {
                    selectedApplications.Add(applicationProvider.Find(int.Parse(selectedApplicationId)));
                }
            }

            return selectedApplications;
        }
    }
}