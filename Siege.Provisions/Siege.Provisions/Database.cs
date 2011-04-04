using Siege.Provisions.UnitOfWork;

namespace Siege.Provisions
{
	public class Database<TDatabase> : IDatabase
		where TDatabase : IDatabase
	{
		private readonly IUnitOfWorkFactory<TDatabase> unitOfWorkFactory;
		private readonly IUnitOfWorkStore unitOfWorkStore;

		public Database(IUnitOfWorkFactory<TDatabase> unitOfWorkFactory,
								 IUnitOfWorkStore unitOfWorkStore)
		{
			this.unitOfWorkFactory = unitOfWorkFactory;
			this.unitOfWorkStore = unitOfWorkStore;
		}

		public IUnitOfWorkStore Store
		{
			get { return unitOfWorkStore; }
		}

		public IUnitOfWorkFactory Factory
		{
			get { return unitOfWorkFactory; }
		}
	}
}