using NUnit.Framework;

namespace Siege.Security.Tests
{
    [TestFixture]
    public class RolesTests
    {
        private Role role;

        [SetUp]
        public void SetUp()
        {
            role = new Role();
            var permission = new Permission { Name = "Test Permission" };
            role.Permissions.Add(permission);
        }

        [Test]
        public void ShouldMatchWhenPermissionsMatch()
        {
            Assert.IsTrue(role.Can("Test Permission"));
        }

        [Test]
        public void ShouldNotMatchWhenPermissionsDontMatch()
        {
            Assert.IsFalse(role.Can("lol"));
        }

        [Test]
        public void ShouldMatchWhenMultiplePermissionsDefined()
        {
            var secondPermission = new Permission { Name = "2nd Permission" };
            role.Permissions.Add(secondPermission);
            Assert.IsTrue(role.Can("2nd Permission"));
        }
        
        [Test]
        public void ShouldNotMatchWhenMultiplePermissionsDefinedNoneMatch()
        {
            var secondPermission = new Permission { Name = "2nd Permission" };
            role.Permissions.Add(secondPermission);
            Assert.IsFalse(role.Can("lol"));
        }
    }
}