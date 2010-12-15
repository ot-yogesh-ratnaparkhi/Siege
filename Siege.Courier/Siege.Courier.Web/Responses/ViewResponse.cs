using System.Web;
using System.Web.Mvc;

namespace Siege.Courier.Web.Responses
{
    public class ViewResponse : Response
    {
        public override void Execute(object model, ControllerContext context)
        {
            context.HttpContext = new HttpContextWrapper(HttpContext.Current);
            var viewResult = new ViewResult {TempData = context.Controller.TempData, ViewData = context.Controller.ViewData};
            
            viewResult.ViewData.Model = null;
            
            viewResult.ExecuteResult(context);
        }
    }
}