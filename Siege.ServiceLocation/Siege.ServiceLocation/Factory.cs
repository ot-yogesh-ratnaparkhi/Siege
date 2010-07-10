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
using Siege.ServiceLocation.RegistrationTemplates.Default;
using Siege.ServiceLocation.EventHandlers;
using Siege.ServiceLocation.Exceptions;
using Siege.ServiceLocation.Registrations;
using Siege.ServiceLocation.Registrations.Containers;
using Siege.ServiceLocation.Resolution;

namespace Siege.ServiceLocation
{
    public class Factory : IGenericFactory, ITypeResolver
    {
        private readonly IContextualServiceLocator serviceLocator;
        private readonly Foundation foundation;
        private readonly List<IRegistration> conditionalRegistrations = new List<IRegistration>();
        private readonly List<IRegistration> defaultRegistrations = new List<IRegistration>();

        public event TypeResolvedEventHandler TypeResolved;

        public Factory(IContextualServiceLocator serviceLocator, Foundation foundation)
        {
            this.serviceLocator = serviceLocator;
            this.foundation = foundation;
            this.serviceLocator.Store.ExecutionStore.WireEvent(this);
        }

        public void AddCase(IRegistration registration)
        {
            if (registration.GetRegistrationTemplate() is DefaultRegistrationTemplate) defaultRegistrations.Add(registration);
            else conditionalRegistrations.Add(registration);
        }

        private void RaiseTypeResolvedEvent(Type type)
        {
            if (this.TypeResolved != null) this.TypeResolved(type);
        }

        public object Build(Type type)
        {
            for (int i = 0; i < conditionalRegistrations.Count; i++)
            {
                var registration = conditionalRegistrations[i];
                object result = null;

                if (registration.IsValid(serviceLocator.Store))
                {
                    result = Resolve(registration);
                }

                if (result != null)
                {
                    RaiseTypeResolvedEvent(registration.GetMappedFromType());
                    return result;
                }
            }

            for (int i = 0; i < defaultRegistrations.Count; i++)
            {
                var registration = defaultRegistrations[i];
                object result = Resolve(registration);

                if (result != null)
                {
                    RaiseTypeResolvedEvent(registration.GetMappedFromType());
                    return result;
                }
            }

            throw new RegistrationNotFoundException(type);
        }

        private object Resolve(IRegistration registration)
        {
            var value = registration.ResolveWith(this.serviceLocator, this.serviceLocator.Store);

            if (value != null)
            {
                this.RaiseTypeResolvedEvent(registration.GetMappedToType());
                ExecutePostConditions<DefaultPostResolutionRegistrationContainer>(registration, actionregistration => value = actionregistration.ResolveWith(new ValueResolver(value), this.serviceLocator.Store));
                ExecutePostConditions<ConditionalPostResolutionRegistrationContainer>(registration, actionregistration =>
                {
                    if (actionregistration.IsValid(this.serviceLocator.Store))
                        value = actionregistration.ResolveWith(new ValueResolver(value), this.serviceLocator.Store);
                });
            }

            return value;
        }

        private void ExecutePostConditions<TRegistrationManager>(IRegistration registration, Action<IRegistration> action) where TRegistrationManager : IRegistrationContainer
        {
            var manager = foundation.GetRegistrationContainer<TRegistrationManager>();
            IList<IRegistration> actions = manager.GetregistrationsForType(registration.GetMappedToType()) ??
                                      manager.GetregistrationsForType(registration.GetMappedFromType());

            if (actions != null)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    var actionregistration = actions[i];
                    action(actionregistration);
                }
            }
        }
    }
}