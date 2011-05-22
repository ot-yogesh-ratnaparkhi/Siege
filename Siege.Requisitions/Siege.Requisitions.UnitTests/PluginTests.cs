using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Siege.Requisitions.Dynamic;
using Siege.Requisitions.Resolution;
using Siege.Requisitions.UnitTests.TestClasses;
using TestContext = Siege.Requisitions.UnitTests.TestClasses.TestContext;

namespace Siege.Requisitions.UnitTests
{
    public partial class ServiceLocatorTests
    {
        [Test]
        public void RubyDefaultRegistration()
        {
            var assemblies = new List<Assembly>
            {
                typeof (ITestInterface).Assembly
            };

            locator.Register(Install.From("Installers\\DefaultRegistrationTest.rb", assemblies));

            var instance = locator.GetInstance<ITestInterface>();

            Assert.IsInstanceOf<TestCase1>(instance);
        }

        [Test]
        public void RubyConditionalRegistration()
        {
            var assemblies = new List<Assembly>
            {
                typeof (ITestInterface).Assembly,
            };

            locator.Register(Install.From("Installers\\ConditionalRegistrationTest.rb", assemblies));


            Assert.IsInstanceOf<TestCase2>(locator.GetInstance<ITestInterface>(new ContextArgument(new TestContext(TestEnum.Case2))));
        }

        [Test]
        public void RubySingletonRegistration()
        {
            var assemblies = new List<Assembly>
            {
                typeof (ITestInterface).Assembly
            };

            locator.Register(Install.From("Installers\\SingletonRegistrationTest.rb", assemblies));

            var instance = locator.GetInstance<ITestInterface>();
            var instance2 = locator.GetInstance<ITestInterface>();

            Assert.IsInstanceOf<TestCase1>(instance);
            Assert.AreSame(instance, instance2);
        }

        [Test]
        public void RubyDefaultInstanceRegistration()
        {
            var assemblies = new List<Assembly>
            {
                typeof (ITestInterface).Assembly
            };

            locator.Register(Install.From("Installers\\DefaultInstanceRegistrationTest.rb", assemblies));

            var instance = locator.GetInstance<ITestInterface>();
            var instance2 = locator.GetInstance<ITestInterface>();

            Assert.IsInstanceOf<TestCase1>(instance);
            Assert.AreSame(instance, instance2);
        }

        [Test]
        public void RubyConditionalInstanceRegistration()
        {
            var assemblies = new List<Assembly>
            {
                typeof (ITestInterface).Assembly,
            };

            locator.Register(Install.From("Installers\\ConditionalInstanceRegistrationTest.rb", assemblies));

            var instance = locator.GetInstance<ITestInterface>(new ContextArgument(new TestContext(TestEnum.Case2)));
            var instance2 = locator.GetInstance<ITestInterface>(new ContextArgument(new TestContext(TestEnum.Case2)));

            Assert.IsInstanceOf<TestCase2>(instance);
            Assert.AreSame(instance, instance2);
        }
    }
}