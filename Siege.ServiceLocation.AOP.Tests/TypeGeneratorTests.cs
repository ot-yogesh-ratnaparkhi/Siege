using System;
using NUnit.Framework;
using Siege.ServiceLocation.Aop;

namespace Siege.ServiceLocation.AOP.Tests
{
    [TestFixture]
    public class TypeGeneratorTests
    {
        protected IContextualServiceLocator locator;

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

        [Test, Ignore]
        public void Should_Throw_Exception_From_Attribute()
        {
            var result = AopBinder.Generate<TestType>();
            TestType instance = (TestType)Activator.CreateInstance(result);
            
            var test = instance.Test("yay", "yay");

            Assert.AreEqual("yay", test);
        }

        [Test, Ignore]
        public void Should_Use_AOP()
        {
            locator.Register(Given<AOPExample>.Then<AOPExample>());
            locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
            locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
            locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());
            locator.GetInstance<AOPExample>().Test();
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
            var value = Test2(arg1, arg2);
            //var value = new SampleEncapsulatingAttribute(locator).Process(() => this.Test2(arg1, arg2));
            new SamplePostProcessingAttribute().Process();

            return value;
        }

        public string Test2(object arg1, object arg2)
        {
            return base.Test(arg1, arg2);
        }
    }

    public class AOPExample
    {
        [SampleEncapsulating]
        public virtual string Test()
        {
            return "yay";
        }
    }

    public class SampleBase
    {
        protected readonly IContextualServiceLocator locator;

        public SampleBase(IContextualServiceLocator locator)
        {
            this.locator = locator;
        }

        public virtual string Test()
        {
            return "yay";
        }
    }

    public class SampleClass : SampleBase
    {
        public SampleClass(IContextualServiceLocator locator)
            : base(locator)
        {
        }

        public override string Test()
        {
            return locator.GetInstance<SampleEncapsulatingAttribute>().Process(() => Test2());
        }

        private string Test2()
        {
            return base.Test();
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
            return default(TResponseType);
        }

        public void Process(Action action)
        {
        }
    }

    public class SamplePreProcessingAttribute : Attribute, IPreProcessingAttribute
    {
        public void Process()
        {

        }
    }

    public class SamplePostProcessingAttribute : Attribute, IPostProcessingAttribute
    {
        public void Process()
        {
        }
    }
}