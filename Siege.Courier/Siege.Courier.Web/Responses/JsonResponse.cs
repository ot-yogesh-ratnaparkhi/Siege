using System.Web.Mvc;
using System.Web.Routing;

namespace Siege.Courier.Web.Responses
{
    public class JsonResponse : Response
    {
        private readonly object data;

        public JsonResponse(object data)
        {
            this.data = data;
        }

        public override void Execute(RequestContext requestContext)
        {
            var result = new JsonResult {Data = data};

            result.ExecuteResult(GetControllerContext(requestContext));
        }
    }
}