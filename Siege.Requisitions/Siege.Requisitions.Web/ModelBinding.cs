/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System;
using System.Web;
using System.Web.Mvc;

namespace Siege.Requisitions.Web
{
    public class ModelBinding<T>
    {
        public Func<T> Using<TModelBinder>(IServiceLocator serviceLocator) where TModelBinder : IModelBinder
        {
            return () => (T)serviceLocator.GetInstance<TModelBinder>().BindModel(new ControllerContext(), new ModelBindingContext
            {
                ValueProvider = new NameValueCollectionValueProvider(HttpContext.Current.Request.Form, null),
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(T))
            });
        }

        public Func<T> UsingDefaultBinder()
        {
            return () => (T)new DefaultModelBinder().BindModel(new ControllerContext(), new ModelBindingContext
            {
                ValueProvider = new NameValueCollectionValueProvider(HttpContext.Current.Request.Form, null),
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(T))
            });
        }
    }

    public class ModelBinding
    {
        public static ModelBinding<T> For<T>()
        {
            return new ModelBinding<T>();
        }
    }
}
