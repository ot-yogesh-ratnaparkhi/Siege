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

using Siege.Requisitions.InternalStorage;

namespace Siege.Requisitions.Web
{
    public class HttpServiceLocatorStore : IServiceLocatorStore
    {
        private readonly IContextStore contextStore;
		private IResolutionStore resolutionStore;
		private IExecutionStore executionStore;

        public HttpServiceLocatorStore(IContextStore store)
		{
			this.contextStore = store;
			this.resolutionStore = new HttpResolutionStore();
			this.executionStore = HttpContextExecutionStore.New(this);
		}

		public IContextStore ContextStore
		{
			get { return this.contextStore; }
		}

		public IResolutionStore ResolutionStore
		{
			get { return this.resolutionStore; }
			set { this.resolutionStore = value; }
		}
		
		public IExecutionStore ExecutionStore
		{
			get { return this.executionStore; }
			set { this.executionStore = value; }
		}

		public void Dispose()
		{
			this.contextStore.Clear();
			this.resolutionStore.Clear();
			this.executionStore = HttpContextExecutionStore.New(this);
		}
    }
}