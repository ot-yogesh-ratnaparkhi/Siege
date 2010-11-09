using System.Collections.Specialized;
using System.Dynamic;

namespace Siege.Courier.Web
{
    public class QueryString : DynamicObject
    {
        private readonly NameValueCollection queryString;

        public QueryString(NameValueCollection queryString)
        {
            this.queryString = queryString;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string value = queryString[binder.Name];
            if(value == null)
            {
                result = null;
                return true;
            }
            result = value;
            return true;
        }
    }
}