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

using Siege.ServiceLocator.Registrations.PostResolution;
using Siege.ServiceLocator.Registrations.Stores;
using Siege.ServiceLocator.RegistrationTemplates;
using Siege.ServiceLocator.RegistrationTemplates.PostResolution;

namespace Siege.ServiceLocator.Extensions.Decorator
{
    public class ConditionalDecoratorRegistration<TService> : PostResolutionRegistration<TService>,
                                                              IDecoratorRegistration<TService>
    {
        private readonly ConditionalPostResolutionRegistrationTemplate conditionalPostResolutionRegistrationTemplate = new ConditionalPostResolutionRegistrationTemplate();
        private readonly ConditionalPostResolutionStore conditionalPostResolutionStore = new ConditionalPostResolutionStore();

        public override IRegistrationStore GetRegistrationStore()
        {
            return conditionalPostResolutionStore;
        }

        public override IRegistrationTemplate GetRegistrationTemplate()
        {
            return conditionalPostResolutionRegistrationTemplate;
        }
    }
}