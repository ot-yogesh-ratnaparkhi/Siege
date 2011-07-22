using System.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace Siege.Eventing.Web
{
    public class ViewModel : DynamicObject
    {
        private readonly ViewDataDictionary viewData;

        public ViewDataDictionary ViewData { get { return viewData; } }

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
            HttpContext.Current.Items[this.GetType()] = this;

            return true;
        }

        public void AddModelError(string key, string errorMessage)
        {
            this.viewData.ModelState.AddModelError(key, errorMessage);

            HttpContext.Current.Items[this.GetType()] = this;
        }

        public void ClearModelState()
        {
            this.viewData.ModelState.Clear();

            HttpContext.Current.Items[this.GetType()] = this;
        }
    }
}