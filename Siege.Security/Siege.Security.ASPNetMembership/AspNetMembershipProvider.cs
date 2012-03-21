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

            if(status != MembershipCreateStatus.Success) throw new Exception("Unable to create user.");
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
    }
}