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
using Siege.ServiceLocation.Extensions.ExtendedSyntax;
using Siege.ServiceLocation.Resolution;
using Siege.ServiceLocation.UnitTests.TestClasses;

namespace Siege.ServiceLocation.UnitTests
{
	public abstract partial class SiegeContainerTests
	{
		[Test]
		public void Should_Resolve_With_Request_Level_Context()
		{
			locator.Register(Given<ITestInterface>
			                 	.When<TestContext>(context => context.TestCases == TestEnum.Case2)
			                 	.Then<TestCase2>());

			Assert.IsTrue(locator.GetInstance<ITestInterface>(new ContextArgument(CreateContext(TestEnum.Case2))) is TestCase2);
		}

		[Test]
		public void Should_Resolve_With_Request_Level_Context_Differently_Per_Request()
		{
			locator
				.Register(Given<ITestInterface>
				          	.When<TestContext>(context => context.TestCases == TestEnum.Case2)
				          	.Then<TestCase2>())
				.Register(Given<ITestInterface>
				          	.When<TestContext>(context => context.TestCases == TestEnum.Case1)
				          	.Then<TestCase1>());

			Assert.IsTrue(locator.GetInstance<ITestInterface>(new ContextArgument(CreateContext(TestEnum.Case2))) is TestCase2);
			Assert.IsTrue(locator.GetInstance<ITestInterface>(new ContextArgument(CreateContext(TestEnum.Case1))) is TestCase1);
		}

		[Test]
		public void Should_Resolve_With_Request_Level_Context_Instead_Of_ContextStore()
		{
			locator
				.Register(Given<ITestInterface>
				          	.When<TestContext>(context => context.TestCases == TestEnum.Case2)
				          	.Then<TestCase2>())
				.Register(Given<ITestInterface>
				          	.When<TestContext>(context => context.TestCases == TestEnum.Case1)
				          	.Then<TestCase1>());

			locator.AddContext(CreateContext(TestEnum.Case1));

			Assert.IsTrue(locator.GetInstance<ITestInterface>(new ContextArgument(CreateContext(TestEnum.Case2))) is TestCase2);
			Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);
		}
	}
}