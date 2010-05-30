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
using Siege.ServiceLocation.UseCases.Named;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;

namespace Siege.ServiceLocation.StructureMapAdapter
{
	public class KeyBasedUseCaseBinding<TService> : IKeyBasedUseCaseBinding<TService>
	{
		private StructureMap.Container container;

		public KeyBasedUseCaseBinding(StructureMap.Container container)
		{
			this.container = container;
		}

		public void Bind(IUseCase useCase, IFactoryFetcher locator)
		{
			Bind((IKeyBasedUseCase<TService>) useCase);
		}

		private void Bind(IKeyBasedUseCase<TService> useCase)
		{
			if (container.Model.HasImplementationsFor(useCase.GetBoundType())) return;

			Registry registry = new Registry();
			registry.ForRequestedType<TService>().CacheBy(InstanceScope.PerRequest).AddInstances(ex => ex.OfConcreteType(useCase.GetBoundType()).WithName(useCase.Key));
			container.Configure(configure => configure.AddRegistry(registry));
		}

		public void BindInstance(IInstanceUseCase useCase, IFactoryFetcher locator)
		{
			Registry registry = new Registry();

			registry.ForRequestedType<TService>().CacheBy(InstanceScope.PerRequest).AddInstances(ex => ex.IsThis((TService) useCase.GetBinding()).WithName(((IKeyBasedInstanceUseCase) useCase).Key));

			container.Configure(configure => configure.AddRegistry(registry));
		}
	}
}