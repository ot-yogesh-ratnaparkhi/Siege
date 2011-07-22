/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions andctory
     limitations under the License.
*/

using Siege.ServiceLocator.Registrations;
using Siege.ServiceLocator.Registrations.Stores;
using Siege.ServiceLocator.RegistrationTemplates;
using Siege.ServiceLocator.RegistrationTemplates.Conditional;

namespace Siege.ServiceLocator.Extensions.ConditionalInjection
{
    public class InjectionInstanceRegistration<TService> : InstanceRegistration<TService>
    {
        private readonly ConditionalInstanceRegistrationTemplate conditionalInstanceRegistrationTemplate = new ConditionalInstanceRegistrationTemplate();
        private readonly ConditionalRegistrationStore conditionalRegistrationStore = new ConditionalRegistrationStore();

        public override IRegistrationStore GetRegistrationStore()
        {
            return conditionalRegistrationStore;
        }

        public override IRegistrationTemplate GetRegistrationTemplate()
        {
            return conditionalInstanceRegistrationTemplate;
        }
    }
}