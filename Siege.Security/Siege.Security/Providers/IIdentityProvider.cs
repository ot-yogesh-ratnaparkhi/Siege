namespace Siege.Security.Providers
{
    public interface IIdentityProvider
    {
        void Create(User user);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        bool UnlockUser(User user);
        int MinimumRequiredPasswordLength { get; }
        bool ValidateUser(string userName, string password);
    }
}