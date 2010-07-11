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
using NUnit.Framework;
using Siege.Requisitions.UnitTests.TestClasses;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;

namespace Siege.Requisitions.UnitTests
{
    public abstract partial class SiegeContainerTests
    {
        [Test]
        public void Should_Resolve_Open_Generic()
        {
            locator
                .Register(Given<TestCase1>.Then<TestCase1>())
                .Register(Given.OpenType(typeof (ISomeType<>)).Then(typeof (ConcreteType<>)));

            Assert.IsInstanceOfType(typeof(ConcreteType<TestCase1>), locator.GetInstance<ISomeType<TestCase1>>());
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "Type: Siege.Requisitions.UnitTests.TestClasses.ITestInterface is not a generic type.")]
        public void Should_Throw_Exception_On_NonGeneric_Source()
        {
            locator
                .Register(Given<TestCase1>.Then<TestCase1>())
                .Register(Given.OpenType(typeof(ITestInterface)).Then(typeof(ConcreteType<>)));

            Assert.IsInstanceOfType(typeof(ConcreteType<TestCase1>), locator.GetInstance<ISomeType<TestCase1>>());
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "Type: Siege.Requisitions.UnitTests.TestClasses.ITestInterface is not a generic type.")]
        public void Should_Throw_Exception_On_NonGeneric_Target()
        {
            locator
                .Register(Given<TestCase1>.Then<TestCase1>())
                .Register(Given.OpenType(typeof(ISomeType<>)).Then(typeof(ITestInterface)));

            Assert.IsInstanceOfType(typeof(ConcreteType<TestCase1>), locator.GetInstance<ISomeType<TestCase1>>());
        }
    }

    public interface ISomeType<T> {}
    public class ConcreteType<T> : ISomeType<T> {}
}