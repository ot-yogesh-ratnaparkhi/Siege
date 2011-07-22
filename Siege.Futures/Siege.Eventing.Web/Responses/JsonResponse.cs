using System.Web;
using System.Web.Mvc;

namespace Siege.Eventing.Web.Responses
{
    public class JsonResponse : Response
    {
        public override void Execute(object model, ControllerContext context)
        {
            context.HttpContext = new HttpContextWrapper(HttpContext.Current);
            var result = new JsonResult {Data = model};

            result.ExecuteResult(context);
        }
    }
}