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
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.UnitTests.TestClasses;
using TestContext = Siege.Requisitions.UnitTests.TestClasses.TestContext;

namespace Siege.Requisitions.UnitTests
{
    public partial class ServiceLocatorTests
    {
        [Test]
        public void ShouldUseCorrectRuleGivenMultipleConditions()
        {
            locator
                .Register(Given<ITestInterface>
                              .When<TestContext>(context =>
                                                            {
                                                                context.When(context2 => context.TestCases == TestEnum.Case2);
                                                                context.When(context2 => context.TestCases == TestEnum.Case2);
                                                            })
                              .Then<TestCase2>());                
            locator.AddContext(CreateContext(TestEnum.Case2));

            Assert.IsInstanceOf<TestCase1>(locator.GetInstance<ITestInterface>());
        }
    }
}
