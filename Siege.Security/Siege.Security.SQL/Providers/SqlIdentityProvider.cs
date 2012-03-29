using System;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Security;
using Siege.Repository;
using Siege.Security.Providers;
using Siege.Security.SQL.Repository;

namespace Siege.Security.SQL.Providers
{
    public class SqlIdentityProvider : IIdentityProvider
    {
        private readonly IRepository<SecurityDatabase> repository;

        public SqlIdentityProvider(IRepository<SecurityDatabase> repository)
        {
            this.repository = repository;
        }

        public void Create(User user)
        {
            var status = MembershipCreateStatus.Success;

            if (string.IsNullOrEmpty(user.Password))
            {
                status = MembershipCreateStatus.InvalidPassword;
            }

            if(string.IsNullOrEmpty(user.Name))
            {
                status = MembershipCreateStatus.InvalidUserName;
            }

            //if(string.IsNullOrEmpty(user.Email))
            //{
            //    status = MembershipCreateStatus.InvalidEmail;
            //}

            if(repository.Query<User>(query => query.Where(u => u.Name == user.Name)).FindFirstOrDefault() != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }

            if (repository.Query<User>(query => query.Where(u => u.Email == user.Email)).FindFirstOrDefault() != null)
            {
                status = MembershipCreateStatus.DuplicateEmail;
            }

            if (status != MembershipCreateStatus.Success)
            {
                RaiseMembershipCreationError(user.Name, status);
            }
            
            var hashedPassword = HashPassword(user.Password, CreateSalt(8));

            user.Password = hashedPassword.Value;
            user.Salt = hashedPassword.Salt;
            
            repository.Save(user);
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var loadedUser = repository.Query<User>(query => query.Where(u => u.Name == userName)).FindFirstOrDefault();

            if(loadedUser == null) return false;

            var hashedPassword = HashPassword(oldPassword, loadedUser.Salt);

            if(loadedUser.Password != hashedPassword.Value) return false;

            var updatedPassword = HashPassword(newPassword, loadedUser.Salt);

            loadedUser.Password = updatedPassword.Value;

            repository.Save(loadedUser);

            return true;
        }
        
        public bool UnlockUser(User user)
        {
            user.IsLockedOut = false;
            repository.Save(user);

            return true;
        }

        public int MinimumRequiredPasswordLength
        {
            get
            {
                return 6;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            var loadedUser = repository.Query<User>(query => query.Where(u => u.Name == userName)).FindFirstOrDefault();

            if (loadedUser == null) return false;

            var hashedPassword = HashPassword(password, loadedUser.Salt);

            if (loadedUser.Password != hashedPassword.Value) return false;

            return true;
        }
        
        private void RaiseMembershipCreationError(string name, MembershipCreateStatus status)
        {
            var errorMessage = "Failed to create customer";
            switch (status)
            {
                case MembershipCreateStatus.DuplicateEmail:
                    errorMessage += ": duplicate email";
                    break;
                case MembershipCreateStatus.DuplicateUserName:
                    errorMessage += ": duplicate user name";
                    break;
                case MembershipCreateStatus.InvalidEmail:
                    errorMessage += ": invalid e-mail";
                    break;
                case MembershipCreateStatus.InvalidPassword:
                    errorMessage += ": invalid password";
                    break;
                case MembershipCreateStatus.InvalidQuestion:
                    errorMessage += ": invalid secret question";
                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    errorMessage += ": invalid secret answer";
                    break;
                case MembershipCreateStatus.InvalidUserName:
                    errorMessage += ": invalid user name";
                    break;
                case MembershipCreateStatus.UserRejected:
                    errorMessage += ": user rejected";
                    break;
            }
            var error = new Exception(errorMessage);
            error.Data["UserName"] = name;
            throw error;
        }

        private static string CreateSalt(int length)
        {
            var salt = new byte[length];
            var rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(salt);
            return Convert.ToBase64String(salt);
        }

        private static HashedPassword HashPassword(string password, string salt)
        {
            password += salt;
            var hasher = new SHA256CryptoServiceProvider();
            var value = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = hasher.ComputeHash(value);

            return new HashedPassword { Salt = salt, Value = Convert.ToBase64String(hash) };
        }

        private class HashedPassword
        {
            public string Salt { get; set; }
            public string Value { get; set; }
        }
    }

}