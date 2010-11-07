using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Siege.Courier.Web.Responses
{
    public abstract class Response
    {
        public abstract void Execute(object model, RequestContext requestContext);

        protected ControllerContext GetControllerContext(RequestContext requestContext)
        {
            HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
            return new ControllerContext(context, requestContext.RouteData, new DummyController());
        }
    }
}