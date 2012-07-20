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
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.Registrations;
using Siege.ServiceLocator.Registrations.Named;
using Siege.ServiceLocator.Registrations.Stores;
using Siege.ServiceLocator.RegistrationTemplates;
using Siege.ServiceLocator.Resolution.Pipeline;

namespace Siege.ServiceLocator.RegistrationPolicies
{
    public abstract class AbstractRegistrationPolicy : IRegistrationPolicy
    {
        protected IRegistration registration;

        public void MapsTo(object implementationType)
        {
            this.registration.MapsTo(implementationType);
        }

        public object GetMappedTo()
        {
            return registration.GetMappedTo();
        }

        public Type GetMappedToType()
        {
            return registration.GetMappedToType();
        }

        public IRegistrationStore GetRegistrationStore()
        {
            return registration.GetRegistrationStore();
        }

        public IRegistrationTemplate GetRegistrationTemplate()
        {
            return registration.GetRegistrationTemplate();
        }

        public Type GetMappedFromType()
        {
            return registration.GetMappedFromType();
        }

        public abstract object ResolveWith(IInstanceResolver locator, IServiceLocatorStore context, PostResolutionPipeline pipeline);

        public bool IsValid(IInstanceResolver locator, IServiceLocatorStore context)
        {
            return registration.IsValid(locator, context);
        }

        public void Handle(IRegistration registration)
        {
            this.registration = registration;
        }

        public string Key
        {
            get
            {
                if(registration is INamedRegistration) return ((INamedRegistration)registration).Key;

                return null;
            }
        }

        public bool Equals(IRegistration other)
        {
            return this.registration.Equals(other);
        }
    }
}