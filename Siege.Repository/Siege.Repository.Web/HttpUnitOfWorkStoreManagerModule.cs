using System;
using System.Web;
using Siege.Repository.UnitOfWork;
using Siege.ServiceLocator;

namespace Siege.Repository.Web
{
	public class HttpUnitOfWorkStoreManagerModule : IHttpModule
	{
		private IServiceLocator serviceLocator;

		public void Init(HttpApplication context)
		{
			if (!(context is IServiceLocatorAccessor))
			{
				throw new HttpUnitOfWorkStoreManagerModuleException(
					"HttpApplication must inherit from IServiceLocatorAccessor when using HttpUnitOfWorkStoreManagerModule");
			}

			serviceLocator = ((IServiceLocatorAccessor)context).ServiceLocator;
			context.EndRequest += OnLeave;
		}

		private bool PersistenceFrameworkConfigured()
		{
			return serviceLocator.HasTypeRegistered(typeof(IUnitOfWorkStore));
		}

		private void OnLeave(object sender, EventArgs e)
		{
			if (PersistenceFrameworkConfigured())
			{
				var store = serviceLocator.GetInstance<IUnitOfWorkStore>();
				store.Dispose();
			}
		}

		public void Dispose()
		{
		}
	}
}
