using System.Dynamic;
using System.Web.Mvc;

namespace Siege.Courier.Web
{
    public class TempData : DynamicObject
    {
        private readonly TempDataDictionary dictionary;

        public TempData(TempDataDictionary dictionary)
        {
            this.dictionary = dictionary;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if(!dictionary.ContainsKey(binder.Name))
            {
                result = null;
                return true;
            }

            result = dictionary[binder.Name];

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            dictionary[binder.Name] = value;

            return true;
        }
    }
}