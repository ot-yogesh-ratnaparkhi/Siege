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
using Siege.ServiceLocation.Bindings.Default;
using Siege.ServiceLocation.EventHandlers;
using Siege.ServiceLocation.Exceptions;
using Siege.ServiceLocation.UseCases;
using Siege.ServiceLocation.UseCases.Conditional;
using Siege.ServiceLocation.UseCases.Default;
using Siege.ServiceLocation.UseCases.Managers;
using Siege.ServiceLocation.UseCases.PostResolution;

namespace Siege.ServiceLocation
{
    public class Factory : IGenericFactory, ITypeResolver
    {
        private readonly IContextualServiceLocator serviceLocator;
        private readonly Foundation foundation;
        private readonly List<IUseCase> conditionalUseCases = new List<IUseCase>();
        private readonly List<IUseCase> defaultCases = new List<IUseCase>();

        public event TypeResolvedEventHandler TypeResolved;

        public Factory(IContextualServiceLocator serviceLocator, Foundation foundation)
        {
            this.serviceLocator = serviceLocator;
            this.foundation = foundation;
            this.serviceLocator.Store.ExecutionStore.WireEvent(this);
        }

        public void AddCase(IUseCase useCase)
        {
            if (useCase.GetUseCaseBinding() is DefaultUseCaseBinding) defaultCases.Add(useCase);
            else conditionalUseCases.Add(useCase);
        }

        private void RaiseTypeResolvedEvent(Type type)
        {
            if (this.TypeResolved != null) this.TypeResolved(type);
        }

        public object Build(Type type)
        {
            for (int i = 0; i < conditionalUseCases.Count; i++)
            {
                var useCase = conditionalUseCases[i];
                object result = null;

                if (useCase.IsValid(serviceLocator.Store))
                {
                    result = Resolve(useCase, new ConditionalResolutionStrategy(serviceLocator, serviceLocator.Store));
                }

                if (result != null)
                {
                    RaiseTypeResolvedEvent(useCase.GetBaseBindingType());
                    return result;
                }
            }

            for (int i = 0; i < defaultCases.Count; i++)
            {
                var useCase = defaultCases[i];
                object result = Resolve(useCase, new DefaultResolutionStrategy(serviceLocator, serviceLocator.Store));

                if (result != null)
                {
                    RaiseTypeResolvedEvent(useCase.GetBaseBindingType());
                    return result;
                }
            }

            throw new RegistrationNotFoundException(type);
        }

        private object Resolve(IUseCase useCase, IResolutionStrategy strategy)
        {
            var value = useCase.Resolve(strategy, this.serviceLocator.Store);

            if (value != null)
            {
                this.RaiseTypeResolvedEvent(useCase.GetBoundType());
                ExecutePostConditions<DefaultPostResolutionUseCaseManager>(useCase, actionUseCase => value = actionUseCase.Resolve(new PostResolutionStrategy(value), this.serviceLocator.Store));
                ExecutePostConditions<ConditionalPostResolutionUseCaseManager>(useCase, actionUseCase =>
                {
                    if (actionUseCase.IsValid(this.serviceLocator.Store))
                        value = actionUseCase.Resolve(new PostResolutionStrategy(value), this.serviceLocator.Store);
                });
            }

            return value;
        }

        private void ExecutePostConditions<TUseCaseManager>(IUseCase useCase, Action<IUseCase> action) where TUseCaseManager : IUseCaseManager
        {
            var manager = foundation.GetUseCaseManager<TUseCaseManager>();
            IList<IUseCase> actions = manager.GetUseCasesForType(useCase.GetBoundType()) ??
                                      manager.GetUseCasesForType(useCase.GetBaseBindingType());

            if (actions != null)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    var actionUseCase = actions[i];
                    action(actionUseCase);
                }
            }
        }
    }
}