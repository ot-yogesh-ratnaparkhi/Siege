namespace Siege.Repository.NHibernate
{
    public class NHibernateRepository<TDatabase> : Repository<TDatabase> where TDatabase : IDatabase
    {
        public NHibernateRepository(NHibernateUnitOfWorkManager unitOfWork) : base(unitOfWork)
        {
        }
    }
}