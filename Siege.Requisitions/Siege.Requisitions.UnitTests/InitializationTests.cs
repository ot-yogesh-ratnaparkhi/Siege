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

namespace Siege.Requisitions.UnitTests
{
    public interface ISimpleType
    {
        string Property { get; set; }
    }

    public class SimpleType : ISimpleType
    {
        public string Property { get; set; }
    }

    public abstract partial class ServiceLocatorTests
    {
        [Test]
        public void ShouldInitializePropertyAfterResolution()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<TestCase1>.InitializeWith(testCase1 => testCase1.Property1 = "lulz"));

            var instance = (TestCase1)locator.GetInstance<ITestInterface>();
            Assert.AreEqual("lulz", instance.Property1);
        }

        [Test]
        public void ShouldInitializePropertyAfterResolutionForBaseType()
        {
            locator.Register(Given<ISimpleType>.Then<SimpleType>());
            locator.Register(Given<ISimpleType>.InitializeWith(testCase1 => testCase1.Property = "lulz"));

            var instance = locator.GetInstance<ISimpleType>();
            Assert.AreEqual("lulz", instance.Property);
        }

        [Test]
        public void ShouldInitializePropertyAfterResolutionDependingOnContext()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(
                Given<TestCase1>.When<TestEnum>(x => x == TestEnum.Case2).InitializeWith(
                    testCase1 => testCase1.Property1 = "lulz"));

            locator.AddContext(TestEnum.Case2);

            var instance = (TestCase1)locator.GetInstance<ITestInterface>();
            Assert.AreEqual("lulz", instance.Property1);
        }


        [Test]
        public void ShouldInitializePropertyAfterResolutionWithNoContext()
        {
            locator
                .Register(Given<ITestInterface>.Then<TestCase1>())
                .Register(Given<TestCase1>.InitializeWith(testCase1 => testCase1.Property1 = "lulz"))
                .Register(Given<TestCase1>.When<TestEnum>(x => x == TestEnum.Case2).InitializeWith(
                    testCase1 => testCase1.Property1 = "rofl"));

            var instance = (TestCase1)locator.GetInstance<ITestInterface>();
            Assert.AreEqual("lulz", instance.Property1);
        }

        [Test]
        public void ShouldInitializeDirectMappedTypes()
        {
            locator
                .Register(Given<TestCase1>.Then<TestCase1>())
                .Register(Given<TestCase1>.InitializeWith(testCase1 => testCase1.Property1 = "lulz"));

            var instance = locator.GetInstance<TestCase1>();
            Assert.AreEqual("lulz", instance.Property1);
        }

        [Test]
        public void ShouldInitializeDirectMappedTypesWhenNested()
        {
            locator
                .Register(Given<TestContainer>.Then<TestContainer>())
                .Register(Given<TestCase1>.Then<TestCase1>())
                .Register(Given<TestCase1>.InitializeWith(testCase1 => testCase1.Property1 = "lulz"));

            var instance = locator.GetInstance<TestContainer>();
            Assert.AreEqual("lulz", instance.TestCase.Property1);
        }

        public class TestContainer
        {
            private readonly TestCase1 testCase;

            public TestContainer(TestCase1 testCase)
            {
                this.testCase = testCase;
            }

            public TestCase1 TestCase
            {
                get { return testCase; }
            }
        }
    }
}