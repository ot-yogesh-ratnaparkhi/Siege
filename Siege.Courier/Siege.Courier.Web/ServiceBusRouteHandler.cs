using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Siege.Courier.Web
{
    public class ServiceBusRouteHandler : MvcRouteHandler
    {
        private readonly Func<IHttpHandler> serviceBusHandler;

        public ServiceBusRouteHandler(Func<IHttpHandler> serviceBusHandler)
        {
            this.serviceBusHandler = serviceBusHandler;
        }

        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return serviceBusHandler();
        }
    }
}