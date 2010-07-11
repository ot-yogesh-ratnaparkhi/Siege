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
using Siege.Requisitions.UnitTests.TestClasses;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.Resolution;

namespace Siege.Requisitions.UnitTests
{
	public abstract partial class SiegeContainerTests
	{
		[Test]
		public void Should_Use_Unregistered_Constructor_Argument()
		{
			locator.Register(Given<ITestInterface>.Then<DependsOnInterface>());

			var argument = new ConstructorArgument();
			var instance = locator.GetInstance<ITestInterface>(new ConstructorParameter {Name = "argument", Value = argument});

			Assert.AreSame(argument, ((DependsOnInterface) instance).Argument);
		}

		[Test]
		public void Should_Use_Multiple_Unregistered_Constructor_Argument()
		{
			locator.Register(Given<ITestInterface>.Then<DependsOnMultipleInterface>());

			var argument = new ConstructorArgument();
			var argument2 = new ConstructorArgument();
			var instance = locator.GetInstance<ITestInterface>(new ConstructorParameter {Name = "argument1", Value = argument},
			                                                   new ConstructorParameter {Name = "argument2", Value = argument2});

			Assert.AreSame(argument, ((DependsOnMultipleInterface) instance).Argument);
			Assert.AreSame(argument2, ((DependsOnMultipleInterface) instance).Argument2);
		}

		[Test]
		public virtual void Should_Use_Unregistered_Constructor_Argument_With_Name()
		{
			locator.Register(Given<ITestInterface>.Then<DependsOnInterface>("test"));

			var argument = new ConstructorArgument();
			var instance = locator.GetInstance<ITestInterface>("test",
			                                                   new ConstructorParameter {Name = "argument", Value = argument});

			Assert.AreSame(argument, ((DependsOnInterface) instance).Argument);
		}

		[Test]
		public void Should_Resolve_With_Args_In_Any_Order()
		{
			locator.Register(Given<ITestInterface>.Then<DependsOnMultipleInterfaceTypes>());

			var argument = new ConstructorArgument();
			var instance = locator.GetInstance<ITestInterface>(new ConstructorParameter { Name = "arg", Value = argument });


			Assert.AreSame(argument, ((DependsOnMultipleInterfaceTypes)instance).Arg);
			Assert.AreSame(locator, ((DependsOnMultipleInterfaceTypes)instance).Locator);
		}
	}
}