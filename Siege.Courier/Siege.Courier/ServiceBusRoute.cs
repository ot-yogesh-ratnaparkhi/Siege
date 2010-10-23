using System.Collections.Generic;
using System.Web.Routing;

namespace Siege.Courier
{
    public class ServiceBusRoute : Route
    {
        public ServiceBusRoute(ServiceBusRouteHandler routeHandler) : base("{noun}/{verb}/{id}", routeHandler)
        {
            this.Defaults = new RouteValueDictionary {{"id", ""}};
        }
    }
}