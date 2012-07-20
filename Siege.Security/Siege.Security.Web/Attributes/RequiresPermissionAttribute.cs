using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Principals;

namespace Siege.Security.Web.Attributes
{
    public class RequiresPermissionAttribute : ActionFilterAttribute
    {
        protected List<string> actions;

        protected RequiresPermissionAttribute() { }
        public RequiresPermissionAttribute(string action)
        {
            actions = new List<string> { action };
        }

        public RequiresPermissionAttribute(IEnumerable<string> actions)
        {
            this.actions = new List<string>(actions);
        }

        public RequiresPermissionAttribute(Type actionType)
        {
            actions = new List<string> { actionType.Name };
        }

        public RequiresPermissionAttribute(IEnumerable<Type> actionTypes)
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
            return actions.All(user.Can);
        }
    }
}
