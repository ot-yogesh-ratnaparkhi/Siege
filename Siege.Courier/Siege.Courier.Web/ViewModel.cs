using System.Dynamic;
using System.Web.Mvc;

namespace Siege.Courier.Web
{
    public class ViewModel : DynamicObject
    {
        private readonly ViewDataDictionary viewData;

        public ViewModel(ViewDataDictionary viewData)
        {
            this.viewData = viewData;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if(!viewData.ContainsKey(binder.Name))
            {
                result = null;
                return true;
            }

            result = viewData[binder.Name];

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            viewData[binder.Name] = value;

            return true;
        }

        public void AddModelError(string key, string errorMessage)
        {
            this.viewData.ModelState.AddModelError(key, errorMessage);
        }
    }
}