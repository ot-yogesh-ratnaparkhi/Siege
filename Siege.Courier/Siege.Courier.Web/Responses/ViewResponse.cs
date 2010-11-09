using System.Web.Mvc;

namespace Siege.Courier.Web.Responses
{
    public class ViewResponse : Response
    {
        public ViewDataDictionary ViewData { get; set; }
        public TempDataDictionary TempData { get; set; }

        public override void Execute(object model, ControllerContext context)
        {
            var viewResult = new ViewResult {TempData = this.TempData, ViewData = this.ViewData};

            viewResult.ExecuteResult(context);
        }
    }
}