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
using NUnit.Framework;
using Siege.ServiceLocator.RegistrationSyntax;
using Siege.ServiceLocator.Resolution;
using Siege.ServiceLocator.UnitTests.TestClasses;

namespace Siege.ServiceLocator.UnitTests
{
    [TestFixture]
	public abstract partial class ServiceLocatorTests
	{
		[Test]
		public void ShouldUseUnregisteredConstructorArgument()
		{
			locator.Register(Given<ITestInterface>.Then<DependsOnInterface>());

			var argument = new ConstructorArgument();
			var instance = locator.GetInstance<ITestInterface>(new ConstructorParameter {Name = "argument", Value = argument});

			Assert.AreSame(argument, ((DependsOnInterface) instance).Argument);
		}

		[Test]
		public void ShouldUseMultipleUnregisteredConstructorArgument()
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
		public virtual void ShouldUseUnregisteredConstructorArgumentWithName()
		{
			locator.Register(Given<ITestInterface>.Then<DependsOnInterface>("test"));

			var argument = new ConstructorArgument();
			var instance = locator.GetInstance<ITestInterface>("test",
			                                                   new ConstructorParameter {Name = "argument", Value = argument});

			Assert.AreSame(argument, ((DependsOnInterface) instance).Argument);
		}

		[Test]
		public void ShouldResolveWithArgsInAnyOrder()
		{
			locator.Register(Given<ITestInterface>.Then<DependsOnMultipleInterfaceTypes>());

			var argument = new ConstructorArgument();
			var instance = locator.GetInstance<ITestInterface>(new ConstructorParameter { Name = "arg", Value = argument });


			Assert.AreSame(argument, ((DependsOnMultipleInterfaceTypes)instance).Arg);
			Assert.AreSame(locator, ((DependsOnMultipleInterfaceTypes)instance).Locator);
		}

        [Test]
        public void ShouldBeAbleToSpecifyArgumentsAtRegistration()
        {
            var argument = new ConstructorArgument();
            var argument2 = new ConstructorArgument();

            var arguments = new List<IResolutionArgument>
                                {
                                    new ConstructorParameter {Name = "argument1", Value = argument},
                                    new ConstructorParameter {Name = "argument2", Value = argument2}
                                };

            locator.Register(Given<ITestInterface>.Then<DependsOnMultipleInterface>());
            locator.Register(Given<DependsOnMultipleInterface>.ConstructWith(arguments));

            var instance = locator.GetInstance<ITestInterface>();

            Assert.AreSame(argument, ((DependsOnMultipleInterface)instance).Argument);
            Assert.AreSame(argument2, ((DependsOnMultipleInterface)instance).Argument2);
        }

        [Test]
        public void ShouldBeAbleToSpecifyArgumentsUsedDuringLazyResolution()
        {
            var argument = new ConstructorArgument();
            var argument2 = new ConstructorArgument();

            var arguments = new List<IResolutionArgument>
                                {
                                    new ConstructorParameter {Name = "argument1", Value = argument},
                                    new ConstructorParameter {Name = "argument2", Value = argument2}
                                };

            locator.Register(Given<ITestInterface>.Then<DependsOnMultipleInterface>());
            locator.Register(Given<DependsOnMultipleInterface>.ConstructWith(arguments));

            var instance = locator.GetInstance<Func<ITestInterface>>();

            Assert.AreSame(argument, ((DependsOnMultipleInterface)instance()).Argument);
            Assert.AreSame(argument2, ((DependsOnMultipleInterface)instance()).Argument2);
        }
	}
}