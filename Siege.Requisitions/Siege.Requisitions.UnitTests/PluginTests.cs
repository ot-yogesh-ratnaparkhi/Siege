using NUnit.Framework;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.UnitTests.TestClasses;

namespace Siege.Requisitions.UnitTests
{
    public partial class ServiceLocatorTests
    {
        [Test]
        public void ShouldLoadFromPythonFile()
        {
            locator.Register(Install.From("Installers\\Installer.py"));

            Assert.IsInstanceOf<TestCase1>(locator.GetInstance<ITestInterface>());
        }
    }
}