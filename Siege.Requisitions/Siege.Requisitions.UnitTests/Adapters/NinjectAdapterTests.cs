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
using NUnit.Framework;
using Siege.Requisitions.UnitTests.TestClasses;
using Siege.Requisitions.RegistrationSyntax;

namespace Siege.Requisitions.UnitTests.Adapters
{
	[Category("Ninject")]
	public class NinjectAdapterTests : ServiceLocatorTests
	{
		private IKernel kernel;

		protected override void ResolveWithoutSiege<T>()
		{
			kernel.Get<T>();
		}

		public override void SetUp()
		{
			kernel = new StandardKernel();
			base.SetUp();
		}

		protected override IServiceLocatorAdapter GetAdapter()
		{
			return new NinjectAdapter.NinjectAdapter(kernel);
		}

		protected override void RegisterWithoutSiege<TFrom, TTo>()
		{
			kernel.Bind<TFrom>().To<TTo>();
		}

		[Test]
		public void ShouldDisposeFromContainers()
		{
			var disposableContainer = new StandardKernel();
			using (
				var disposableLocater = new ThreadedServiceLocator(new NinjectAdapter.NinjectAdapter(disposableContainer)))
			{
				disposableLocater.Register(Given<ITestInterface>.Then<TestCase1>());
				Assert.IsTrue(disposableLocater.GetInstance<ITestInterface>() is TestCase1);
			}

			Assert.IsTrue(disposableContainer.IsDisposed);
		}

		public override void ShouldNotBeAbleToBindAnInterfaceToATypeWithANameWhenNoNameProvided()
		{
			base.ShouldNotBeAbleToBindAnInterfaceToATypeWithANameWhenNoNameProvided();

			Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);
		}
	}
}