using System;
using System.Web;
using System.Web.Mvc;
using Siege.Courier.Messages;
using Siege.Courier.Web.Responses;

namespace Siege.Courier.Web
{
    public class ServiceBusHandler : IHttpHandler
    {
        private readonly Func<IServiceBus> serviceBus;
        private readonly HandlerContext handlerContext;
        private readonly Func<string, Response> response;

        public ServiceBusHandler(Func<IServiceBus> serviceBus, HandlerContext handlerContext, Func<string, Response> response)
        {
            this.serviceBus = serviceBus;
            this.handlerContext = handlerContext;
            this.response = response;
        }

        public void ProcessRequest(HttpContext httpContext)
        {
            var modelBindingResult = handlerContext.ModelBinding.Using(new DefaultModelBinder()).BindAs<IMessage>();

            if (modelBindingResult.IsValid)
            {
                serviceBus().Publish(modelBindingResult.Output);
            }

            response(handlerContext.ResponseType).Execute(handlerContext.RequestContext);
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}