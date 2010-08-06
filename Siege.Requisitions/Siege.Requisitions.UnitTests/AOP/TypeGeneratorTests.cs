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
using Siege.Arsenal;
using Siege.Requisitions.RegistrationSyntax;

namespace Siege.Requisitions.UnitTests.AOP
{
    [TestFixture]
    public class TypeGeneratorTests
    {
        protected IServiceLocator locator;

        [SetUp]
        public void SetUp()
        {
            Counter.Count = 0;
        }

        [Test]
        public void ShouldOverrideVirtualMethodsWithReturnTypesWithServiceLocator()
        {
            locator = new ThreadedServiceLocator(new NinjectAdapter.NinjectAdapter());
            locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
            locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
            locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());

            Type type = new SiegeProxy().WithServiceLocator().Create<TestType>();

            var instance = Activator.CreateInstance(type, locator);

            Assert.AreEqual("lolarg1", type.GetMethod("Test").Invoke(instance, new[] { "arg1", "arg2" }));
            Assert.AreEqual(1, Counter.Count);
        }

        [Test]
        public void ShouldOverrideVirtualMethodsWithoutReturnTypesWithServiceLocator()
        {
            locator = new ThreadedServiceLocator(new NinjectAdapter.NinjectAdapter());
            locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
            locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
            locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());

            Type type = new SiegeProxy().WithServiceLocator().Create<TestType>();

            var instance = Activator.CreateInstance(type, locator);

            type.GetMethod("TestNoReturn").Invoke(instance, new[] { "arg1", "arg2" });
            Assert.AreEqual(1, Counter.Count);
        }

        [Test]
        public void ShouldOverrideVirtualMethodsWithReturnTypesWithoutServiceLocator()
        {
            locator = new ThreadedServiceLocator(new NinjectAdapter.NinjectAdapter());
            locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
            locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
            locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());

            Type type = new SiegeProxy().Create<TestType>();

            var instance = Activator.CreateInstance(type);

            Assert.AreEqual("lolarg1", type.GetMethod("Test").Invoke(instance, new[] { "arg1", "arg2" }));
            Assert.AreEqual(1, Counter.Count);
        }

        [Test]
        public void ShouldOverrideVirtualMethodsWithoutReturnTypesWithoutServiceLocator()
        {
            locator = new ThreadedServiceLocator(new NinjectAdapter.NinjectAdapter());
            locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
            locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
            locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());

            Type type = new SiegeProxy().Create<TestType>();

            var instance = Activator.CreateInstance(type);

            type.GetMethod("TestNoReturn").Invoke(instance, new[] { "arg1", "arg2" });
            Assert.AreEqual(1, Counter.Count);
        }

        [Test]
        public void ShouldOverrideVirtualMethodsWithReturnTypesWithServiceLocatorMultipleEncapsulation()
        {
            locator = new ThreadedServiceLocator(new NinjectAdapter.NinjectAdapter());
            locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
            locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
            locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());

            Type type = new SiegeProxy().WithServiceLocator().Create<TestType2>();

            var instance = Activator.CreateInstance(type, locator);

            Assert.AreEqual("lolarg1", type.GetMethod("Test").Invoke(instance, new[] { "arg1", "arg2" }));
            Assert.AreEqual(3, Counter.Count);
        }
    }
}