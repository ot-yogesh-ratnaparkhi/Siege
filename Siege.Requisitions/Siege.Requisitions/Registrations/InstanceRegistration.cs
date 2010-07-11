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
using Siege.Requisitions.InternalStorage;

namespace Siege.Requisitions.Registrations
{
    public abstract class InstanceRegistration<TBaseService> : Registration
    {
        protected TBaseService implementation;

        public override void MapsTo(object implementation)
        {
            this.implementation = (TBaseService)implementation;
        }

        public override object GetMappedTo()
        {
            return implementation;
        }

        protected override IActivationStrategy GetActivationStrategy()
        {
            return new InstanceRegistrationActivationStrategy(implementation);
        }

        public override Type GetMappedToType()
        {
            return implementation.GetType();
        }

        public override Type GetMappedFromType()
        {
            return typeof(TBaseService);
        }

        public class InstanceRegistrationActivationStrategy : IActivationStrategy
        {
            private readonly TBaseService implementation;

            public InstanceRegistrationActivationStrategy(TBaseService implementation)
            {
                this.implementation = implementation;
            }

            public object Resolve(IInstanceResolver locator, IServiceLocatorStore context)
            {
                return implementation;
            }
        }
    }
}