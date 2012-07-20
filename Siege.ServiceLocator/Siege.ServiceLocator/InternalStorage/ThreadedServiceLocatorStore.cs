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
using System.Linq;
using Siege.ServiceLocator.Registrations.ConditionalAwareness;
using Siege.ServiceLocator.Registrations.InjectionOverrides;

namespace Siege.ServiceLocator.InternalStorage
{
    public class ThreadedServiceLocatorStore : IServiceLocatorStore
    {
        private readonly Dictionary<Type, IStore> stores = new Dictionary<Type, IStore>();

        public ThreadedServiceLocatorStore()
            : this(new ThreadLocalStore())
        {
        }

        public ThreadedServiceLocatorStore(IContextStore store)
        {
            AddStore<IContextStore>(store);
            AddStore<IResolutionStore>(new ThreadedResolutionStore());
            AddStore<IExecutionStore>(ThreadedExecutionStore.New(this));
            AddStore<IAwarenessStore>(new AwarenessStore());
            AddStore<IInjectionOverrideStore>(new InjectionOverrideStore());
        }

        public void SetStore<TStoreType>(IStore store) where TStoreType : IStore
        {
            this.stores[typeof (TStoreType)] = store;
        }

        public TStoreType Get<TStoreType>() where TStoreType : IStore
        {
            return (TStoreType)this.stores[typeof (TStoreType)];
        }

        public List<TStoreType> All<TStoreType>() where TStoreType : IStore
        {
            return this.stores.Values.ToList().Where(x => typeof(TStoreType).IsInstanceOfType(x)).Cast<TStoreType>().ToList();
        }

        public void AddStore<TStoreType>(IStore store) where TStoreType : IStore
        {
            if(!this.stores.ContainsKey(typeof(TStoreType))) this.stores.Add(typeof(TStoreType), store);
        }

        public void Dispose()
        {
            foreach(IStore store in this.stores.Values) store.Dispose();
        }
    }
}