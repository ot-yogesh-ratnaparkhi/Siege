using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.Extensions.RubyInstaller;
using Siege.Requisitions.UnitTests.TestClasses;

namespace Siege.Requisitions.UnitTests
{
    public partial class ServiceLocatorTests
    {
        [Test]
        public void ShouldLoadFromRubyFile()
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
    }
}