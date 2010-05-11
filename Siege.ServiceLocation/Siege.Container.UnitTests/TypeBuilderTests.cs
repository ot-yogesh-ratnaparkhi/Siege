using System;
using NUnit.Framework;
using Siege.ServiceLocation.AOP;
using Siege.ServiceLocation.Syntax;

namespace Siege.ServiceLocation.UnitTests
{
    public abstract partial class SiegeContainerTests
    {
        [Test]
        public void Should_Use_SiegeProxy_TypeBuilder()
        {
            locator.Register(Given<SampleEncapsulatingAttribute>.Then<SampleEncapsulatingAttribute>());
            locator.Register(Given<SamplePreProcessingAttribute>.Then<SamplePreProcessingAttribute>());
            locator.Register(Given<SamplePostProcessingAttribute>.Then<SamplePostProcessingAttribute>());
            locator.Register(Given<TestType>.Then<TestType>());

            var instance = locator.GetInstance<TestType>();

            Assert.AreEqual("lolarg1", instance.Test("arg1", "arg2"));
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