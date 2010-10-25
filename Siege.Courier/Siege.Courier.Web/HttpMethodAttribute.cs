using System;

namespace Siege.Courier.Web
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HttpMethodAttribute : Attribute
    {
        private readonly string method;

        public HttpMethodAttribute(string method)
        {
            this.method = method;
        }

        public string Method
        {
            get { return method; }
        }
    }
}