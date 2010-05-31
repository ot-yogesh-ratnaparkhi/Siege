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
using Siege.ServiceLocation.Resolution;
using Siege.ServiceLocation.SiegeAdapter;
using Siege.ServiceLocation.SiegeAdapter.ConstructionStrategies;

namespace Siege.ServiceLocation.UnitTests.Adapters
{
    [Category("Siege")]
	public class SiegeAdapterTests : SiegeContainerTests
	{
        private SiegeTypeResolver reflectionResolver;

        public override void SetUp()
        {
            reflectionResolver = new SiegeTypeResolver(new ReflectionConstructionStrategy());
            base.SetUp();
        }

		protected override IServiceLocatorAdapter GetAdapter()
		{
			return new SiegeAdapter.SiegeAdapter(new ReflectionConstructionStrategy());
		}

		protected override void RegisterWithoutSiege<TFrom, TTo>()
		{
            reflectionResolver.Register(typeof(TFrom), typeof(TTo));
		}

		protected override void ResolveWithoutSiege<T>()
		{
		    reflectionResolver.Get(typeof (T), new ConstructorParameter[] {});
		}

		[Ignore]
		public override void Should_Resolve_If_Exists_In_IoC_But_Not_Registered_In_Container()
		{
			base.Should_Resolve_If_Exists_In_IoC_But_Not_Registered_In_Container();
		}
	}
}