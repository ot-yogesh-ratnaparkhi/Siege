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
        private readonly Hashtable bindings = new Hashtable();
        private readonly Hashtable useCaseManagers = new Hashtable();
        private readonly Hashtable useCaseManagersByManagerType = new Hashtable();
        
        public Foundation(IServiceLocatorAdapter serviceLocator)
        {
            AddBinding(new DefaultUseCaseBinding(serviceLocator));
            AddBinding(new ConditionalUseCaseBinding(serviceLocator));
            AddBinding(new NamedUseCaseBinding(serviceLocator));
            AddBinding(new OpenGenericUseCaseBinding(serviceLocator));
            AddBinding(new ConditionalPostResolutionUseCaseBinding());
            AddBinding(new DefaultPostResolutionUseCaseBinding());

            AddUseCaseManager(typeof(ConditionalUseCaseBinding), new ConditionalUseCaseManager());
            AddUseCaseManager(typeof(OpenGenericUseCaseBinding), new ConditionalUseCaseManager());
            AddUseCaseManager(typeof(DefaultUseCaseBinding), new DefaultUseCaseManager());
            AddUseCaseManager(typeof(NamedUseCaseBinding), new ConditionalUseCaseManager());
            AddUseCaseManager(typeof(DefaultPostResolutionUseCaseBinding), new DefaultPostResolutionUseCaseManager());
            AddUseCaseManager(typeof(ConditionalPostResolutionUseCaseBinding), new ConditionalPostResolutionUseCaseManager());
            AddUseCaseManager(typeof(ConditionalRegistrationUseCaseBinding), new ConditionalRegistrationUseCaseManager());
            AddUseCaseManager(typeof(DefaultRegistrationUseCaseBinding), new DefaultRegistrationUseCaseManager());
            
        }

        private void AddBinding<TBinding>(TBinding instance) where TBinding : IUseCaseBinding
        {
            if (!bindings.ContainsKey(typeof(TBinding)))
            {
                lock (bindings.SyncRoot)
                {
                    if (!bindings.ContainsKey(typeof(TBinding)))
                    {
                        bindings.Add(typeof(TBinding), instance);
                    }
                }
            }
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

        public IUseCaseManager GetUseCaseManager(Type bindingType)
        {
            return (IUseCaseManager) useCaseManagers[bindingType];
        }

        public IUseCaseManager GetUseCaseManager<TUseCaseManager>() where TUseCaseManager : IUseCaseManager
        {
            return (TUseCaseManager)useCaseManagersByManagerType[typeof(TUseCaseManager)];
        }

        public IUseCaseBinding GetUseCaseBinding(Type useCaseBindingType)
        {
            return (IUseCaseBinding)this.bindings[useCaseBindingType];
        }

        public bool ContainsBinding(Type bindingType)
        {
            return bindings.ContainsKey(bindingType);
        }

        public bool ContainsUseCaseManager(Type useCaseManagerType)
        {
            return useCaseManagersByManagerType.ContainsKey(useCaseManagerType);
        }

        public bool ContainsUseCaseManagerForBinding(Type useCaseBindingType)
        {
            return useCaseManagers.ContainsKey(useCaseBindingType);
        }
    }
}