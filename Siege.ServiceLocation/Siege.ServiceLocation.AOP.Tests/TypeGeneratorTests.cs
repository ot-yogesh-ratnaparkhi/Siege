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
using Siege.ServiceLocation.Stores;
using Siege.ServiceLocation.Syntax;

namespace Siege.ServiceLocation.AOP.Tests
{
    [TestFixture]
    public class TypeGeneratorTests
    {
        protected IContextualServiceLocator locator;

        [Test]
        public void Should_Override_Virtual_Methods_With_Return_Types_With_ServiceLocator()
        {
            locator = new SiegeContainer(new NinjectAdapter.NinjectAdapter(), new ThreadedServiceLocatorStore());
            locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
            locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
            locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());

            Type type = new SiegeProxy().WithServiceLocator().Create<TestType>();

            var instance = Activator.CreateInstance(type, locator);

            Assert.AreEqual("lolarg1", type.GetMethod("Test").Invoke(instance, new[] { "arg1", "arg2"}));
        }

        [Test]
        public void Should_Override_Virtual_Methods_Without_Return_Types_With_ServiceLocator()
        {
            locator = new SiegeContainer(new NinjectAdapter.NinjectAdapter(), new ThreadedServiceLocatorStore());
            locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
            locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
            locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());

            Type type = new SiegeProxy().WithServiceLocator().Create<TestType>();

            var instance = Activator.CreateInstance(type, locator);

            type.GetMethod("TestNoReturn").Invoke(instance, new[] { "arg1", "arg2" });
        }

        [Test]
        public void Should_Override_Virtual_Methods_With_Return_Types_Without_ServiceLocator()
        {
            locator = new SiegeContainer(new NinjectAdapter.NinjectAdapter(), new ThreadedServiceLocatorStore());
            locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
            locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
            locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());

            Type type = new SiegeProxy().Create<TestType>();

            var instance = Activator.CreateInstance(type);

            Assert.AreEqual("lolarg1", type.GetMethod("Test").Invoke(instance, new[] { "arg1", "arg2" }));
        }

        [Test]
        public void Should_Override_Virtual_Methods_Without_Return_Types_Without_ServiceLocator()
        {
            locator = new SiegeContainer(new NinjectAdapter.NinjectAdapter(), new ThreadedServiceLocatorStore());
            locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
            locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
            locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());

            Type type = new SiegeProxy().Create<TestType>();

            var instance = Activator.CreateInstance(type);

            type.GetMethod("TestNoReturn").Invoke(instance, new[] { "arg1", "arg2" });
        }
    }

    public class TestType
    {
        [SamplePreProcessing, SampleEncapsulating, SamplePostProcessing]
        public virtual string Test(object arg1, object arg2)
        {
            return "lol" + arg1;
        }

        [SamplePreProcessing, SampleEncapsulating, SamplePostProcessing]
        public virtual void TestNoReturn(object arg1, object arg2)
        {
        }
    }

    
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class SampleEncapsulatingAttribute : Attribute, IProcessEncapsulatingAttribute
    {
        private readonly IContextualServiceLocator locator;

        public SampleEncapsulatingAttribute() { }

        public SampleEncapsulatingAttribute(IContextualServiceLocator locator)
        {
            this.locator = locator;
        }

        public TResponseType Process<TResponseType>(Func<TResponseType> func)
        {
            return func();
        }

        public void Process(Action action)
        {
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class SamplePreProcessingAttribute : Attribute, IPreProcessingAttribute
    {
        public void Process()
        {

        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class SamplePostProcessingAttribute : Attribute, IPostProcessingAttribute
    {
        public void Process()
        {
        }
    }
}