using Siege.Repository;
using Siege.Repository.UnitOfWork;

namespace Siege.Security.SQL.Repository
{
    public class SecurityDatabase : Database<SecurityDatabase>
    {
        public SecurityDatabase(IUnitOfWorkFactory<SecurityDatabase> unitOfWorkFactory, IUnitOfWorkStore unitOfWorkStore) : base(unitOfWorkFactory, unitOfWorkStore)
        {
        }
    }
}