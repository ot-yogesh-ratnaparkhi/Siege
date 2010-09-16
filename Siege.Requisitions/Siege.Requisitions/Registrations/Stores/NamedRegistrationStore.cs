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
using Siege.Requisitions.Registrations.Named;

namespace Siege.Requisitions.Registrations.Stores
{
    public class NamedRegistrationStore : RegistrationStore
    {
        private Dictionary<string, Dictionary<Type, INamedRegistration>> namedRegistrations = new Dictionary<string, Dictionary<Type, INamedRegistration>>();

        public override void Add(IRegistration registration)
        {
            var namedRegistration = registration as INamedRegistration;
            var registrationMap = new Dictionary<Type, INamedRegistration>();
            
            if(namedRegistrations.ContainsKey(namedRegistration.Key))
            {
                registrationMap = namedRegistrations[namedRegistration.Key];
            }

            if(!registrationMap.ContainsKey(namedRegistration.GetMappedFromType())) registrationMap.Add(namedRegistration.GetMappedFromType(), namedRegistration);

            namedRegistrations[namedRegistration.Key] = registrationMap;

            base.Add(registration);
        }

        public bool HasRegistrationsForTypeNamed(Type type, string name)
        {
            if (!namedRegistrations.ContainsKey(name)) return false;
            if (!namedRegistrations[name].ContainsKey(type)) return false;

            return true;
        }

        public INamedRegistration GetRegistrationForTypeAndName(Type type, string key)
        {
            return namedRegistrations[key][type];
        }
    }
}