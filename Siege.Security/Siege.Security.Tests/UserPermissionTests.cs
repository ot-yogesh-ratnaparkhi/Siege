using NUnit.Framework;

namespace Siege.Security.Tests
{
    [TestFixture]
    public class UserPermissionTests
    {
        private User user;

        [SetUp]
        public void SetUp()
        {
            user = new User();

            var group = new Group();
            var role = new Role();
            var permission = new Permission { Name = "Test Permission" };
            role.Permissions.Add(permission);
            group.Roles.Add(role);
            user.Groups.Add(group);

            user.IsActive = true;
            user.IsAuthenticated = true;
        }

        [Test]
        public void ShouldMatchWhenGroupHasRoleWithPermissionAndUserIsActiveAuthenticated()
        {
            Assert.IsTrue(user.Can("Test Permission"));
        }

        [Test]
        public void ShouldNotMatchWhenGroupHasNoRoleWithPermissionAndUserIsActiveAuthenticated()
        {
            Assert.IsFalse(user.Can("lol"));
        }

        [Test]
        public void ShouldMatchWhenGroupHasMultipleRolesWithMatchAndUserIsActiveAuthenticated()
        {
            var secondGroup = new Group();
            var secondRole = new Role();
            var secondPermission = new Permission { Name = "2nd Permission" };
            secondRole.Permissions.Add(secondPermission);
            secondGroup.Roles.Add(secondRole);

            user.Groups.Add(secondGroup);
            Assert.IsTrue(user.Can("2nd Permission"));
        }

        [Test]
        public void ShouldNotMatchWhenGroupHasMultipleRolesWithNoMatchAndUserIsActiveAuthenticated()
        {
            var secondGroup = new Group();
            var secondRole = new Role();
            var secondPermission = new Permission { Name = "2nd Permission" };
            secondRole.Permissions.Add(secondPermission);
            secondGroup.Roles.Add(secondRole);

            user.Groups.Add(secondGroup);
            Assert.IsFalse(user.Can("lol"));
        }

        [Test]
        public void ShouldNotMatchWhenUserNotActive()
        {
            user.IsActive = false;

            Assert.IsFalse(user.Can("Test Permission"));
        }

        [Test]
        public void ShouldNotMatchWhenUserNotAuthenticated()
        {
            user.IsAuthenticated = false;

            Assert.IsFalse(user.Can("Test Permission"));
        }
    }
}