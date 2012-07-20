using NUnit.Framework;

namespace Siege.Security.Tests
{
    [TestFixture]
    public class GroupTests
    {
        private Group group;

        [SetUp]
        public void SetUp()
        {
            group = new Group();
            var role = new Role();
            var permission = new Permission { Name = "Test Permission" };
            role.Permissions.Add(permission);
            group.Roles.Add(role);
        }

        [Test]
        public void ShouldMatchWhenGroupHasRoleWithPermission()
        {
            Assert.IsTrue(group.Can("Test Permission"));
        }

        [Test]
        public void ShouldNotMatchWhenGroupHasNoRoleWithPermission()
        {
            Assert.IsFalse(group.Can("lol"));
        }

        [Test]
        public void ShouldMatchWhenGroupHasMultipleRolesWithMatch()
        {
            var secondRole = new Role();
            var secondPermission = new Permission { Name = "2nd Permission" };
            secondRole.Permissions.Add(secondPermission);
            group.Roles.Add(secondRole);

            Assert.IsTrue(group.Can("2nd Permission"));
        }

        [Test]
        public void ShouldNotMatchWhenGroupHasMultipleRolesWithNoMatch()
        {
            var secondRole = new Role();
            var secondPermission = new Permission { Name = "lol" };
            secondRole.Permissions.Add(secondPermission);
            group.Roles.Add(secondRole);

            Assert.IsFalse(group.Can("2nd Permission"));
        }
    }
}