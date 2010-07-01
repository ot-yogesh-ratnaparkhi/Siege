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

using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.Bindings.Default
{
    public class DefaultUseCaseBinding : IUseCaseBinding, IInstanceUseCaseBinding
    {
        private readonly IServiceLocatorAdapter adapter;

        public DefaultUseCaseBinding(IServiceLocatorAdapter adapter)
        {
            this.adapter = adapter;
        }

        void IUseCaseBinding.Bind(IUseCase useCase, IFactoryFetcher locator)
        {
            var factory = (Factory) locator.GetFactory(useCase.GetBaseBindingType());
            factory.AddCase(useCase);

            if (useCase.GetBaseBindingType() != useCase.GetBoundType())
            {
                adapter.RegisterFactoryMethod(useCase.GetBaseBindingType(), () => factory.Build(useCase.GetBoundType()));
            }

            adapter.Register(useCase.GetBoundType(), useCase.GetBoundType());
        }

        void IInstanceUseCaseBinding.Bind(IUseCase useCase, IFactoryFetcher locator)
        {
            var factory = (Factory)locator.GetFactory(useCase.GetBaseBindingType());
            factory.AddCase(useCase);

            if (useCase.GetBaseBindingType() != useCase.GetBoundType())
            {
                adapter.RegisterFactoryMethod(useCase.GetBaseBindingType(), () => factory.Build(useCase.GetBoundType()));
            }

            adapter.RegisterInstance(useCase.GetBoundType(), useCase.GetBinding());
        }
    }
}