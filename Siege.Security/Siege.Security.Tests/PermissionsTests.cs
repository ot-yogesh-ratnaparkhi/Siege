using NUnit.Framework;

namespace Siege.Security.Tests
{
    [TestFixture]
    public class PermissionsTests
    {
        private Permission permission;

        [SetUp]
        public void StartUp()
        {
            permission = new Permission { Name = "TestPermission" };
        }

        [Test]
        public void ShouldAllowWhenPermissionsMatch()
        {
            Assert.IsTrue(permission.Can("TestPermission"));
        }

        [Test]
        public void ShouldNotAllowWhenPermissionsDontMatch()
        {
            Assert.IsFalse(permission.Can("lol"));
        }

    }
}