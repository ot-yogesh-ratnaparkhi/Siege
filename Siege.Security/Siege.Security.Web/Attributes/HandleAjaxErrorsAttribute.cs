using System.Net.Mime;
using System.Web.Mvc;

namespace Siege.Security.Web.Attributes
{
    public class HandleAjaxErrorsAttribute : FilterAttribute, IExceptionFilter
    {
        public virtual void OnException(ExceptionContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                var response = filterContext.RequestContext.HttpContext.Response;
                response.ContentType = MediaTypeNames.Text.Plain;
                response.StatusCode = 500;
                response.Write(filterContext.Exception.Message);
                filterContext.ExceptionHandled = true;
            }
        }
    }
}