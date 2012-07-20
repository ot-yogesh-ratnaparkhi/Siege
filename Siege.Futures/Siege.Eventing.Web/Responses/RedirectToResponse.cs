using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Siege.Eventing.Web.Responses
{
    public class RedirectToResponse<T> : Response
    {
        private readonly string actionName;
        private readonly string controllerName;

        public RedirectToResponse(Expression<Action<T>> destination)
        {
            actionName = ((MethodCallExpression) destination.Body).Method.Name;
            controllerName = typeof (T).Name;
        }

        public override void Execute(object model, ControllerContext context)
        {
            RouteValueDictionary dictionary = MergeRouteValues(actionName, controllerName,
                                                               context.RequestContext.RouteData == null ? null : context.RequestContext.RouteData.Values,
                                                               null, true);

            context.HttpContext = new HttpContextWrapper(HttpContext.Current);
            new RedirectToRouteResult(dictionary).ExecuteResult(context);
        }

        private static RouteValueDictionary MergeRouteValues(string actionName, string controllerName,
                                                             RouteValueDictionary implicitRouteValues,
                                                             RouteValueDictionary routeValues,
                                                             bool includeImplicitMvcValues)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            if (includeImplicitMvcValues)
            {
                object obj2;
                if ((implicitRouteValues != null) && implicitRouteValues.TryGetValue("action", out obj2))
                {
                    dictionary["action"] = obj2;
                }
                if ((implicitRouteValues != null) && implicitRouteValues.TryGetValue("controller", out obj2))
                {
                    dictionary["controller"] = obj2;
                }
            }
            if (routeValues != null)
            {
                foreach (KeyValuePair<string, object> pair in GetRouteValues(routeValues))
                {
                    dictionary[pair.Key] = pair.Value;
                }
            }
            if (actionName != null)
            {
                dictionary["action"] = actionName;
            }
            if (controllerName != null)
            {
                dictionary["controller"] = controllerName;
            }
            return dictionary;
        }

        private static IEnumerable<KeyValuePair<string, object>> GetRouteValues(RouteValueDictionary routeValues)
        {
            if (routeValues == null)
            {
                return new RouteValueDictionary();
            }
            return new RouteValueDictionary(routeValues);
        }
    }
}