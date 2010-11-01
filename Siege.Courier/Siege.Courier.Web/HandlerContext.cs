using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Siege.Courier.Messages;

namespace Siege.Courier.Web
{
    public class HandlerContext
    {
        private readonly HttpContextBase httpContextBase;
        private readonly RequestContext requestContext;
        private readonly Func<TypeFinder> typeFinder;
        private TypeContext typeContext;

        public HandlerContext(HttpContextBase httpContextBase, RequestContext requestContext, Func<TypeFinder> typeFinder)
        {
            this.httpContextBase = httpContextBase;
            this.requestContext = requestContext;
            this.typeFinder = typeFinder;
        }

        public string ResponseType
        {
            get { return this.httpContextBase.Request.QueryString["response"] ?? "view"; }
        }

        public Type Type
        {
            get
            {
                if (typeContext != null) return typeContext.Type;

                typeContext = new TypeContext();
                var method = this.httpContextBase.Request.HttpMethod;
                var noun = this.RequestContext.RouteData.GetRequiredString("noun");

                var type = GetTypeForName(noun);

                if (type != null && TypeMatchesMethod(type, method))
                {
                    typeContext.Type = type;
                    return type;
                }

                var verb = this.RequestContext.RouteData.GetRequiredString("verb");

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
            get { return new ModelBinding(this.Type).WithHttpContext(this.httpContextBase); }
        }

        public RequestContext RequestContext
        {
            get { return requestContext; }
        }

        private Type GetTypeForName(string type)
        {
            return typeFinder().Named(type).WithPossibleSuffix("Message").Implementing<IMessage>().Find();
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