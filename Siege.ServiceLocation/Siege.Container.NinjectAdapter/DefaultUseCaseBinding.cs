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

using Ninject;
using Siege.ServiceLocation.Bindings;
using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.NinjectAdapter
{
    public class DefaultUseCaseBinding<TService> : IDefaultUseCaseBinding<TService>
    {
        private IKernel kernel;

        public DefaultUseCaseBinding(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public void Bind(IUseCase useCase, IFactoryFetcher locator)
        {
            var factory = (Factory<TService>)locator.GetFactory<TService>();
            factory.AddCase(useCase);

            if (typeof(TService) != useCase.GetBoundType()) kernel.Bind<TService>().ToMethod(context => factory.Build());
            kernel.Bind(useCase.GetBoundType()).ToSelf();
        }

        public void BindInstance(IInstanceUseCase useCase, IFactoryFetcher locator)
        {
            var factory = (Factory<TService>)locator.GetFactory<TService>();
            factory.AddCase(useCase);

            if (typeof(TService) != useCase.GetBoundType()) kernel.Bind<TService>().ToMethod(context => factory.Build());
            kernel.Bind(useCase.GetBoundType()).ToConstant(useCase.GetBinding());
        }
    }
}