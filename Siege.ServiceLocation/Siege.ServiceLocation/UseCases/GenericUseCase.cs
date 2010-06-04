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
using Siege.ServiceLocation.ExtensionMethods;
using Siege.ServiceLocation.Resolution;
using Siege.ServiceLocation.Stores;

namespace Siege.ServiceLocation.UseCases
{
    public abstract class GenericUseCase<TBaseService> : UseCase<TBaseService, Type>
    {
        protected Type boundType;

        public void BindTo<TImplementationType>()
        {
            BindTo(typeof(TImplementationType));
        }

        public void BindTo(Type implementationType)
        {
            boundType = TypeHandler.Build(implementationType);
        }

        protected override IActivationStrategy GetActivationStrategy()
        {
            return new GenericActivationStrategy(boundType);
        }

        public override Type GetBoundType()
        {
            return boundType;
        }

        public override Type GetBaseBindingType()
        {
            return typeof(TBaseService);
        }

        public class GenericActivationStrategy : IActivationStrategy
        {
            private readonly Type boundType;

            public GenericActivationStrategy(Type boundType)
            {
                this.boundType = boundType;
            }

            public object Resolve(IInstanceResolver locator, IServiceLocatorStore context)
            {
				return locator.GetInstance(boundType, context.ResolutionStore.Items.OfType<IResolutionArgument, IResolutionArgument>());
            }
        }
    }
}