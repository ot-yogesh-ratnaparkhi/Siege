using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Siege.Courier.Web
{
    public class MvcControllerHandler : IHttpHandler
    {
        private readonly RequestContext requestContext;

        public MvcControllerHandler(RequestContext requestContext)
        {
            this.requestContext = requestContext;
        }

        public void ProcessRequest(HttpContext context)
        {
            requestContext.RouteData.Values["controller"] = requestContext.RouteData.GetRequiredString("noun");
            requestContext.RouteData.Values["action"] = requestContext.RouteData.GetRequiredString("verb");

            ((IHttpHandler) new MvcHandler(requestContext)).ProcessRequest(context);
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}