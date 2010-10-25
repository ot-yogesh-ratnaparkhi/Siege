using System;
using System.Web;
using System.Web.Mvc;

namespace Siege.Courier.Web
{
    public class ServiceBusHandler : IHttpHandler
    {
        private readonly Func<IServiceBus> serviceBus;
        private readonly HandlerContext handlerContext;

        public ServiceBusHandler(Func<IServiceBus> serviceBus, HandlerContext handlerContext)
        {
            this.serviceBus = serviceBus;
            this.handlerContext = handlerContext;
        }

        public void ProcessRequest(HttpContext httpContext)
        {
            var modelBindingResult =
                handlerContext.ModelBinding.WithHttpContext(new HttpContextWrapper(httpContext)).Using(
                    new DefaultModelBinder()).BindAs<IMessage>();

            if (modelBindingResult.IsValid)
            {
                serviceBus().Publish(modelBindingResult.Output);
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}