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
using System.Threading;

namespace Siege.ServiceLocation.Stores
{
    public class ThreadedExecutionStore : IExecutionStore
    {
        public List<Type> RequestedTypes
        {
            get
            {
                var slot = Thread.GetNamedDataSlot("executionContext");
                return (List<Type>) Thread.GetData(slot);
            }
            private set
            {
                var slot = Thread.GetNamedDataSlot("executionContext");

                Thread.SetData(slot, value);
            }
        }

        private int Index
        {
            get
            {
                var slot = Thread.GetNamedDataSlot("recursionCount");
                return (int) Thread.GetData(slot);
            }
            set
            {
                var slot = Thread.GetNamedDataSlot("recursionCount");

                Thread.SetData(slot, value);
            }
        }

        public void AddRequestedType(Type type)
        {
            List<Type> types = RequestedTypes;
            types.Add(type);
            RequestedTypes = types;
            Increment();
        }

        public void Increment()
        {
            int count = Index;
            count++;
            Index = count;
        }

        public void Decrement()
        {
            int count = Index;
            count--;
            Index = count;
        }

        private ThreadedExecutionStore()
        {
            Thread.FreeNamedDataSlot("executionContext");
            var slot = Thread.AllocateNamedDataSlot("executionContext");

            Thread.SetData(slot, new List<Type>());

            Thread.FreeNamedDataSlot("recursionCount");
            slot = Thread.AllocateNamedDataSlot("recursionCount");

            Thread.SetData(slot, 0);
        }

        public IExecutionStore Create()
        {
            if(Index == 0)
            {
                return new ThreadedExecutionStore();
            }
            
            return this;
        }

        public static IExecutionStore New()
        {
            return new ThreadedExecutionStore();
        }
    }
}