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
using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.UnitTests.RegistrationExtensions
{
    public class DecoratorUseCase<TService> : GenericUseCase<TService>, IDecoratorUseCase<TService>
    {
        private IContextualServiceLocator serviceLocator;

        public DecoratorUseCase(IContextualServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public override Type GetUseCaseBindingType()
        {
            return typeof (IDecoratorUseCaseBinding<>);
        }

        public override Type GetBaseBindingType()
        {
            return GetBoundType();
        }

        protected override IActivationStrategy GetActivationStrategy()
        {
            return new DecoratorActivationStrategy(serviceLocator, GetBoundType());
        }

        protected class DecoratorActivationStrategy : IActivationStrategy
        {
            private readonly IContextualServiceLocator serviceLocator;
            private readonly Type decoratedType;

            public DecoratorActivationStrategy(IContextualServiceLocator serviceLocator, Type decoratedType)
            {
                this.serviceLocator = serviceLocator;
                this.decoratedType = decoratedType;
            }

            public object Resolve(IInstanceResolver locator, IStoreAccessor context)
            {
                var useCaseBinding = (IDecoratorUseCaseBinding)locator.GetInstance(typeof(IDecoratorUseCaseBinding<TService>));
                var decorators = new List<Type>();
                var rootObject = locator.GetInstance(decoratedType);

                foreach (IDecoratorUseCase useCase in serviceLocator.GetRegisteredUseCasesForType(decoratedType))
                {
                    if(useCase.IsValid(context)) decorators.Add(useCase.GetDecoratorType());
                }

                return GetInstance(useCaseBinding, rootObject, decoratedType, decorators);
            }

            private object GetInstance(IDecoratorUseCaseBinding binding, object rootObject, Type encapsulatedType, IList<Type> decorators)
            {
                if (decorators.Count == 0) return rootObject;

                Type decorator = decorators[0];
                IList<Type> prunedDecoratorList = new List<Type>(decorators);
                prunedDecoratorList.RemoveAt(0);

                object resolvedObject = binding.Resolve(decorator, encapsulatedType, rootObject);

                return GetInstance(binding, resolvedObject, encapsulatedType, prunedDecoratorList);
            }
        }

        public Type GetDecoratorType()
        {
            return typeof (TService);
        }
    }
}