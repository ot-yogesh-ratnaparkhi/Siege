//using System;
//using System.Web;
//using System.Web.Mvc;
//using Siege.Security.Entities;
//using Siege.Security.Providers;

//namespace Siege.Security.Admin.Security.ModelBinders
//{
//    public class AutoMappingBinder<T, TID, TProvider> : DefaultModelBinder 
//        where T : SecurityEntity<TID>
//        where TProvider : IProvider<T, TID>
//    {
//        private readonly TProvider provider;

//        public AutoMappingBinder(TProvider provider)
//        {
//            this.provider = provider;
//        }

//        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
//        {
//            var model = (T)base.BindModel(controllerContext, bindingContext);

//            var result = Bind(controllerContext, bindingContext, model);

//            if (HttpContext.Current.Request.HttpMethod == "POST") result = Mapper.Map(model, result);

//            return result;
//        }

//        public T Bind(ControllerContext controllerContext, ModelBindingContext bindingContext, object item)
//        {
//            var model = (T)item;
//            T result = null;

//            if (model == null || model.ID == null)
//            {
//                var valueResult = bindingContext.ValueProvider.GetValue("id");
//                if (valueResult != null && !String.IsNullOrEmpty(valueResult.AttemptedValue))
//                {
//                    var id = (TID)bindingContext.ValueProvider.GetValue("id").ConvertTo(typeof(TID));
//                    result = provider.Find(id);
//                }
//            }
//            else
//            {
//                result = provider.Find(model.ID);
//            }

//            return result;
//        }
//    }
//}