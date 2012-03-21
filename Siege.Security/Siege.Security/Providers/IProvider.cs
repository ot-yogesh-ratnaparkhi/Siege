using Siege.Security.Entities;

namespace Siege.Security.Providers
{
    public interface IProvider<T, TID> where T : SecurityEntity<TID>
    {
        void Delete(T item);
        T Save(T item);
        T Find(TID id);
    }
}