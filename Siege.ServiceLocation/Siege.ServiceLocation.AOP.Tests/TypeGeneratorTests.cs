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
using Siege.ServiceLocation.InternalStorage;
using Siege.ServiceLocation.RegistrationSyntax;

namespace Siege.ServiceLocation.AOP.Tests
{
    [TestFixture]
    public class TypeGeneratorTests
    {
        protected IContextualServiceLocator locator;

		[SetUp]
		public void SetUp()
		{
			Counter.Count = 0;
		}

        [Test]
        public void Should_Override_Virtual_Methods_With_Return_Types_With_ServiceLocator()
        {
            locator = new SiegeContainer(new NinjectAdapter.NinjectAdapter(), new ThreadedServiceLocatorStore());
            locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
            locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
            locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());

            Type type = new SiegeProxy(locator).WithServiceLocator().Create<TestType>();

            var instance = Activator.CreateInstance(type, locator);

			Assert.AreEqual("lolarg1", type.GetMethod("Test").Invoke(instance, new[] { "arg1", "arg2" }));
			Assert.AreEqual(1, Counter.Count);
        }

        [Test]
        public void Should_Override_Virtual_Methods_Without_Return_Types_With_ServiceLocator()
        {
            locator = new SiegeContainer(new NinjectAdapter.NinjectAdapter(), new ThreadedServiceLocatorStore());
            locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
            locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
            locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());

			Type type = new SiegeProxy(locator).WithServiceLocator().Create<TestType>();

            var instance = Activator.CreateInstance(type, locator);

			type.GetMethod("TestNoReturn").Invoke(instance, new[] { "arg1", "arg2" });
			Assert.AreEqual(1, Counter.Count);
        }

        [Test]
        public void Should_Override_Virtual_Methods_With_Return_Types_Without_ServiceLocator()
        {
            locator = new SiegeContainer(new NinjectAdapter.NinjectAdapter(), new ThreadedServiceLocatorStore());
            locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
            locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
            locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());

			Type type = new SiegeProxy(locator).Create<TestType>();

            var instance = Activator.CreateInstance(type);

			Assert.AreEqual("lolarg1", type.GetMethod("Test").Invoke(instance, new[] { "arg1", "arg2" }));
			Assert.AreEqual(1, Counter.Count);
        }

        [Test]
        public void Should_Override_Virtual_Methods_Without_Return_Types_Without_ServiceLocator()
        {
            locator = new SiegeContainer(new NinjectAdapter.NinjectAdapter(), new ThreadedServiceLocatorStore());
            locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
            locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
            locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());

			Type type = new SiegeProxy(locator).Create<TestType>();

            var instance = Activator.CreateInstance(type);

			type.GetMethod("TestNoReturn").Invoke(instance, new[] { "arg1", "arg2" });
			Assert.AreEqual(1, Counter.Count);
        }

		[Test]
		public void Should_Override_Virtual_Methods_With_Return_Types_With_ServiceLocator_Multiple_Encapsulation()
		{
			locator = new SiegeContainer(new NinjectAdapter.NinjectAdapter(), new ThreadedServiceLocatorStore());
			locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
			locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
			locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());

			Type type = new SiegeProxy(locator).WithServiceLocator().Create<TestType2>();

			var instance = Activator.CreateInstance(type, locator);

			Assert.AreEqual("lolarg1", type.GetMethod("Test").Invoke(instance, new[] { "arg1", "arg2" }));
			Assert.AreEqual(3, Counter.Count);
		}
	}
}