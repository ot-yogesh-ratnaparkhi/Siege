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

namespace Siege.ServiceLocation.Stores
{
    public class ThreadedExecutionStore : IExecutionStore
    {
        [ThreadStatic] private static List<Type> requestedTypes;
        [ThreadStatic] private static int index;

        public List<Type> RequestedTypes
        {
            get { return requestedTypes; }
            private set { requestedTypes = value; }
        }

        private int Index
        {
            get { return index; }
            set { index = value; }
        }

        public void AddRequestedType(Type type)
        {
            RequestedTypes.Add(type);
            Increment();
        }

        public void Increment()
        {
            Index++;
        }

        public void Decrement()
        {
            Index--;
        }

		private ThreadedExecutionStore(IServiceLocatorStore store)
        {
            RequestedTypes = new List<Type>();
			Index = 0;
			store.ResolutionStore = new ThreadedResolutionStore();
        }

		public IExecutionStore Create(IServiceLocatorStore store)
        {
            if (Index == 0)
            {
                return new ThreadedExecutionStore(store);
            }

            return this;
        }

		public static IExecutionStore New(IServiceLocatorStore store)
        {
            return new ThreadedExecutionStore(store);
        }
    }
}