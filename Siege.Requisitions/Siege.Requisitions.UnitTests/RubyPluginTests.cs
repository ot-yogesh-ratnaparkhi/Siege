using NUnit.Framework;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.UnitTests.TestClasses;

namespace Siege.Requisitions.UnitTests
{
    public partial class ServiceLocatorTests
    {
        [Test, Ignore]
        public void ShouldLoadFromRubyFile()
        {
            locator.Register(Install.From("Installers\\test.rb"));

            Assert.IsInstanceOf<TestCase1>(locator.GetInstance<TestCase1>());
        }
    }
}