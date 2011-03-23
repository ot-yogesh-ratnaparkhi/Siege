using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Siege.Requisitions;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;

namespace Siege.Courier.Web.Conventions
{
    public class AspNetMvcConvention : Requisitions.Extensions.Conventions.IConvention
    {
        public Action<IServiceLocator> Build()
        {
            return serviceLocator =>
                   serviceLocator
                       .Register(Given<PerRequest>.Then<PerRequest>())
                       .Register<PerRequest>(Given<ViewDataDictionary>.ConstructWith(x => new ViewDataDictionary()))
                       .Register<PerRequest>(Given<TempDataDictionary>.ConstructWith(x => new TempDataDictionary()))
                       .Register(Given<DummyController>.Then<DummyController>())
                       .Register<PerRequest>(Given<TempData>.Then<TempData>())
                       .Register<PerRequest>(Given<ViewModel>.Then<ViewModel>())
                       .Register(Given<HttpRequestBase>.ConstructWith(x => new HttpRequestWrapper(HttpContext.Current.Request)))
                       .Register(Given<RouteData>.ConstructWith(x => x.GetInstance<RouteCollection>().GetRouteData(x.GetInstance<HttpContextBase>())))
                       .Register(Given<RequestContext>.Then<RequestContext>())
                       .Register(Given<RequestContext>.InitializeWith(ctx =>
                        {
                            ctx.RouteData.Values["controller"] = ctx.RouteData.GetRequiredString("noun");
                            ctx.RouteData.Values["action"] = ctx.RouteData.GetRequiredString("verb");
                        }))
                       .Register(Given<HttpContextBase>.ConstructWith(x => new HttpContextWrapper(HttpContext.Current)))
                       .Register(Given<ControllerContext>.ConstructWith(x =>
                        {
                             HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
                             return new ControllerContext(context, x.GetInstance<RequestContext>().RouteData, x.GetInstance<DummyController>());
                        }))
                       .Register(Given<DummyController>.InitializeWith(controller =>
                        {
                            controller.TempData = serviceLocator.GetInstance<TempData>().Data;
                            controller.ViewData = serviceLocator.GetInstance<ViewModel>().ViewData;
                        }));
        }
    }
}