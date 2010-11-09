using System.Web.Mvc;

namespace Siege.Courier.Web.Responses
{
    public class JsonResponse : Response
    {
        public override void Execute(object model, ControllerContext context)
        {
            var result = new JsonResult {Data = model};

            result.ExecuteResult(context);
        }
    }
}