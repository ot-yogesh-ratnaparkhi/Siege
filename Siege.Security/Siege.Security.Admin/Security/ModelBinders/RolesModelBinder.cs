using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Siege.Security.Providers;
using Siege.ServiceLocator;

namespace Siege.Security.Admin.Security.ModelBinders
{
    public class RolesBinder : DefaultModelBinder
    {
        private readonly IServiceLocator serviceLocator;

        public RolesBinder(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string selectedRoleIdsAsString = controllerContext.HttpContext.Request["SelectedRoles"] ?? String.Empty;
            string[] selectedRoleIds = selectedRoleIdsAsString.Split(',');

            var roleProvider = serviceLocator.GetInstance<IRoleProvider>();

            var selectedRoles = new List<Role>();
            foreach (string selectedRoleId in selectedRoleIds)
            {
                if (!String.IsNullOrEmpty(selectedRoleId))
                {
                    selectedRoles.Add(roleProvider.Find(int.Parse(selectedRoleId)));
                }
            }
            return selectedRoles;
        }
    }
}