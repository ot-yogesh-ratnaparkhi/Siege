using System.Web.Mvc;

namespace Siege.Courier.Web.Responses
{
    public abstract class Response
    {
        public abstract void Execute(object model, ControllerContext context);
    }
}