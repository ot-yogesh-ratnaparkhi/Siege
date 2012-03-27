using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Siege.Security.Providers;
using Siege.ServiceLocator;

namespace Siege.Security.Admin.Security.ModelBinders
{
    public class GroupsBinder : DefaultModelBinder
    {
        private readonly IServiceLocator serviceLocator;

        public GroupsBinder(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string selectedGroupIdsAsString = controllerContext.HttpContext.Request["SelectedGroups"] ?? String.Empty;
            string[] selectedGroupIds = selectedGroupIdsAsString.Split(',');

            var groupProvider = serviceLocator.GetInstance<IGroupProvider>();
            var selectedGroups = new List<Group>();

            foreach (string selectedGroupId in selectedGroupIds)
            {
                if (!String.IsNullOrEmpty(selectedGroupId))
                {
                    selectedGroups.Add(groupProvider.Find(int.Parse(selectedGroupId)));
                }
            }

            return selectedGroups;
        }
    }
}