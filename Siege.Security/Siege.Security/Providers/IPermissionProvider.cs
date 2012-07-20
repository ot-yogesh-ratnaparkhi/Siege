using System.Collections.Generic;

namespace Siege.Security.Providers
{
    public interface IPermissionProvider : IProvider<Permission>
    {
        IList<Permission> All(bool includeHiddenPermissions);
        IList<Permission> ForApplication(Application application, bool includeHiddenPermissions);
    }
}