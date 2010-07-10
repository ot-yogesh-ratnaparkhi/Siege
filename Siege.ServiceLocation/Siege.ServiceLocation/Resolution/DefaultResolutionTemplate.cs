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
using Siege.ServiceLocation.EventHandlers;
using Siege.ServiceLocation.Exceptions;
using Siege.ServiceLocation.ExtensionMethods;
using Siege.ServiceLocation.InternalStorage;
using Siege.ServiceLocation.Registrations;
using Siege.ServiceLocation.Registrations.Containers;

namespace Siege.ServiceLocation.Resolution
{
    public class DefaultResolutionTemplate : IResolutionTemplate, ITypeResolver
    {
        protected readonly IInstanceResolver serviceLocator;
        protected readonly IServiceLocatorStore store;
        protected readonly Foundation foundation;

        public event TypeResolvedEventHandler TypeResolved;

        public DefaultResolutionTemplate(IInstanceResolver serviceLocator, IServiceLocatorStore store, Foundation foundation)
        {
            this.serviceLocator = serviceLocator;
            this.store = store;
            this.foundation = foundation;
            this.store.ExecutionStore.WireEvent(this);
        }

        public virtual object Resolve(Type type)
        {
            var conditionalManager = foundation.GetRegistrationContainer<ConditionalRegistrationContainer>();
            var defaultManager = foundation.GetRegistrationContainer<DefaultRegistrationContainer>();

            if (conditionalManager.Contains(type))
            {
                IList<IRegistration> conditionalCases = conditionalManager.GetregistrationsForType(type);

                for (int i = 0; i < conditionalCases.Count; i++)
                {
                    var registration = conditionalCases[i];
                    object value = null;

                    if (registration.IsValid(this.store))
                    {
                        value = Resolve(registration);
                    }

                    if (value != null)
                    {
                        return value;
                    }
                }
            }

            if (defaultManager.Contains(type))
            {
                var registration = defaultManager.GetregistrationsForType(type)[0];
                var value = Resolve(registration);

                if (value != null)
                {
                    return value;
                }
            }

            var instance = ResolveFromLocator(type);

            if(instance != null) return instance;

            throw new RegistrationNotFoundException(type);
        }

        protected virtual object ResolveFromLocator(Type type)
        {
            if (serviceLocator.HasTypeRegistered(type) || type.IsGenericType)
            {
                return serviceLocator.GetInstance(type, this.store.ResolutionStore.Items.OfType<ConstructorParameter, IResolutionArgument>());
            }

            return null;
        }

        private object Resolve(IRegistration registration)
        {
            var value = registration.ResolveWith(this.serviceLocator, this.store);

            if (value != null)
            {
                this.RaiseTypeResolvedEvent(registration.GetMappedToType());
                ExecutePostConditions<ConditionalPostResolutionRegistrationContainer>(registration, actionregistration =>
                {
                    if (actionregistration.IsValid(this.store))
                        value = actionregistration.ResolveWith(new ValueResolver(value), this.store);
                });
                ExecutePostConditions<DefaultPostResolutionRegistrationContainer>(registration, actionregistration => value = actionregistration.ResolveWith(new ValueResolver(value), this.store));
            }

            return value;
        }

        private void ExecutePostConditions<TRegistrationManager>(IRegistration registration, Action<IRegistration> action) where TRegistrationManager : IRegistrationContainer
        {
            var manager = foundation.GetRegistrationContainer<TRegistrationManager>();
            IList<IRegistration> actions = null;
            
            if(manager.Contains(registration.GetMappedToType()))
            {
                actions = manager.GetregistrationsForType(registration.GetMappedToType());
            }
            else if (manager.Contains(registration.GetMappedFromType()))
            {
                actions = manager.GetregistrationsForType(registration.GetMappedFromType());
            }

            if (actions != null)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    var actionregistration = actions[i];
                    action(actionregistration);
                }
            }
        }

        private void RaiseTypeResolvedEvent(Type type)
        {
            if (this.TypeResolved != null) this.TypeResolved(type);
        }
    }
}