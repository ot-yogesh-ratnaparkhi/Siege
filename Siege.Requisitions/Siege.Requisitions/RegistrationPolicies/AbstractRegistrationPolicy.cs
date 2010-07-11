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
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.Registrations;
using Siege.Requisitions.Registrations.Named;
using Siege.Requisitions.RegistrationTemplates;

namespace Siege.Requisitions.RegistrationPolicies
{
    public abstract class AbstractRegistrationPolicy : IRegistrationPolicy
    {
        protected readonly IRegistration registration;

        protected AbstractRegistrationPolicy(IRegistration registration)
        {
            this.registration = registration;
        }

        public object GetMappedTo()
        {
            return registration.GetMappedTo();
        }

        public Type GetMappedToType()
        {
            return registration.GetMappedToType();
        }

        public IRegistrationTemplate GetRegistrationTemplate()
        {
            return registration.GetRegistrationTemplate();
        }

        public Type GetMappedFromType()
        {
            return registration.GetMappedFromType();
        }

        public abstract object ResolveWith(IInstanceResolver locator, IServiceLocatorStore context);

        public bool IsValid(IServiceLocatorStore context)
        {
            return registration.IsValid(context);
        }

        public string Key
        {
            get { return ((INamedRegistration)registration).Key; }
        }
    }
}