using System.Web.Mvc;

namespace Siege.Courier.Web.Responses
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
            new RedirectResult(url).ExecuteResult(context);
        }
    }
}