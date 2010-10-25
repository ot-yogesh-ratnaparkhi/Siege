using System;
using System.Web;
using System.Web.Routing;
using Siege.Requisitions;
using Siege.Requisitions.Extensions.Conventions;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;

namespace Siege.Courier.Web.Conventions
{
    public class AspNetMvcConvention : IConvention
    {
        public Action<IServiceLocator> Build()
        {
            return serviceLocator =>
                   serviceLocator
                       .Register(Given<HttpRequestBase>.ConstructWith(x => new HttpRequestWrapper(HttpContext.Current.Request)))
                       .Register(Given<RouteData>.ConstructWith(x => x.GetInstance<RouteCollection>().GetRouteData(x.GetInstance<HttpContextBase>())))
                       .Register(Given<RequestContext>.Then<RequestContext>())
                       .Register(Given<HttpContextBase>.ConstructWith(x => new HttpContextWrapper(HttpContext.Current)));
        }
    }
}