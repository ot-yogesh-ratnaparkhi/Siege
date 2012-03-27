using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Siege.Security.Providers;
using Siege.ServiceLocator;

namespace Siege.Security.Admin.Security.ModelBinders
{
    public class PermissionsBinder : DefaultModelBinder
    {
        private readonly IServiceLocator serviceLocator;

        public PermissionsBinder(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string selectedPermissionIdsAsString = controllerContext.HttpContext.Request["SelectedPermissions"] ?? String.Empty;
            string[] selectedPermissionIds = selectedPermissionIdsAsString.Split(',');

            var permissionProvider = serviceLocator.GetInstance<IPermissionProvider>();

            var selectedPermissions = new List<Permission>();
            foreach (string selectedselectedPermissionId in selectedPermissionIds)
            {
                if (!String.IsNullOrEmpty(selectedselectedPermissionId))
                {
                    selectedPermissions.Add(permissionProvider.Find(int.Parse(selectedselectedPermissionId)));
                }
            }
            return selectedPermissions;
        }
    }
}