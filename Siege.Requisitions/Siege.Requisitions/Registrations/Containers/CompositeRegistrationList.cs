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
using System.Collections;
using System.Collections.Generic;

namespace Siege.Requisitions.Registrations.Containers
{
    public class CompositeRegistrationList : IRegistrationContainer
    {
        private readonly Dictionary<Type, List<IRegistration>> registrations = new Dictionary<Type, List<IRegistration>>();

        public List<IRegistration> GetRegistrationsForType(Type type)
        {
            if (!Contains(type)) return null;

            return this.registrations[type];
        }

        public void Add(IRegistration registration)
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

            selectedRegistration.Add(registration);

            this.registrations[type] = selectedRegistration;
        }

        public bool Contains(Type type)
        {
            return this.registrations.ContainsKey(type);
        }

        public List<IRegistration> All()
        {
            var allRegistrations = new List<IRegistration>();

            foreach(Type key in registrations.Keys)
            {
                allRegistrations.AddRange(this.registrations[key]);
            }

            return allRegistrations;
        }
    }
}