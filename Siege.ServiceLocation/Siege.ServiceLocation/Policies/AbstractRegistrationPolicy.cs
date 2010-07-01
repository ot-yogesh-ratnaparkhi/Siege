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
using Siege.ServiceLocation.Stores;
using Siege.ServiceLocation.UseCases;
using Siege.ServiceLocation.UseCases.Named;

namespace Siege.ServiceLocation.Policies
{
    public abstract class AbstractRegistrationPolicy : IRegistrationPolicy
    {
        protected readonly IUseCase useCase;

        protected AbstractRegistrationPolicy(IUseCase useCase)
        {
            this.useCase = useCase;
        }

        public object GetBinding()
        {
            return useCase.GetBinding();
        }

        public Type GetBoundType()
        {
            return useCase.GetBoundType();
        }

        public Type GetUseCaseBindingType()
        {
            return useCase.GetUseCaseBindingType();
        }

        public Type GetBaseBindingType()
        {
            return useCase.GetBaseBindingType();
        }

        public abstract object Resolve(IResolutionStrategy strategy, IServiceLocatorStore accessor);

        public bool IsValid(IServiceLocatorStore context)
        {
            return useCase.IsValid(context);
        }

        public string Key
        {
            get { return ((INamedUseCase) useCase).Key; }
        }
    }
}