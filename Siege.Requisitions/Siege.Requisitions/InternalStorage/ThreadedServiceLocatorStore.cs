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

namespace Siege.Requisitions.InternalStorage
{
    public class ThreadedServiceLocatorStore : IServiceLocatorStore
    {
        private readonly IContextStore store;
        private IResolutionStore resolutionStore;
        private readonly IExecutionStore executionStore;

        public ThreadedServiceLocatorStore()
            : this(new ThreadLocalStore())
        {
        }

        public ThreadedServiceLocatorStore(IContextStore store)
        {
            this.store = store;
            this.resolutionStore = new ThreadedResolutionStore();
            this.executionStore = ThreadedExecutionStore.New(this);
        }

        public IContextStore ContextStore
        {
            get { return this.store; }
        }

        public IResolutionStore ResolutionStore
        {
            get { return this.resolutionStore; }
            set { this.resolutionStore = value; }
        }

        public IExecutionStore ExecutionStore { get { return this.executionStore; } }

        public void Dispose()
        {
            this.store.Dispose();
            this.resolutionStore.Dispose();
        }
    }
}