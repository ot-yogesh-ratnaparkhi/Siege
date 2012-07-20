using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Principals;

namespace Siege.Security.Web.Attributes
{
    public class RequiresOneOfThePermissionsAttribute : ActionFilterAttribute
    {
        protected List<string> actions;

        protected RequiresOneOfThePermissionsAttribute() { }
        public RequiresOneOfThePermissionsAttribute(string action)
        {
            actions = new List<string> { action };
        }

        public RequiresOneOfThePermissionsAttribute(params string[] list)
        {
            actions = new List<string>();
            actions.AddRange(list);
        }

        public RequiresOneOfThePermissionsAttribute(IEnumerable<string> actions)
        {
            this.actions = new List<string>(actions);
        }

        public RequiresOneOfThePermissionsAttribute(Type actionType)
        {
            actions = new List<string> { actionType.Name };
        }

        public RequiresOneOfThePermissionsAttribute(IEnumerable<Type> actionTypes)
        {
            actions = new List<string>(actionTypes.Select(t => t.Name));
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.User as ISecurityPrincipal;
            if (user == null || !AccessAllowed(user))
            {
                filterContext.Result = new ContentResult
                {
                    Content = "You are not authorized to perform this action."
                };
            }
        }

        protected virtual bool AccessAllowed(ISecurityPrincipal user)
        {
            return actions.Any(user.Can);
        }
    }
}