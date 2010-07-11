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
using Siege.Requisitions.Registrations.Containers;
using Siege.Requisitions.RegistrationTemplates;
using Siege.Requisitions.RegistrationTemplates.Conditional;
using Siege.Requisitions.RegistrationTemplates.Default;
using Siege.Requisitions.RegistrationTemplates.Named;
using Siege.Requisitions.RegistrationTemplates.OpenGenerics;
using Siege.Requisitions.RegistrationTemplates.PostResolution;

namespace Siege.Requisitions
{
    public class Foundation
    {
        private readonly Hashtable registrationContainers = new Hashtable();
        private readonly Hashtable registrationContainersByType = new Hashtable();
        private readonly IRegistrationContainer fallbackContainer;
        
        public Foundation()
        {
            var conditionalManager = new ConditionalRegistrationContainer();
            var defaultManager = new DefaultRegistrationContainer();
            var namedManager = new ConditionalRegistrationContainer();
            this.fallbackContainer = conditionalManager;

            AddRegistrationContainer(typeof(ConditionalRegistrationTemplate), conditionalManager);
            AddRegistrationContainer(typeof(ConditionalInstanceRegistrationTemplate), conditionalManager);
            AddRegistrationContainer(typeof(OpenGenericRegistrationTemplate), new ConditionalRegistrationContainer());
            AddRegistrationContainer(typeof(DefaultRegistrationTemplate), defaultManager);
            AddRegistrationContainer(typeof(DefaultInstanceRegistrationTemplate), defaultManager);
            AddRegistrationContainer(typeof(NamedRegistrationTemplate), namedManager);
            AddRegistrationContainer(typeof(NamedInstanceRegistrationTemplate), namedManager);
            AddRegistrationContainer(typeof(DefaultPostResolutionRegistrationTemplate), new DefaultPostResolutionRegistrationContainer());
            AddRegistrationContainer(typeof(ConditionalPostResolutionRegistrationTemplate), new ConditionalPostResolutionRegistrationContainer());
            AddRegistrationContainer(typeof(ConditionalRegistrationTemplate), new ConditionalPreResolutionRegistrationContainer());
            AddRegistrationContainer(typeof(DefaultRegistrationTemplate), new DefaultPreResolutionRegistrationContainer());
            
        }

        private void AddRegistrationContainer<TRegistrationContainer>(Type templateType, TRegistrationContainer instance) where TRegistrationContainer : IRegistrationContainer
        {
            if (!registrationContainers.ContainsKey(templateType))
            {
                lock (registrationContainers.SyncRoot)
                {
                    if (!registrationContainers.ContainsKey(templateType))
                    {
                        registrationContainers.Add(templateType, instance);
                        if(!registrationContainersByType.ContainsKey(typeof(TRegistrationContainer))) registrationContainersByType.Add(typeof(TRegistrationContainer), instance);
                    }
                }
            }
        }

        public IRegistrationContainer GetRegistrationContainer(IRegistrationTemplate registrationTemplate)
        {
            if(!ContainsRegistrationContainerForTemplate(registrationTemplate)) return this.fallbackContainer;

            return (IRegistrationContainer) registrationContainers[registrationTemplate.GetType()];
        }

        public IRegistrationContainer GetRegistrationContainer<TRegistrationManager>() where TRegistrationManager : IRegistrationContainer
        {
            return (TRegistrationManager)registrationContainersByType[typeof(TRegistrationManager)];
        }

        public bool ContainsRegistrationContainerForTemplate(IRegistrationTemplate registrationTemplate)
        {
            return registrationContainers.ContainsKey(registrationTemplate.GetType());
        }
    }
}