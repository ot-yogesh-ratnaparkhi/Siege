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

using System.Collections.Generic;
using Siege.Requisitions.Registrations;
using Siege.Requisitions.Registrations.Stores;
using Siege.Requisitions.RegistrationTemplates;
using Siege.Requisitions.Resolution;

namespace Siege.Requisitions.Extensions.InjectionOverrides
{
    public class ConstructorRegistration : InstanceRegistration<object>
    {
        private readonly InjectionOverrideRegistrationStore injectionOverrideRegistrationStore = new InjectionOverrideRegistrationStore();
        private readonly InjectionOverrideRegistrationTemplate injectionOverrideRegistrationTemplate = new InjectionOverrideRegistrationTemplate();
        public List<IResolutionArgument> Arguments { get; set; }

        public override IRegistrationStore GetRegistrationStore()
        {
            return injectionOverrideRegistrationStore;
        }

        public override IRegistrationTemplate GetRegistrationTemplate()
        {
            return injectionOverrideRegistrationTemplate;
        }
    }
}