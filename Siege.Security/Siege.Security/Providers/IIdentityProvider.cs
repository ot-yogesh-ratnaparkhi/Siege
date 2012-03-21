using System.Collections;

namespace Siege.Security.Providers
{
    public interface IIdentityProvider
    {
        void Create(string userName, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey);
        string ResetPassword(string userName, string answer);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        IEnumerable GetAllUsers(int currentPage, int pageSize, out int total);
        bool UnlockUser(string userName);
        int MinimumRequiredPasswordLength { get; }
    }
}