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
using Siege.ServiceLocation.Stores;
using Siege.ServiceLocation.UseCases;
using Siege.ServiceLocation.UseCases.Managers;
using Siege.ServiceLocation.UseCases.PostResolution;
using Siege.ServiceLocation.UseCases.Conditional;
using Siege.ServiceLocation.UseCases.Default;

namespace Siege.ServiceLocation.Resolution
{
    public class Resolver : IResolver, ITypeResolver
    {
        protected readonly IInstanceResolver serviceLocator;
        protected readonly IServiceLocatorStore store;
        protected readonly Foundation foundation;

        public event TypeResolvedEventHandler TypeResolved;

        public Resolver(IInstanceResolver serviceLocator, IServiceLocatorStore store, Foundation foundation)
        {
            this.serviceLocator = serviceLocator;
            this.store = store;
            this.foundation = foundation;
            this.store.ExecutionStore.WireEvent(this);
        }

        public virtual object Resolve(Type type)
        {
            var conditionalManager = foundation.GetUseCaseManager<ConditionalUseCaseManager>();
            var defaultManager = foundation.GetUseCaseManager<DefaultUseCaseManager>();

            if (conditionalManager.Contains(type))
            {
                IList<IUseCase> conditionalCases = conditionalManager.GetUseCasesForType(type);

                for (int i = 0; i < conditionalCases.Count; i++)
                {
                    var useCase = conditionalCases[i];
                    object value = null;

                    if (useCase.IsValid(this.store))
                    {
                        value = Resolve(useCase, new ConditionalResolutionStrategy(serviceLocator, this.store));
                    }

                    if (value != null)
                    {
                        return value;
                    }
                }
            }

            if (defaultManager.Contains(type))
            {
                var useCase = defaultManager.GetUseCasesForType(type)[0];
                var value = Resolve(useCase, new DefaultResolutionStrategy(serviceLocator, this.store));

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

        private object Resolve(IUseCase useCase, IResolutionStrategy strategy)
        {
            var value = useCase.Resolve(strategy, this.store);

            if (value != null)
            {
                this.RaiseTypeResolvedEvent(useCase.GetBoundType());
                ExecutePostConditions<ConditionalPostResolutionUseCaseManager>(useCase, actionUseCase =>
                {
                    if (actionUseCase.IsValid(this.store))
                        value = actionUseCase.Resolve(new PostResolutionStrategy(value), this.store);
                }); 
                ExecutePostConditions<DefaultPostResolutionUseCaseManager>(useCase, actionUseCase => value = actionUseCase.Resolve(new PostResolutionStrategy(value), this.store));
            }

            return value;
        }

        private void ExecutePostConditions<TUseCaseManager>(IUseCase useCase, Action<IUseCase> action) where TUseCaseManager : IUseCaseManager
        {
            var manager = foundation.GetUseCaseManager<TUseCaseManager>();
            IList<IUseCase> actions = null;
            
            if(manager.Contains(useCase.GetBoundType()))
            {
                actions = manager.GetUseCasesForType(useCase.GetBoundType());
            }
            else if (manager.Contains(useCase.GetBaseBindingType()))
            {
                actions = manager.GetUseCasesForType(useCase.GetBaseBindingType());
            }

            if (actions != null)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    var actionUseCase = actions[i];
                    action(actionUseCase);
                }
            }
        }

        private void RaiseTypeResolvedEvent(Type type)
        {
            if (this.TypeResolved != null) this.TypeResolved(type);
        }
    }
}