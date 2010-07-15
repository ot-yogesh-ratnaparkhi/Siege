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

namespace Siege.Requisitions.Registrations.Containers
{
    public class DefaultRegistrationList : IRegistrationContainer
    {
        private readonly Dictionary<Type, IRegistration> internalList = new Dictionary<Type, IRegistration>();

        public void Add(IRegistration registration)
        {
            var type = registration.GetMappedFromType();
            if (!Contains(type))
                internalList.Add(type, registration);
        }

        public bool Contains(Type type)
        {
            return this.internalList.ContainsKey(type);
        }

        public List<IRegistration> GetRegistrationsForType(Type type)
        {
            if (!Contains(type)) return null;

            return new List<IRegistration> {this.internalList[type]};
        }

        public List<IRegistration> All()
        {
            var allRegistrations = new List<IRegistration>();

            foreach(Type key in this.internalList.Keys)
            {
                allRegistrations.Add(this.internalList[key]);
            }

            return allRegistrations;
        }
    }
}