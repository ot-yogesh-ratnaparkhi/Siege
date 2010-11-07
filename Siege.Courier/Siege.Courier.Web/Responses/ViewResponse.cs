using System.Web.Mvc;
using System.Web.Routing;

namespace Siege.Courier.Web.Responses
{
    public class ViewResponse : Response
    {
        public override void Execute(object model, RequestContext requestContext)
        {
            new ViewResult().ExecuteResult(GetControllerContext(requestContext));
        }
    }
}