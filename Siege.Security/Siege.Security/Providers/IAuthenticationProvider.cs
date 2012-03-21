namespace Siege.Security.Providers
{
    public interface IAuthenticationProvider
    {
        bool Authenticate(string userName, string password, bool rememberMe);
        void Clear();
    }
}