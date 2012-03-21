using System.Collections.Generic;

namespace Siege.Security.Providers
{
    public interface IPermissionProvider : IProvider<Permission, int?>
    {
        IList<Permission> All(bool includeHiddenPermissions);
    }
}