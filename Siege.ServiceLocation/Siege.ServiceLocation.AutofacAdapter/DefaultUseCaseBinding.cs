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
using Autofac;
using Autofac.Builder;
using Siege.ServiceLocation.Bindings;
using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.AutofacAdapter
{
    public class DefaultUseCaseBinding<TService> : IDefaultUseCaseBinding<TService>
    {
        private IContainer container;

        public DefaultUseCaseBinding(IContainer container)
        {
            this.container = container;
        }

        public void Bind(IUseCase useCase, IFactoryFetcher locator)
        {
            var factory = (Factory<TService>)locator.GetFactory<TService>();
            factory.AddCase(useCase);

            var builder = new ContainerBuilder();

            if (typeof(TService) != useCase.GetBoundType())
            {
                if (!container.IsRegistered<IEnumerable<TService>>()) builder.RegisterCollection<TService>().As<IEnumerable<TService>>();
                builder.Register((c => factory.Build())).As<TService>().FactoryScoped().MemberOf<IEnumerable<TService>>();
            }

            builder.Register(useCase.GetBoundType()).FactoryScoped();

            builder.Build(container);
        }

        public void BindInstance(IInstanceUseCase useCase, IFactoryFetcher locator)
        {
            var factory = (Factory<TService>)locator.GetFactory<TService>();
            factory.AddCase(useCase);

            var builder = new ContainerBuilder();

            if (typeof(TService) != useCase.GetBoundType())
            {
                if (!container.IsRegistered<IEnumerable<TService>>()) builder.RegisterCollection<TService>().As<IEnumerable<TService>>();
                builder.Register((c => factory.Build())).As<TService>().FactoryScoped().MemberOf<IEnumerable<TService>>();
            }

            builder.Register(useCase.GetBoundType());
            builder.Register(c => useCase.GetBinding());

            builder.Build(container);
        }
    }
}