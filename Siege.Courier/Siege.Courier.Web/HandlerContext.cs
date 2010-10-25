using System;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Siege.Courier.Web
{
    public class HandlerContext
    {
        private readonly HttpRequestBase request;
        private readonly RequestContext requestContext;
        private readonly Func<TypeFinder> typeFinder;
        private TypeContext typeContext;

        public HandlerContext(HttpRequestBase request, RequestContext requestContext, Func<TypeFinder> typeFinder)
        {
            this.request = request;
            this.requestContext = requestContext;
            this.typeFinder = typeFinder;
        }

        public Type Type
        {
            get
            {
                if (typeContext != null) return typeContext.Type;

                typeContext = new TypeContext();
                var method = this.request.HttpMethod;
                var noun = this.requestContext.RouteData.GetRequiredString("noun");

                var type = GetTypeForName(noun);

                if (type != null && TypeMatchesMethod(type, method))
                {
                    typeContext.Type = type;
                    return type;
                }

                var verb = this.requestContext.RouteData.GetRequiredString("verb");

                type = GetTypeForName(verb + noun);

                if (type != null && TypeMatchesMethod(type, method))
                {
                    typeContext.Type = type;
                    return type;
                }

                return null;
            }
        }

        public ModelBinding ModelBinding
        {
            get { return new ModelBinding(this.Type); }
        }

        private Type GetTypeForName(string type)
        {
            return typeFinder().Named(type).Implementing<IMessage>().Find();
        }

        private static bool TypeMatchesMethod(Type type, string method)
        {
            var attribute = type.GetCustomAttributes(typeof (HttpMethodAttribute), true).FirstOrDefault() as HttpMethodAttribute;

            if (attribute == null || method.ToLower() == attribute.Method.ToLower()) return true;

            return false;
        }

        private class TypeContext
        {
            public Type Type { get; set; }
        }
    }
}