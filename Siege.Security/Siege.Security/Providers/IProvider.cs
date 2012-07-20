using Siege.Security.Entities;

namespace Siege.Security.Providers
{
    public interface IProvider<T> where T : SecurityEntity
    {
        void Delete(T item);
        T Save(T item);
        T Find(int? id);
    }
}