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
using Siege.ServiceLocation.UseCases.Named;

namespace Siege.ServiceLocation.Bindings.Named
{
    public class NamedUseCaseBinding : IUseCaseBinding, IInstanceUseCaseBinding
    {
        private readonly IServiceLocatorAdapter adapter;

        public NamedUseCaseBinding(IServiceLocatorAdapter adapter)
        {
            this.adapter = adapter;
        }

        public void Bind(IUseCase useCase, IFactoryFetcher locator)
        {
            Bind((IKeyBasedUseCase) useCase);
        }

        public void BindInstance(IInstanceUseCase useCase, IFactoryFetcher locator)
        {
            BindInstance((IKeyBasedInstanceUseCase) useCase);
        }

        private void BindInstance(IKeyBasedInstanceUseCase useCase)
        {
            adapter.RegisterInstanceWithName(useCase.GetBoundType(), useCase.GetBinding(), useCase.Key);
            adapter.RegisterInstanceWithName(useCase.GetBaseBindingType(), useCase.GetBinding(), useCase.Key);
        }

        private void Bind(IKeyBasedUseCase useCase)
        {
            adapter.RegisterWithName(useCase.GetBoundType(), useCase.GetBoundType(), useCase.Key);
            adapter.RegisterWithName(useCase.GetBaseBindingType(), useCase.GetBoundType(), useCase.Key);
        }
    }
}