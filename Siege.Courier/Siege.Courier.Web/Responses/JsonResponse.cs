using System.Web.Mvc;
using System.Web.Routing;

namespace Siege.Courier.Web.Responses
{
    public class JsonResponse : Response
    {
        public override void Execute(object model, RequestContext requestContext)
        {
            var result = new JsonResult {Data = model};

            result.ExecuteResult(GetControllerContext(requestContext));
        }
    }
}