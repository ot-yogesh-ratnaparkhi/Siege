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
using Siege.ServiceLocator.ExtensionMethods;
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.Resolution;

namespace Siege.ServiceLocator.Registrations
{
    public abstract class TypedRegistration : Registration
    {
        protected Type mapsToType;
        protected Type mapsFromType;

        protected TypedRegistration(Type mappedFromType)
        {
            this.mapsFromType = mappedFromType;
        }

        public void MapsTo<TImplementationType>()
        {
            MapsTo(typeof(TImplementationType));
        }

        public override void MapsTo(object implementationType)
        {
            mapsToType = (Type)implementationType;
        }

        protected override IActivationStrategy GetActivationStrategy()
        {
            return new TypedRegistrationActivationStrategy(mapsToType);
        }

        public override object GetMappedTo()
        {
            return GetMappedToType();
        }

        public override Type GetMappedToType()
        {
            return mapsToType;
        }

        public override Type GetMappedFromType()
        {
            return mapsFromType;
        }

        public class TypedRegistrationActivationStrategy : IActivationStrategy
        {
            private readonly Type mapsToType;

            public TypedRegistrationActivationStrategy(Type mapsToType)
            {
                this.mapsToType = mapsToType;
            }

            public object Resolve(IInstanceResolver locator, IServiceLocatorStore context)
            {
                var stores = context.All<IResolutionStore>().ToList();
                var items = new List<IResolutionArgument>();
                stores.ForEach(x => items.AddRange(x.Items));
                
                return locator.GetInstance(mapsToType, items.OfType<IResolutionArgument, IResolutionArgument>());
            }
        }
    }
}