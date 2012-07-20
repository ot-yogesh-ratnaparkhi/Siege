using System.Web;
using System.Web.Mvc;

namespace Siege.Eventing.Web.Responses
{
    public class RedirectResponse : Response
    {
        private readonly string url;

        public RedirectResponse(string url)
        {
            this.url = url;
        }

        public override void Execute(object model, ControllerContext context)
        {
            context.HttpContext = new HttpContextWrapper(HttpContext.Current);
            new RedirectResult(url).ExecuteResult(context);
        }
    }
}