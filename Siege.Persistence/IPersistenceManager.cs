using NHibernate;

namespace Siege.Persistence
{
    public interface IPersistenceManager
    {
        ISessionFactory SessionFactory { get; }
    }
}
