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

using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Siege.ServiceLocation;
using Siege.ServiceLocation.Bindings;
using Siege.ServiceLocation.UseCases;

namespace Siege.SeviceLocation.WindsorAdapter
{
    public class OpenGenericUseCaseBinding : IOpenGenericUseCaseBinding
    {
        private IKernel kernel;

        public OpenGenericUseCaseBinding(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public void Bind(IUseCase useCase, IFactoryFetcher locator)
        {
            var factory = (Factory<object>) locator.GetFactory<object>();
            factory.AddCase(useCase);

            //kernel.Register(Component.For(useCase.GetBoundType()).UsingFactoryMethod(() => factory.Build()).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
            kernel.Register(
                Component.For(useCase.GetBaseBindingType()).ImplementedBy(useCase.GetBoundType()).LifeStyle.Transient.
                    Unless(Component.ServiceAlreadyRegistered));
        }
    }
}