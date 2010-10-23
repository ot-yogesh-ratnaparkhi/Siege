using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Siege.Courier
{
    public class ServiceBusRouteHandler : MvcRouteHandler
    {
        private readonly Func<IServiceBus> serviceBus;

        public ServiceBusRouteHandler(Func<IServiceBus> serviceBus)
        {
            this.serviceBus = serviceBus;
        }

        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ServiceBusHandler(serviceBus, requestContext);
        }
    }
}