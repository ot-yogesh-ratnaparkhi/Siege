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
using Microsoft.Practices.Unity;
using Siege.ServiceLocation.Bindings;
using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.UnityAdapter
{
    public class DefaultInstanceUseCaseBinding<TService> : IDefaultInstanceUseCaseBinding<TService>
    {
        private IUnityContainer container;

        public DefaultInstanceUseCaseBinding(IUnityContainer container)
        {
            this.container = container;
        }

        public void Bind(IUseCase useCase, IFactoryFetcher locator)
        {
            Bind((DefaultInstanceUseCase<TService>)useCase, locator);
        }

        private void Bind(DefaultInstanceUseCase<TService> useCase, IFactoryFetcher locator)
        {
            var factory = (Factory<TService>)locator.GetFactory<TService>();
            factory.AddCase(useCase);

            if (typeof(TService) != useCase.GetBoundType())
            {
                container.RegisterType<TService>(new TransientLifetimeManager(), new InjectionFactory(f => factory.Build()));
                container.RegisterType<TService>(Guid.NewGuid().ToString(), new TransientLifetimeManager(), new InjectionFactory(f => factory.Build()));
            }
            container.RegisterInstance(useCase.GetBoundType(), useCase.GetBinding(), new TransientLifetimeManager());
        }
    }
}
