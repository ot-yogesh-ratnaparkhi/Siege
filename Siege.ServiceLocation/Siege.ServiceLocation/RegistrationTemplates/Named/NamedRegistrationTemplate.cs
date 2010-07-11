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

using Siege.ServiceLocation.Registrations;
using Siege.ServiceLocation.Registrations.Named;
using Siege.ServiceLocation.Resolution;

namespace Siege.ServiceLocation.RegistrationTemplates.Named
{
    public class NamedRegistrationTemplate : IRegistrationTemplate
    {
        public virtual void Register(IServiceLocatorAdapter adapter, IRegistration registration, IResolutionTemplate template)
        {
            var namedRegistration = (INamedRegistration)registration;

            adapter.RegisterWithName(registration.GetMappedToType(), registration.GetMappedToType(), namedRegistration.Key);
            adapter.RegisterWithName(registration.GetMappedFromType(), registration.GetMappedToType(), namedRegistration.Key);
        }
    }
}