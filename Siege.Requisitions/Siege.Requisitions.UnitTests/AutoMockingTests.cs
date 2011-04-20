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
        public void Should_Be_Able_To_Resolve_Type_With_Mock_Interface_Dependencies()
        {
            locator.Register(Mock<TestClassWithInterfaceDependencies>.Using(new RhinoMockAdapter()));

            Assert.IsInstanceOfType(typeof(TestInterfaceWithMethods), locator.GetInstance<TestClassWithInterfaceDependencies>().TestInterface);
        }

        [Test]
        public void Should_Be_Able_To_Resolve_Type_With_Class_Denpendices()
        {
            locator.Register(Mock<TestClassWithClassDependencies>.Using(new RhinoMockAdapter()));

            var classDependencies = locator.GetInstance<TestClassWithClassDependencies>();
            Assert.IsInstanceOfType(typeof(TestClassWithInterfaceDependencies), classDependencies.TestClassDependices);
            Assert.IsInstanceOfType(typeof(TestInterfaceWithMethods), classDependencies.TestClassDependices.TestInterface);
        }

        [Test]
        public void Should_Be_Able_To_Set_Expecation_On_Mocked_Instance()
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
        public void Should_Be_Able_To_Stub_On_Objects()
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
        public void Should_Not_Mock_Same_Interface_Twice()
        {

            locator.Register(Mock<TestClassWithInterfaceDependencies>.Using(new RhinoMockAdapter()));

            Assert.AreSame(locator.GetInstance<TestInterfaceWithMethods>(), locator.GetInstance<TestClassWithInterfaceDependencies>().TestInterface);
        }


        [Test]
        public void Should_Not_Stub_Same_Class_Twice()
        {
            locator.Register(Mock<TestClassWithClassDependencies>.Using(new RhinoMockAdapter()));

            var testClass = locator.GetInstance<TestClassWithClassDependencies>();
            var testClassDependency = locator.GetInstance<TestClassWithInterfaceDependencies>();

            Assert.AreSame(testClass.TestClassDependices, testClassDependency);
        }

        [Test]
        public void Should_Be_Able_To_Mock_In_AAA_Mode()
        {
            var adapter = new RhinoMockAdapter(MockMode.AAA);
            locator.Register(Mock<TestClassWithInterfaceDependencies>.Using(adapter));
            var testClass = locator.GetInstance<TestClassWithInterfaceDependencies>();
            testClass.TestInterface.Expect(i => i.GetSomeValue()).Return(1);
            var result = testClass.TestInterface.GetSomeValue();

            Assert.AreEqual(1, result);
        }

        [Test]
        public void Should_Be_Able_To_Stub_In_AAA_Mode()
        {
            var adapter = new RhinoMockAdapter(MockMode.AAA);
            locator.Register(Mock<TestClassWithClassDependencies>.Using(adapter));
            var testClass = locator.GetInstance<TestClassWithInterfaceDependencies>();
            testClass.Stub(c => c.GetSomeValue(Arg<int>.Is.Anything)).Return(1);
            var result = testClass.GetSomeValue(0);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void AAA_Mode_Can_Be_Enabled_Via_Property_Setter()
        {
            var adapter = new RhinoMockAdapter();
            adapter.MockingMode = MockMode.AAA;
            locator.Register(Mock<TestClassWithInterfaceDependencies>.Using(adapter));
            var testClass = locator.GetInstance<TestClassWithInterfaceDependencies>();
            testClass.TestInterface.Expect(i => i.GetSomeValue()).Return(1);
            var result = testClass.TestInterface.GetSomeValue();

            Assert.AreEqual(1, result);
        }

        [Test]
        public void Special_Types_Test()
        {
            var adapter = new RhinoMockAdapter();
            adapter.MockingMode = MockMode.AAA;
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
        public void Should_Not_Mock_Types_Already_Registered_In_Service_Locator()
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