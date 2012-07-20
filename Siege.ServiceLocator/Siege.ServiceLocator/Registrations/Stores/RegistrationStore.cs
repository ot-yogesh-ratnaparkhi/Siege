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

namespace Siege.ServiceLocator.Registrations.Stores
{
    public class RegistrationStore : IRegistrationStore
    {
        private readonly Dictionary<Type, IList<IRegistration>> registrations = new Dictionary<Type, IList<IRegistration>>();
        private readonly List<IRegistration> addedRegistrations = new List<IRegistration>();

        public IList<IRegistration> GetRegistrationsForType(Type type)
        {
            if (!Contains(type)) return null;

            return this.registrations[type];
        }

        public virtual void Add(IRegistration registration)
        {
            Add(registration.GetMappedFromType(), registration);
        }

        public void Add(Type type, IRegistration registration)
        {
            if (!registrations.ContainsKey(type))
            {
                var list = new List<IRegistration>();
                registrations.Add(type, list);
            }

            var selectedRegistration = GetRegistrationsForType(type);
            if (selectedRegistration.Contains(registration)) return;

            Add(selectedRegistration, registration);

            this.registrations[type] = selectedRegistration;
            this.addedRegistrations.Add(registration);
        }

        protected virtual void Add(IList<IRegistration> registrations, IRegistration registration)
        {
            registrations.Add(registration);
        }

        public bool Contains(Type type)
        {
            return this.registrations.ContainsKey(type);
        }

        public bool Contains(IRegistration registration)
        {
            return addedRegistrations.Contains(registration);
        }

        public List<IRegistration> All()
        {
            var allRegistrations = new List<IRegistration>();

            foreach(var key in registrations.Keys)
            {
                allRegistrations.AddRange(this.registrations[key]);
            }

            return allRegistrations;
        }
    }
}