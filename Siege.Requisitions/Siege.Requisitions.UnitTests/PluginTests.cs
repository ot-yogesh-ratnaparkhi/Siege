using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.Extensions.RubyInstaller;
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
                typeof (IServiceLocator).Assembly,
                typeof (ITestInterface).Assembly,
                typeof (RubyInstaller).Assembly
            };

            locator.Register(Install.From("Installers\\test.rb", assemblies));

            var instance = locator.GetInstance(typeof (ITestInterface));
            var instance2 = locator.GetInstance(typeof (ITestInterface));

            Assert.IsInstanceOf<TestCase1>(instance);
            Assert.AreSame(instance, instance2);
        }

        [Test]
        public void RubyConditionalRegistration()
        {
            var assemblies = new List<Assembly>
            {
                typeof (IServiceLocator).Assembly,
                typeof (ITestInterface).Assembly,
                typeof (RubyInstaller).Assembly
            };

            locator.Register(Install.From("Installers\\test.rb", assemblies));


            Assert.IsInstanceOf<TestCase2>(locator.GetInstance<ITestInterface>(new ContextArgument(new TestContext(TestEnum.Case2))));
        }
    }
}