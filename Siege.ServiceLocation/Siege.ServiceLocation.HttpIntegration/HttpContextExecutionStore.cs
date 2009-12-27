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
using Siege.ServiceLocation.Stores;

namespace Siege.ServiceLocation.HttpIntegration
{
    public class HttpContextExecutionStore : IExecutionStore
    {
        protected HttpContextExecutionStore()
        {
            HttpContext.Current.Items["executionCount"] = 0;
        }
        
        private int Count
        {
            get
            {
                if (!HttpContext.Current.Items.Contains("executionCount")) return 0;

                return (int) HttpContext.Current.Items["executionCount"];
            }
        }

        public IExecutionStore Create()
        {
            if(Count == 0) return new HttpContextExecutionStore();

            return this;
        }

        public static IExecutionStore New()
        {
            return new HttpContextExecutionStore();
        }

        public List<Type> RequestedTypes
        {
            get
            {
                List<Type> types = new List<Type>();
                foreach(object item in HttpContext.Current.Items.Values)
                {
                    if (item is Type) types.Add((Type)item);
                }

                return types;
            }
        }

        public void AddRequestedType(Type type)
        {
            HttpContext.Current.Items.Add("ExecutionStore" + Guid.NewGuid(), type);
            Increment();
        }

        public void Increment()
        {
            HttpContext.Current.Items["executionCount"] = Count + 1;
        }

        public void Decrement()
        {
            HttpContext.Current.Items["executionCount"] = Count - 1;
        }
    }
}
