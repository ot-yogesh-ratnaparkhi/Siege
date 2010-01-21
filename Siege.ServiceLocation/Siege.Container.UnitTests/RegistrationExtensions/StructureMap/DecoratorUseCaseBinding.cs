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
using System.Linq;
using Siege.ServiceLocation.UseCases;
using StructureMap;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace Siege.ServiceLocation.UnitTests.RegistrationExtensions.StructureMap
{
    public class DecoratorUseCaseBinding<TService> : IDecoratorUseCaseBinding<TService>
    {
        private Container container;

        public DecoratorUseCaseBinding(Container container)
        {
            this.container = container;
        }

        public void Bind(IUseCase useCase, IFactoryFetcher locator)
        {
            Bind((IDecoratorUseCase<TService>)useCase);
        }

        public void BindInstance(IInstanceUseCase useCase, IFactoryFetcher locator)
        {
            
        }

        public object Resolve(Type typeToResolve, Type argumentType, object rootObject)
        {
            string parameterName = typeToResolve.GetConstructor(new[] { argumentType }).GetParameters().Where(parameter => parameter.ParameterType == argumentType).First().Name;

            Dictionary<string, object> dictionary = new Dictionary<string, object> { { parameterName, rootObject } };
            return container.GetInstance(typeToResolve, new ExplicitArguments(dictionary));
        }

        private void Bind(IDecoratorUseCase<TService> useCase)
        {
            Registry registry = new Registry();

            registry.ForRequestedType(useCase.GetDecoratorType()).CacheBy(InstanceScope.PerRequest).TheDefaultIsConcreteType(useCase.GetDecoratorType());
            
            container.Configure(configure => configure.AddRegistry(registry));
        }
    }
}