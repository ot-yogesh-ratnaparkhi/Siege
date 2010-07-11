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

using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using NUnit.Framework;
using Siege.Requisitions.UnitTests.TestClasses;
using Siege.Requisitions.Exceptions;
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.RegistrationSyntax;

namespace Siege.Requisitions.UnitTests.Adapters
{
	[TestFixture]
	[Category("Windsor")]
	public class WindsorAdapterTests : SiegeContainerTests
	{
		private IKernel kernel;

		protected override void ResolveWithoutSiege<T>()
		{
			kernel.Resolve<T>();
		}

		public override void SetUp()
		{
			kernel = new DefaultKernel();
			base.SetUp();
		}

		protected override IServiceLocatorAdapter GetAdapter()
		{
			return new WindsorAdapter.WindsorAdapter(kernel);
		}

		protected override void RegisterWithoutSiege<TFrom, TTo>()
		{
			kernel.Register(Component.For<TFrom>().ImplementedBy<TTo>());
		}

		[Test, Ignore("Bug in Windsor lol")]
		public virtual void Should_Dispose_From_Containers()
		{
			var disposableKernel = new DefaultKernel();
			using (
				var disposableLocater = new SiegeContainer(new WindsorAdapter.WindsorAdapter(disposableKernel), new ThreadedServiceLocatorStore()))
			{
				disposableLocater.Register(Given<ITestInterface>.Then<TestCase1>());
				Assert.IsTrue(disposableLocater.GetInstance<ITestInterface>() is TestCase1);
			}

			Assert.IsFalse(disposableKernel.HasComponent(typeof (ITestInterface)));
		}

		[ExpectedException(typeof (RegistrationNotFoundException))]
		public override void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_Wrong_Name_Provided()
		{
			base.Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_Wrong_Name_Provided();
		}

		[ExpectedException(typeof (RegistrationNotFoundException))]
		public override void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_No_Name_Provided()
		{
			base.Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_No_Name_Provided();
		}

        [Ignore("Castle Dynamic Proxy doesn't seem to play nice!")]
        public override void Should_Proxy_All_Types()
        {
            base.Should_Proxy_All_Types();
        }
	}
}