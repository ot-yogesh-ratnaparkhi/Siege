namespace Siege.Repository.EntityFramework
{
    public class EntityFrameworkRepository<TDatabase> : Repository<TDatabase> where TDatabase : IDatabase
    {
        public EntityFrameworkRepository(EntityFrameworkUnitOfWorkManager unitOfWork) : base(unitOfWork)
        {
        }
    }
}