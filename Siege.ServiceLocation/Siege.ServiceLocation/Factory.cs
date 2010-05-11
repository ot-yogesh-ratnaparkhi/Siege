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

using System.Collections.Generic;
using Siege.ServiceLocation.Exceptions;
using Siege.ServiceLocation.UseCases;
using Siege.ServiceLocation.UseCases.Conditional;
using Siege.ServiceLocation.UseCases.Default;

namespace Siege.ServiceLocation
{
    public class Factory<TBaseService> : IGenericFactory<TBaseService>
    {
        private IContextualServiceLocator serviceLocator;
        private readonly List<IUseCase> conditionalUseCases = new List<IUseCase>();
        private readonly List<IUseCase> defaultCases = new List<IUseCase>();

        public Factory(IContextualServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public void AddCase(IUseCase useCase)
        {
            if (useCase is IDefaultUseCase) defaultCases.Add(useCase);
            else conditionalUseCases.Add(useCase);
        }

        public TBaseService Build()
        {
            foreach (IGenericUseCase useCase in conditionalUseCases)
            {
                TBaseService result = default(TBaseService);

                if (useCase.IsValid(serviceLocator.Store))
                {
                    result =
                        (TBaseService)useCase.Resolve(new ConditionalResolutionStrategy(serviceLocator, serviceLocator.Store), serviceLocator.Store);
                }

                if (!Equals(result, default(TBaseService)))
                {
                    serviceLocator.Store.ExecutionStore.Decrement();
                    return result;
                }
            }

            foreach (IGenericUseCase useCase in defaultCases)
            {
                TBaseService result =
                    (TBaseService)useCase.Resolve(new DefaultResolutionStrategy(serviceLocator, serviceLocator.Store), serviceLocator.Store);

                if (!Equals(result, default(TBaseService)))
                {
                    serviceLocator.Store.ExecutionStore.Decrement();
                    return result;
                }
            }

            throw new RegistrationNotFoundException(typeof(TBaseService));
        }
    }
}