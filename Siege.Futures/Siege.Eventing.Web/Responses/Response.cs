using System.Web.Mvc;

namespace Siege.Eventing.Web.Responses
{
    public abstract class Response
    {
        public abstract void Execute(object model, ControllerContext context);
    }
}