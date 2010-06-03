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
using Siege.ServiceLocation.UseCases;
using Siege.ServiceLocation.UseCases.Conditional;
using Siege.ServiceLocation.UseCases.Default;

namespace Siege.ServiceLocation
{
    public class Factory : IGenericFactory, ITypeResolver
    {
        private IContextualServiceLocator serviceLocator;
        private readonly List<IUseCase> conditionalUseCases = new List<IUseCase>();
        private readonly List<IUseCase> defaultCases = new List<IUseCase>();

        public event TypeResolvedEventHandler TypeResolved;

        public Factory(IContextualServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
            this.serviceLocator.Store.ExecutionStore.WireEvent(this);
        }

        public void AddCase(IUseCase useCase)
        {
            if (useCase is IDefaultUseCase) defaultCases.Add(useCase);
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
                var useCase = (IGenericUseCase)conditionalUseCases[i];
                object result = null;

                if (useCase.IsValid(serviceLocator.Store))
                {
                    result = useCase.Resolve(new ConditionalResolutionStrategy(serviceLocator, serviceLocator.Store), serviceLocator.Store);
                }

                if (result != null)
                {
                    RaiseTypeResolvedEvent(useCase.GetBaseBindingType());
                    return result;
                }
            }

            for(int i = 0; i < defaultCases.Count; i++)
            {
                var useCase = (IGenericUseCase)defaultCases[i];
                object result = useCase.Resolve(new DefaultResolutionStrategy(serviceLocator, serviceLocator.Store), serviceLocator.Store);

                if (result != null)
                {
                    RaiseTypeResolvedEvent(useCase.GetBaseBindingType());
                    return result;
                }
            }

            throw new RegistrationNotFoundException(type);
        }
    }
}