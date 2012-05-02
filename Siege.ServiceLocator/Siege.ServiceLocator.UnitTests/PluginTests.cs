using NUnit.Framework;
using Siege.ServiceLocator.Dynamic;
using Siege.ServiceLocator.Resolution;
using Siege.ServiceLocator.UnitTests.TestClasses;

namespace Siege.ServiceLocator.UnitTests
{
    [TestFixture]
    public partial class ServiceLocatorTests
    {
        [Test]
        public void RubyDefaultRegistration()
        {
            locator.Register(Install.From("Installers\\DefaultRegistrationTest.rb"));

            var instance = locator.GetInstance<ITestInterface>();

            Assert.IsInstanceOf<TestCase1>(instance);
        }

        [Test]
        public void RubyConditionalRegistration()
        {
            locator.Register(Install.From("Installers\\ConditionalRegistrationTest.rb"));


            Assert.IsInstanceOf<TestCase2>(locator.GetInstance<ITestInterface>(new ContextArgument(new TestClasses.TestContext(TestEnum.Case2))));
        }

        [Test]
        public void RubySingletonRegistration()
        {
            locator.Register(Install.From("Installers\\SingletonRegistrationTest.rb"));

            var instance = locator.GetInstance<ITestInterface>();
            var instance2 = locator.GetInstance<ITestInterface>();

            Assert.IsInstanceOf<TestCase1>(instance);
            Assert.AreSame(instance, instance2);
        }

        [Test]
        public void RubyDefaultInstanceRegistration()
        {
            locator.Register(Install.From("Installers\\DefaultInstanceRegistrationTest.rb"));

            var instance = locator.GetInstance<ITestInterface>();

            Assert.IsInstanceOf<TestCase1>(instance);
        }

        [Test]
        public void RubyConditionalInstanceRegistration()
        {
            locator.Register(Install.From("Installers\\ConditionalInstanceRegistrationTest.rb"));

            var instance = locator.GetInstance<ITestInterface>(new ContextArgument(new TestClasses.TestContext(TestEnum.Case2)));
        
            Assert.IsInstanceOf<TestCase2>(instance);
        }

        [Test]
        public void RubyNamedRegistration()
        {
            locator.Register(Install.From("Installers\\NamedRegistrationTest.rb"));

            var instance = locator.GetInstance<ITestInterface>("Test");

            Assert.IsInstanceOf<TestCase2>(instance);

            instance = locator.GetInstance<ITestInterface>("Test1");

            Assert.IsInstanceOf<TestCase1>(instance);
        }

        [Test]
        public void RubyNamedInstanceRegistration()
        {
            locator.Register(Install.From("Installers\\NamedInstanceRegistrationTest.rb"));

            var instance = locator.GetInstance<ITestInterface>("Test");

            Assert.IsInstanceOf<TestCase2>(instance);

            instance = locator.GetInstance<ITestInterface>("Test1");

            Assert.IsInstanceOf<TestCase1>(instance);
        }

        [Test]
        public void RubyIConditionRegistration()
        {
            locator.Register(Install.From("Installers\\ConditionalGenericRegistrationTest.rb"));

            locator.AddContext(CreateContext(TestEnum.Case2));

            Assert.IsInstanceOf<TestCase2>(locator.GetInstance<ITestInterface>());
        }

        [Test]
        public void RubyIConditionInstanceRegistration()
        {
            locator.Register(Install.From("Installers\\ConditionalGenericInstanceRegistrationTest.rb"));

            locator.AddContext(CreateContext(TestEnum.Case2));

            Assert.IsInstanceOf<TestCase2>(locator.GetInstance<ITestInterface>());
        }

        [Test]
        public void RubyConventionRegistration()
        {
            locator.Register(Install.From("Installers\\ConventionRegistrationTest.rb"));

            Assert.IsInstanceOf<AutoScannedType>(locator.GetInstance<IAutoScannedInterface>());
        }

        [Test]
        public void RubyConventionInstanceRegistration()
        {
            locator.Register(Install.From("Installers\\ConventionInstanceRegistrationTest.rb"));

            Assert.IsInstanceOf<AutoScannedType>(locator.GetInstance<IAutoScannedInterface>());
        }
    }
}