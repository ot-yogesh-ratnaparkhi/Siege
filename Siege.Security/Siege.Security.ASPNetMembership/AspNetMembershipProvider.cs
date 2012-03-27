using System;
using System.Collections;
using System.Web.Security;
using Siege.Security.Providers;

namespace Siege.Security.ASPNetMembership
{
    public class AspNetMembershipProvider : IIdentityProvider, IAuthenticationProvider
    {
        private readonly MembershipProvider provider;

        public AspNetMembershipProvider(MembershipProvider provider)
        {
            this.provider = provider;
        }

        public void Create(string userName, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey)
        {
            MembershipCreateStatus status;
            provider.CreateUser(userName, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);

            if (status != MembershipCreateStatus.Success)
            {
                RaiseMembershipCreationError(userName, status);
            }
        }

        public string ResetPassword(string userName, string answer)
        {
            return provider.ResetPassword(userName, answer);
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return provider.ChangePassword(userName, oldPassword, newPassword);
        }

        public IEnumerable GetAllUsers(int currentPage, int pageSize, out int total)
        {
            return provider.GetAllUsers(currentPage, pageSize, out total);
        }

        public bool UnlockUser(string userName)
        {
            return provider.UnlockUser(userName);
        }

        public int MinimumRequiredPasswordLength
        {
            get
            {
                return provider.MinRequiredPasswordLength;
            }
        }

        public bool Authenticate(string userName, string password, bool rememberMe)
        {
            if(provider.ValidateUser(userName, password))
            {
                FormsAuthentication.SetAuthCookie(userName, rememberMe);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            FormsAuthentication.SignOut();
        }

        private void RaiseMembershipCreationError(string name, MembershipCreateStatus status)
        {
            var errorMessage = "Failed to create customer";
            switch (status)
            {
                case MembershipCreateStatus.DuplicateEmail:
                    errorMessage += ": duplicate email";
                    break;
                case MembershipCreateStatus.DuplicateProviderUserKey:
                    errorMessage += ": duplicate provider user key";
                    break;
                case MembershipCreateStatus.DuplicateUserName:
                    errorMessage += ": duplicate user name";
                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    errorMessage += ": invalid secret answer";
                    break;
                case MembershipCreateStatus.InvalidEmail:
                    errorMessage += ": invalid e-mail";
                    break;
                case MembershipCreateStatus.InvalidPassword:
                    errorMessage += ": invalid password";
                    break;
                case MembershipCreateStatus.InvalidProviderUserKey:
                    errorMessage += ": invalid provider user key";
                    break;
                case MembershipCreateStatus.InvalidQuestion:
                    errorMessage += ": invalid secret question";
                    break;
                case MembershipCreateStatus.InvalidUserName:
                    errorMessage += ": invalid user name";
                    break;
                case MembershipCreateStatus.ProviderError:
                    errorMessage += ": provider error";
                    break;
                case MembershipCreateStatus.UserRejected:
                    errorMessage += ": user rejected";
                    break;
            }
            var error = new Exception(errorMessage);
            error.Data["UserName"] = name;
            throw error;
        }
    }
}