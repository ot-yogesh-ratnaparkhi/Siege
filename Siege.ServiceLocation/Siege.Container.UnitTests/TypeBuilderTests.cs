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
using Siege.ServiceLocation.AOP.Attributes;
using Siege.ServiceLocation.Syntax;

namespace Siege.ServiceLocation.UnitTests
{
    public abstract partial class SiegeContainerTests
    {
        [Test]
        public virtual void Should_Use_SiegeProxy_TypeBuilder()
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
    public class SamplePreProcessingAttribute : Attribute, IDefaultPreProcessingAttribute
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