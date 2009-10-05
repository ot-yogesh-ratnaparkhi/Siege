using System;
using System.Collections.ObjectModel;
using NUnit.Framework;
using Siege.ServiceLocation;
using Siege.ServiceLocation.Aop;

namespace Siege.Container.UnitTests.TypeGenerationTests
{
    [TestFixture]
    public class TypeGeneratorTests
    {
        [SetUp]
        public void SetUp()
        {
            AopBinder.ServiceLocator = null;
        }

        [Test]
        public void Should_Generate_Type()
        {
            var result = AopBinder.Generate<TestType>();
            TestType instance = (TestType)Activator.CreateInstance(result);
            Assert.IsTrue(instance is TestType);
            Assert.AreNotEqual(typeof(TestType), instance.GetType());
        }

        [Test]
        public void Should_Throw_Exception_From_Attribute()
        {
            var result = AopBinder.Generate<TestType>();
            TestType instance = (TestType)Activator.CreateInstance(result);
            
            var test = instance.Test("yay", "yay");

            Assert.AreEqual("yay", test);
        }
    }

    public class TestType
    {
        [SamplePreProcessing, SamplePostProcessing]
        public virtual string Test(object arg1, object arg2)
        {
            return arg1.ToString();
        }
    }

    public class TestBase
    {
        protected readonly IContextualServiceLocator locator;

        public TestBase(IContextualServiceLocator locator)
        {
            this.locator = locator;
        }

        public virtual string Test(object arg1, object arg2)
        {
            return "yay";
        }
    }

    public class TestClass : TestBase
    {
        public object sample;

        public TestClass(IContextualServiceLocator locator) : base(locator)
        {
        }

        public override string Test(object arg1, object arg2)
        {
            sample = locator;
            new SamplePreProcessingAttribute().Process();
            var value = this.Test2(arg1, arg2);
            //var value = new SampleEncapsulatingAttribute(locator).Process(() => this.Test2(arg1, arg2));
            new SamplePostProcessingAttribute().Process();

            return value;
        }

        public string Test2(object arg1, object arg2)
        {
            return base.Test(arg1, arg2);
        }
    }
}
