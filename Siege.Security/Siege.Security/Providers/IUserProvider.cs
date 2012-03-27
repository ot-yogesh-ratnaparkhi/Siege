using System;

namespace Siege.Security.Providers
{
    public interface IUserProvider : ISecurityProvider<User>
    {
        User FindByUserName(string userName);
    }
}