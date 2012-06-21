/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System;
using System.Data.Entity;
using Siege.Repository.UnitOfWork;
using Siege.ServiceLocator;
using Siege.ServiceLocator.RegistrationPolicies;
using Siege.ServiceLocator.RegistrationSyntax;
using Siege.ServiceLocator.Registrations.Conventions;

namespace Siege.Repository.EntityFramework
{
	public class EntityFrameworkConvention<TUnitOfWorkStore, TDatabase, TDbContext> : IConvention
		where TUnitOfWorkStore : IUnitOfWorkStore
		where TDatabase : IDatabase
        where TDbContext : DbContext
	{
		public Action<IServiceLocator> Build()
		{
			return serviceLocator => serviceLocator
                .Register<Singleton>(Given<IUnitOfWorkFactory<TDatabase>>.Then<EntityFrameworkUnitOfWorkFactory<TDbContext, TDatabase>>())
				.Register(Given<IUnitOfWork>.ConstructWith(l => l.GetInstance<IUnitOfWorkManager>().For<TDatabase>()))
				.Register<Singleton>(Given<IUnitOfWorkStore>.Then<TUnitOfWorkStore>())
				.Register(Given<IRepository<TDatabase>>.Then<EntityFrameworkRepository<TDatabase>>())
				.Register(Given<TDatabase>.Then<TDatabase>())
                .Register(Given<TDbContext>.Then<TDbContext>())
                .Register<Singleton>(Given<IUnitOfWorkManager>.ConstructWith(locator => locator.GetInstance<EntityFrameworkUnitOfWorkManager>()))
                .Register(Given<EntityFrameworkUnitOfWorkManager>.InitializeWith(manager => manager.Add(serviceLocator.GetInstance<TDatabase>())))
                .Register<Singleton>(Given<EntityFrameworkUnitOfWorkManager>.Then<EntityFrameworkUnitOfWorkManager>());
		}
	}
}