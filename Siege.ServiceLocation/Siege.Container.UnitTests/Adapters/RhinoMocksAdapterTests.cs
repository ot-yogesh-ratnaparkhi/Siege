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

namespace Siege.ServiceLocation.UnitTests.Adapters
{
	public class RhinoMocksAdapterTests : SiegeContainerTests
	{
		protected override IServiceLocatorAdapter GetAdapter()
		{
			return new RhinoMocksAdapter.RhinoMocksAdapter();
		}

		protected override void RegisterWithoutSiege<TFrom, TTo>()
		{
		}

		protected override void ResolveWithoutSiege<T>()
		{
		}

		[Ignore]
		public override void WithoutSiege()
		{
			base.WithoutSiege();
		}

		[Ignore]
		public override void Should_Resolve_If_Exists_In_IoC_But_Not_Registered_In_Container()
		{
			base.Should_Resolve_If_Exists_In_IoC_But_Not_Registered_In_Container();
		}

        [Ignore]
        public override void Should_Use_SiegeProxy_TypeBuilder()
        {
            base.Should_Use_SiegeProxy_TypeBuilder();
        }

        [Ignore]
        public override void Load_Performance_Test()
        {
            base.Load_Performance_Test();
        } 
	}
}