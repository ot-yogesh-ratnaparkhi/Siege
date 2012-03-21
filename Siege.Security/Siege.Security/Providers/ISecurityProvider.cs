using System.Collections.Generic;
using Siege.Security.Entities;

namespace Siege.Security.Providers
{
    public interface ISecurityProvider<T, TID> : IProvider<T, TID> where T : ApplicationBasedSecurityEntity<TID>
    {
        IList<T> GetForApplication(Application application);
    }
}