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
using System.Collections.Generic;
using System.Web;
using Siege.ServiceLocator.EventHandlers;
using Siege.ServiceLocator.InternalStorage;

namespace Siege.ServiceLocator.Web
{
    public class HttpContextExecutionStore : IExecutionStore
    {
        private readonly IServiceLocatorStore store;

        protected HttpContextExecutionStore(IServiceLocatorStore store)
        {
            this.store = store;
            this.store.SetStore<IResolutionStore>(new HttpResolutionStore());
            HttpContext.Current.Items["executionCount"] = 0;
        }

        public int Index
        {
            get
            {
                if (!HttpContext.Current.Items.Contains("executionCount")) return 0;

                return (int)HttpContext.Current.Items["executionCount"];
            }
        }

        public static IExecutionStore New(IServiceLocatorStore store)
        {
            return new HttpContextExecutionStore(store);
        }

        public List<Type> RequestedTypes
        {
            get
            {
                List<Type> types = new List<Type>();
                foreach (object item in HttpContext.Current.Items.Values)
                {
                    if (item is Type) types.Add((Type)item);
                }

                return types;
            }
        }

        public void WireEvent(ITypeResolver typeResolver)
        {
            typeResolver.TypeResolved += OnTypeResolved;
        }

        public void WireEvent(ITypeRequester typeRequestor)
        {
            typeRequestor.TypeRequested += OnTypeRequested;
        }

        public void UnWireEvent(ITypeResolver typeResolver)
        {
            typeResolver.TypeResolved -= OnTypeResolved;
        }

        public void UnWireEvent(ITypeRequester typeRequestor)
        {
            typeRequestor.TypeRequested -= OnTypeRequested;
        }

        void OnTypeResolved(Type type)
        {
            Decrement();
        }

        void OnTypeRequested(Type type)
        {
            AddRequestedType(type);
        }

        public void AddRequestedType(Type type)
        {
            HttpContext.Current.Items.Add("ExecutionStore" + Guid.NewGuid(), type);
            Increment();
        }

        private void Increment()
        {
            HttpContext.Current.Items["executionCount"] = Index + 1;
        }

        private void Decrement()
        {
            HttpContext.Current.Items["executionCount"] = Index - 1;
            if (Index == 0)
            {
                store.SetStore<IResolutionStore>(new HttpResolutionStore());
            }
        }

        public void Dispose()
        {
            
        }
    }
}
