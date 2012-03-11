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
using Siege.ServiceLocator.Registrations;
using Siege.ServiceLocator.Registrations.ConditionalAwareness;
using Siege.ServiceLocator.Registrations.InjectionOverrides;
using Siege.ServiceLocator.Registrations.Stores;

namespace Siege.ServiceLocator
{
    public class Foundation
    {
        private readonly Dictionary<Type, IRegistrationStore> registrationContainers = new Dictionary<Type, IRegistrationStore>();
        private static readonly object lockObject = new object();
        
        public Foundation()
        {
            AddRegistrationStore<ConditionalRegistrationStore>();
            AddRegistrationStore<OpenGenericRegistrationStore>();
            AddRegistrationStore<DefaultRegistrationStore>();
            AddRegistrationStore<NamedRegistrationStore>();
            AddRegistrationStore<DefaultPostResolutionStore>();
            AddRegistrationStore<ConditionalPostResolutionStore>();
            AddRegistrationStore<InjectionOverrideRegistrationStore>();
            AddRegistrationStore<ContextualRegistrationStore>();
        }

        private void AddRegistrationStore<TRegistrationStore>() where TRegistrationStore : IRegistrationStore, new()
        {
            var templateType = typeof(TRegistrationStore);
            if (!registrationContainers.ContainsKey(templateType))
            {
                lock (lockObject)
                {
                    if (!registrationContainers.ContainsKey(templateType))
                    {
                        registrationContainers.Add(templateType, new TRegistrationStore());
                    }
                }
            }
        }

        public IRegistrationStore StoreFor(IRegistration registration)
        {
            return registrationContainers[registration.GetRegistrationStore().GetType()];
        }

        public TRegistrationStore StoreFor<TRegistrationStore>() where TRegistrationStore : IRegistrationStore
        {
            return (TRegistrationStore)registrationContainers[typeof(TRegistrationStore)];
        }

        public bool HasStoreFor<TRegistrationStore>() where TRegistrationStore : IRegistrationStore
        {
            return registrationContainers.ContainsKey(typeof(TRegistrationStore));
        }

        public bool HasStoreFor(IRegistration registration)
        {
            return registrationContainers.ContainsKey(registration.GetRegistrationStore().GetType());
        }

        public bool IsRegistered(IRegistration registration)
        {
            foreach (IRegistrationStore container in this.registrationContainers.Values)
            {
                if (container.Contains(registration)) return true;
            }

            return false;
        }
    }
}