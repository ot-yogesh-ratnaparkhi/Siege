using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Siege.Eventing.Web.Responses;

namespace Siege.Eventing.Web
{
    public class MvcControllerHandler : IHttpHandler
    {
        private readonly RequestContext requestContext;
        private readonly ControllerContext controllerContext;

        public MvcControllerHandler(RequestContext requestContext, ControllerContext controllerContext)
        {
            this.requestContext = requestContext;
            this.controllerContext = controllerContext;
        }

        public void ProcessRequest(HttpContext context)
        {
            requestContext.RouteData.Values["controller"] = requestContext.RouteData.GetRequiredString("noun");
            requestContext.RouteData.Values["action"] = requestContext.RouteData.GetRequiredString("verb");

            try
            {
                ((IHttpHandler)new MvcHandler(requestContext)).ProcessRequest(context);
            }
            catch (InvalidOperationException)
            {   
                new ViewResponse().Execute(null, controllerContext);
            }
            catch(HttpException)
            {
                new ViewResponse().Execute(null, controllerContext);
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}