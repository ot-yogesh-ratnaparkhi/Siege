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

using Siege.ServiceLocation.Bindings;
using Siege.ServiceLocation.UseCases;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;

namespace Siege.ServiceLocation.StructureMapAdapter
{
    public class ConditionalUseCaseBinding<TService> : IConditionalUseCaseBinding<TService>
    {
        private StructureMap.Container container;

        public ConditionalUseCaseBinding(StructureMap.Container container)
        {
            this.container = container;
        }

        public void Bind(IUseCase useCase, IFactoryFetcher locator)
        {
            Registry registry = new Registry();

            var factory = (Factory<TService>)locator.GetFactory<TService>();
            factory.AddCase(useCase);

            registry.ForRequestedType<TService>().CacheBy(InstanceScope.PerRequest).TheDefault.Is.ConstructedBy(context => factory.Build());
            registry.ForRequestedType(useCase.GetBoundType()).CacheBy(InstanceScope.PerRequest).AddType(useCase.GetBoundType());
            container.Configure(configure => configure.AddRegistry(registry));
        }
    }
}