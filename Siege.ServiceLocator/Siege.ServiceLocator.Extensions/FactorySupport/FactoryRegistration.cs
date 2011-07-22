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
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.Registrations;

namespace Siege.ServiceLocator.Extensions.FactorySupport
{
    public abstract class FactoryRegistration<TService> : TypedRegistration
    {
        private Func<IInstanceResolver, TService> factoryMethod;

        protected FactoryRegistration() : base(typeof(TService))
        {
            
        }

        public void ConstructWith(Func<IInstanceResolver, TService> factoryMethod)
        {
            this.factoryMethod = factoryMethod;
        }

        protected override IActivationStrategy GetActivationStrategy()
        {
            return new FactoryActivationStrategy(factoryMethod);
        }

        public class FactoryActivationStrategy : IActivationStrategy
        {
            private readonly Func<IInstanceResolver, TService> factoryMethod;

            public FactoryActivationStrategy(Func<IInstanceResolver, TService> factoryMethod)
            {
                this.factoryMethod = factoryMethod;
            }

            public object Resolve(IInstanceResolver locator, IServiceLocatorStore context)
            {
                return factoryMethod(locator);
            }
        }
    }
}