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

using NUnit.Framework;
using Siege.ServiceLocator.Resolution;
using Siege.ServiceLocator.Native;
using Siege.ServiceLocator.Native.ConstructionStrategies;

namespace Siege.ServiceLocator.UnitTests.Adapters
{
    [Category("Siege")]
    public class ILSiegeAdapterTests : ServiceLocatorTests
    {
        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new Native.SiegeAdapter();
        }

        private SiegeTypeResolver ilResolver;

        public override void SetUp()
        {
            ilResolver = new SiegeTypeResolver(new SiegeProxyConstructionStrategy());
            base.SetUp();
        }

        protected override void RegisterWithoutSiege<TFrom, TTo>()
        {
            ilResolver.Register(typeof(TFrom), typeof(TTo));
        }

        protected override void ResolveWithoutSiege<T>()
        {
            ilResolver.Get(typeof(T), new ConstructorParameter[] { });
        }

        [Ignore]
        public override void ShouldResolveIfExistsInIoCButNotRegisteredInContainer()
        {
            base.ShouldResolveIfExistsInIoCButNotRegisteredInContainer();
        }
    }
}