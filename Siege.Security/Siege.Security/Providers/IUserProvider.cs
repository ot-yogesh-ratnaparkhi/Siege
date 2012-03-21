using System;

namespace Siege.Security.Providers
{
    public interface IUserProvider : ISecurityProvider<User, Guid?>
    {
        User FindByUserName(string userName);
    }
}