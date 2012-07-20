//using NUnit.Framework;

//namespace Siege.Security.Tests.SQL
//{
//    public class SqlPermissionProviderTests : SqlTest
//    {
//        [Test]
//        public void ShouldBeAbleToCreateAPermission()
//        {
//            var permission = new Permission
//                                 {
//                                     Name = "Test Permission 1",
//                                     Description = "Test Description"
//                                 };

//            permissionProvider.Save(permission);

//            var savedPermission = repository.Get<Permission>(permission.ID);

//            Assert.AreEqual(permission.ID, savedPermission.ID);
//            Assert.AreEqual(permission.Name, savedPermission.Name);
//            Assert.AreEqual(permission.Description, savedPermission.Description);
//        }

//        [Test]
//        public void ShouldBeAbleToValidateRuleForPermission()
//        {
//            var permission = new Permission
//            {
//                Name = "Test Permission 1",
//                Description = "Test Description"
//            };

//            permissionProvider.Save(permission);

//            var savedPermission = repository.Get<Permission>(permission.ID);

//            Assert.IsTrue(savedPermission.Can("Test Permission 1"));
//        }

//    }
//}