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

using Autofac;
using Autofac.Builder;
using NUnit.Framework;
using Siege.ServiceLocation.Exceptions;

namespace Siege.ServiceLocation.UnitTests.Adapters
{
	[TestFixture]
	[Category("Autofac")]
	public class AutofacAdapterTests : SiegeContainerTests
	{
		private IContainer container;


		protected override void ResolveWithoutSiege<T>()
		{
			container.Resolve<T>();
		}

		public override void SetUp()
		{
			container = new Container();
			base.SetUp();
		}

		protected override IServiceLocatorAdapter GetAdapter()
		{
			return new AutofacAdapter.AutofacAdapter(container);
		}

		protected override void RegisterWithoutSiege<TFrom, TTo>()
		{
			var builder = new ContainerBuilder();
			builder.Register<TTo>().As<TFrom>();
			builder.Build(container);
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
	}
}