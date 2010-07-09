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
using Siege.ServiceLocation.Bindings;
using Siege.ServiceLocation.Bindings.Conditional;
using Siege.ServiceLocation.Bindings.Default;
using Siege.ServiceLocation.Bindings.Named;
using Siege.ServiceLocation.Bindings.OpenGenerics;
using Siege.ServiceLocation.Bindings.PostResolution;
using Siege.ServiceLocation.Bindings.Registration;
using Siege.ServiceLocation.UseCases.Managers;

namespace Siege.ServiceLocation
{
    public class Foundation
    {
        private readonly Hashtable useCaseManagers = new Hashtable();
        private readonly Hashtable useCaseManagersByManagerType = new Hashtable();
        
        public Foundation()
        {
            var conditionalManager = new ConditionalUseCaseManager();
            var defaultManager = new DefaultUseCaseManager();
            var namedManager = new ConditionalUseCaseManager();

            AddUseCaseManager(typeof(ConditionalUseCaseBinding), conditionalManager);
            AddUseCaseManager(typeof(ConditionalInstanceUseCaseBinding), conditionalManager);
            AddUseCaseManager(typeof(OpenGenericUseCaseBinding), new ConditionalUseCaseManager());
            AddUseCaseManager(typeof(DefaultUseCaseBinding), defaultManager);
            AddUseCaseManager(typeof(DefaultInstanceUseCaseBinding), defaultManager);
            AddUseCaseManager(typeof(NamedUseCaseBinding), namedManager);
            AddUseCaseManager(typeof(NamedInstanceUseCaseBinding), namedManager);
            AddUseCaseManager(typeof(DefaultPostResolutionUseCaseBinding), new DefaultPostResolutionUseCaseManager());
            AddUseCaseManager(typeof(ConditionalPostResolutionUseCaseBinding), new ConditionalPostResolutionUseCaseManager());
            AddUseCaseManager(typeof(ConditionalRegistrationUseCaseBinding), new ConditionalRegistrationUseCaseManager());
            AddUseCaseManager(typeof(DefaultRegistrationUseCaseBinding), new DefaultRegistrationUseCaseManager());
            
        }

        private void AddUseCaseManager<TUseCaseManager>(Type bindingType, TUseCaseManager instance) where TUseCaseManager : IUseCaseManager
        {
            if (!useCaseManagers.ContainsKey(bindingType))
            {
                lock (useCaseManagers.SyncRoot)
                {
                    if (!useCaseManagers.ContainsKey(bindingType))
                    {
                        useCaseManagers.Add(bindingType, instance);
                        if(!useCaseManagersByManagerType.ContainsKey(typeof(TUseCaseManager))) useCaseManagersByManagerType.Add(typeof(TUseCaseManager), instance);
                    }
                }
            }
        }

        public IUseCaseManager GetUseCaseManager(IUseCaseBinding useCaseBinding)
        {
            return (IUseCaseManager) useCaseManagers[useCaseBinding.GetType()];
        }

        public IUseCaseManager GetUseCaseManager<TUseCaseManager>() where TUseCaseManager : IUseCaseManager
        {
            return (TUseCaseManager)useCaseManagersByManagerType[typeof(TUseCaseManager)];
        }

        public bool ContainsUseCaseManagerForBinding(IUseCaseBinding useCaseBinding)
        {
            return useCaseManagers.ContainsKey(useCaseBinding.GetType());
        }
    }
}