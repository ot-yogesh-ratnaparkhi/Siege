using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Siege.Eventing.Messages;

namespace Siege.Eventing.Web
{
    public class ModelBinding
    {
        private readonly Type type;
        private HttpContextBase httpContext;
        private IModelBinder modelBinder;

        public ModelBinding(Type type)
        {
            this.type = type;
        }

        public ModelBinding WithHttpContext(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;
            return this;
        }

        public ModelBinding Using(IModelBinder binder)
        {
            this.modelBinder = new DefaultModelBinder();
            return this;
        }

        public ModelBindingResult<T> BindAs<T>() where T : IMessage 
        {
            var controllerContext = GetControllerContext();
            var valueProvider = GetValueProvider(controllerContext);
            var modelBindingContext = new ModelBindingContext
                                          {
                                              ValueProvider = valueProvider,
                                              ModelMetadata =
                                                  ModelMetadataProviders.Current.
                                                  GetMetadataForType(null, type)
                                          };

            var output = (T) this.modelBinder.BindModel(controllerContext, modelBindingContext);

            return new ModelBindingResult<T>(output, modelBindingContext);
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

        private ControllerContext GetControllerContext()
        {
            HttpContextBase context = this.httpContext ?? new HttpContextWrapper(HttpContext.Current);
            var routeData = RouteTable.Routes.GetRouteData(context);
            return new ControllerContext(context, routeData, new DummyController());
        }
    }

    public class ModelBindingResult<T> where T : IMessage
    {
        private readonly T output;
        private readonly ModelBindingContext modelBindingContext;

        public ModelBindingResult(T output, ModelBindingContext modelBindingContext)
        {
            this.output = output;
            this.modelBindingContext = modelBindingContext;
        }

        public T Output
        {
            get { return output; }
        }

        public bool Validate(Action<IMessage> onFailure)
        {
            if(!modelBindingContext.ModelState.IsValid)
            {
                onFailure((IMessage)Activator.CreateInstance(typeof(MessageValidationFailedMessage<>).MakeGenericType(output.GetType()), output));
                return false;
            }

            return true;
        }
    }
}