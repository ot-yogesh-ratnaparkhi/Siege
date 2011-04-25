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
using Rhino.Mocks;
using Siege.Requisitions.AutoMocker;
using Siege.Requisitions.AutoMocker.RhinoMocks;
using Siege.Requisitions.RegistrationSyntax;
using Siege.Requisitions.UnitTests.TestClasses;

namespace Siege.Requisitions.UnitTests
{
    public partial class ServiceLocatorTests
    {
        [Test]
        public void ShouldBeAbleToResolveTypeWithMockInterfaceDependencies()
        {
            locator.Register(Mock<TestClassWithInterfaceDependencies>.Using(new RhinoMockAdapter()));

            Assert.IsInstanceOf<TestInterfaceWithMethods>(locator.GetInstance<TestClassWithInterfaceDependencies>().TestInterface);
        }

        [Test]
        public void ShouldBeAbleToResolveTypeWithClassDenpendices()
        {
            locator.Register(Mock<TestClassWithClassDependencies>.Using(new RhinoMockAdapter()));

            var classDependencies = locator.GetInstance<TestClassWithClassDependencies>();
            Assert.IsInstanceOf<TestClassWithInterfaceDependencies>(classDependencies.TestClassDependices);
            Assert.IsInstanceOf<TestInterfaceWithMethods>(classDependencies.TestClassDependices.TestInterface);
        }

        [Test]
        public void ShouldBeAbleToSetExpecationOnMockedInstance()
        {
            var adapter = new RhinoMockAdapter();
            locator.Register(Mock<TestClassWithInterfaceDependencies>.Using(adapter));
            var testClass = locator.GetInstance<TestClassWithInterfaceDependencies>();

            using (adapter.Repository.Record())
            {
                testClass.TestInterface.Expect(i => i.GetSomeValue()).Return(1);
            }

            var result = testClass.TestInterface.GetSomeValue();

            Assert.AreEqual(1, result);

        }

        [Test]
        public void ShouldBeAbleToStubOnObjects()
        {
            var adapter = new RhinoMockAdapter();
            locator.Register(Mock<TestClassWithClassDependencies>.Using(adapter));
            var testClass = locator.GetInstance<TestClassWithInterfaceDependencies>();
            using (adapter.Repository.Record())
            {
                testClass.Stub(c => c.GetSomeValue(Arg<int>.Is.Anything)).Return(1);
            }
            var result = testClass.GetSomeValue(0);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldNotMockSameInterfaceTwice()
        {

            locator.Register(Mock<TestClassWithInterfaceDependencies>.Using(new RhinoMockAdapter()));

            Assert.AreSame(locator.GetInstance<TestInterfaceWithMethods>(), locator.GetInstance<TestClassWithInterfaceDependencies>().TestInterface);
        }


        [Test]
        public void ShouldNotStubSameClassTwice()
        {
            locator.Register(Mock<TestClassWithClassDependencies>.Using(new RhinoMockAdapter()));

            var testClass = locator.GetInstance<TestClassWithClassDependencies>();
            var testClassDependency = locator.GetInstance<TestClassWithInterfaceDependencies>();

            Assert.AreSame(testClass.TestClassDependices, testClassDependency);
        }

        [Test]
        public void ShouldBeAbleToMockInAAAMode()
        {
            var adapter = new RhinoMockAdapter(MockMode.AAA);
            locator.Register(Mock<TestClassWithInterfaceDependencies>.Using(adapter));
            var testClass = locator.GetInstance<TestClassWithInterfaceDependencies>();
            testClass.TestInterface.Expect(i => i.GetSomeValue()).Return(1);
            var result = testClass.TestInterface.GetSomeValue();

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldBeAbleToStubInAAAMode()
        {
            var adapter = new RhinoMockAdapter(MockMode.AAA);
            locator.Register(Mock<TestClassWithClassDependencies>.Using(adapter));
            var testClass = locator.GetInstance<TestClassWithInterfaceDependencies>();
            testClass.Stub(c => c.GetSomeValue(Arg<int>.Is.Anything)).Return(1);
            var result = testClass.GetSomeValue(0);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void AAAModeCanBeEnabledViaPropertySetter()
        {
            var adapter = new RhinoMockAdapter {MockingMode = MockMode.AAA};
            locator.Register(Mock<TestClassWithInterfaceDependencies>.Using(adapter));
            var testClass = locator.GetInstance<TestClassWithInterfaceDependencies>();
            testClass.TestInterface.Expect(i => i.GetSomeValue()).Return(1);
            var result = testClass.TestInterface.GetSomeValue();

            Assert.AreEqual(1, result);
        }

        [Test]
        public void SpecialTypesTest()
        {
            var adapter = new RhinoMockAdapter {MockingMode = MockMode.AAA};
            locator.Register(Mock<SpecialTestCase>.Using(adapter));

            var testClass = locator.GetInstance<SpecialTestCase>();
            var stringResult = testClass.StringInput;
            var intResult = testClass.IntInput;
            var boolResult = testClass.BoolInput;
            var enumReulst = testClass.EnumInput;
            var structResult = testClass.StructInput;

            Assert.AreEqual(string.Empty, stringResult);
            Assert.AreEqual(0, intResult);
            Assert.AreEqual(false, boolResult);
            Assert.AreEqual(TestEnum.Case1, enumReulst);
            Assert.IsTrue(structResult.x == 0 && structResult.y == 0);
        }

        [Test]
        public void ShouldNotMockTypesAlreadyRegisteredInServiceLocator()
        {
            var adapter = new RhinoMockAdapter();
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            adapter.MockingMode = MockMode.AAA;
            locator.Register(Mock<ITestInterface>.Using(adapter, locator));
            var testClass = locator.GetInstance<ITestInterface>();
            Assert.IsInstanceOf(typeof(TestCase1), testClass);
        }
    }
}