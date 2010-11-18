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
        private readonly Func<ControllerContext> controllerContext;

        public ServiceBusHandler(Func<IServiceBus> serviceBus, HandlerContext handlerContext, Func<string, Response> response, IMessageBucket bucket, DelegateManager manager, Func<ControllerContext> controllerContext)
        {
            this.serviceBus = serviceBus;
            this.handlerContext = handlerContext;
            this.response = response;
            this.bucket = bucket;
            this.manager = manager;
            this.controllerContext = controllerContext;
        }

        public void ProcessRequest(HttpContext httpContext)
        {
            var context = controllerContext();

            var modelBindingResult = handlerContext.ModelBinding.Using(new DefaultModelBinder()).BindAs<IMessage>();

            if (!modelBindingResult.Validate(message => manager.CreateDelegate(message, serviceBus).DynamicInvoke(message))) return;

            manager.CreateDelegate(modelBindingResult.Output, serviceBus).DynamicInvoke(modelBindingResult.Output);
            
            if(!context.Controller.TempData.ContainsKey("EventHandled") || !(bool)context.Controller.TempData["EventHandled"])
            {
                response(handlerContext.ResponseType).Execute(bucket.All(), context);
            }

            context.HttpContext.Response.Flush();
            context.HttpContext.Response.Close();
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}