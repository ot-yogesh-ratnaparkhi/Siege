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
using Siege.ServiceLocator.UnitTests.TestClasses;
using StructureMap;

namespace Siege.ServiceLocator.UnitTests.Adapters
{
	[TestFixture]
	[Category("StructureMap")]
	public class StructureMapAdapterTests : ServiceLocatorTests
	{
		private Container container;

		protected override IServiceLocatorAdapter GetAdapter()
		{
			container = new Container();
			return new StructureMapAdapter.StructureMapAdapter(container);
		}

		protected override void RegisterWithoutSiege<TFrom, TTo>()
		{
			container.Configure(
				registry => registry.For<TFrom>().Use<TTo>());
		}

		protected override void ResolveWithoutSiege<T>()
		{
			container.GetInstance<T>();
		}

		public override void ShouldNotBeAbleToBindAnInterfaceToATypeWithANameWhenNoNameProvided()
		{
			base.ShouldNotBeAbleToBindAnInterfaceToATypeWithANameWhenNoNameProvided();
			Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);
		}
	}
}