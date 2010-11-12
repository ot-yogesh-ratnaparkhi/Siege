using System.Web.Mvc;

namespace Siege.Courier.Web.Responses
{
    public class ViewResponse : Response
    {
        public override void Execute(object model, ControllerContext context)
        {
            var viewResult = new ViewResult {TempData = context.Controller.TempData, ViewData = context.Controller.ViewData};

            viewResult.ExecuteResult(context);
        }
    }
}