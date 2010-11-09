using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Siege.Courier.Messages;
using Siege.Courier.Web.Responses;

namespace Siege.Courier.Web
{
    public class ServiceBusHandler : IHttpHandler
    {
        private readonly Func<IServiceBus> serviceBus;
        private readonly HandlerContext handlerContext;
        private readonly Func<string, Response> response;
        private readonly IMessageBucket bucket;
        private readonly DelegateManager manager;

        public ServiceBusHandler(Func<IServiceBus> serviceBus, HandlerContext handlerContext, Func<string, Response> response, IMessageBucket bucket, DelegateManager manager)
        {
            this.serviceBus = serviceBus;
            this.handlerContext = handlerContext;
            this.response = response;
            this.bucket = bucket;
            this.manager = manager;
        }

        public void ProcessRequest(HttpContext httpContext)
        {
            var modelBindingResult = handlerContext.ModelBinding.Using(new DefaultModelBinder()).BindAs<IMessage>();

            if (!modelBindingResult.Validate(message => manager.CreateDelegate(message, serviceBus).DynamicInvoke(message))) return;

            manager.CreateDelegate(modelBindingResult.Output, serviceBus).DynamicInvoke(modelBindingResult.Output);

            response(handlerContext.ResponseType).Execute(bucket.All(), GetControllerContext(handlerContext.RequestContext));
        }

        protected ControllerContext GetControllerContext(RequestContext requestContext)
        {
            HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
            return new ControllerContext(context, requestContext.RouteData, new DummyController());
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}