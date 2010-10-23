using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Siege.Courier
{
    public class ServiceBusHandler : IHttpHandler
    {
        private readonly RequestContext requestContext;
        private readonly Func<IServiceBus> serviceBus;

        public ServiceBusHandler(Func<IServiceBus> serviceBus, RequestContext requestContext)
        {
            this.serviceBus = serviceBus;
            this.requestContext = requestContext;
        }

        public void ProcessRequest(HttpContext httpContext)
        {
            var method = httpContext.Request.HttpMethod;
            var format = httpContext.Request.QueryString["format"] ?? "view";
            var noun = this.requestContext.RouteData.GetRequiredString("noun");
            var type = GetTypeForName(noun);

            if(type != null)
            {
                if (ExecuteIfMatchingMethod(type, method)) return;
            }

            var verb = this.requestContext.RouteData.GetRequiredString("verb");

            type = GetTypeForName(verb+noun);
                
            if (type != null)
            {
                if (ExecuteIfMatchingMethod(type, method)) return;
            }

            requestContext.RouteData.Values["controller"] = noun;
            requestContext.RouteData.Values["action"] = verb;

            ((IHttpHandler)new MvcHandler(requestContext)).ProcessRequest(httpContext);
        }

        private bool ExecuteIfMatchingMethod(Type type, string method)
        {
            var attribute = type.GetCustomAttributes(typeof (HttpMethodAttribute), true).FirstOrDefault() as HttpMethodAttribute;

            if(attribute == null || (attribute != null && method.ToLower() == attribute.Method.ToLower()))
            {
                Execute(type);
                return true;
            }

            return false;
        }

        private void Execute(Type type)
        {
            var controllerContext = GetControllerContext();
            var valueProvider = GetValueProvider(controllerContext);
            ModelBindingContext modelBindingContext = ModelBindingContext(valueProvider, type);

            var message = (IMessage)new DefaultModelBinder().BindModel(
                controllerContext,
                modelBindingContext);

            if (modelBindingContext.ModelState.IsValid)
            {
                serviceBus().Publish(message);
                new ViewResult().ExecuteResult(controllerContext);
            }
        }

        private static ModelBindingContext ModelBindingContext(IValueProvider valueProvider, Type type)
        {
            return new ModelBindingContext
                       {
                           ValueProvider = valueProvider,
                           ModelMetadata =
                               ModelMetadataProviders.Current.
                               GetMetadataForType(null, type)
                       };
        }

        private static Type GetTypeForName(string type)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().Where(y => y.GetTypes().Any(x => x != typeof(IMessage) && (typeof(IMessage).IsAssignableFrom(x)))).FirstOrDefault();
            
            return types.GetTypes().Where(x => x.Name.ToLower() == type.ToLower()).FirstOrDefault();
        }

        private static IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            return new ValueProviderCollection
                       {
                           new FormValueProvider(controllerContext),
                           new RouteDataValueProvider(controllerContext),
                           new QueryStringValueProvider(controllerContext)
                       };
        }

        private static ControllerContext GetControllerContext()
        {
            HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
            var routeData = RouteTable.Routes.GetRouteData(context);
            return new ControllerContext(context, routeData, new DummyController());
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}