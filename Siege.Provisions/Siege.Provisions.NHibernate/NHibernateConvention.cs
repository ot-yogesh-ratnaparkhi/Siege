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
using NHibernate;
using Siege.Requisitions;
using Siege.Requisitions.Extensions.Conventions;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.RegistrationPolicies;

namespace Siege.Provisions.NHibernate
{
    public class NHibernateConvention<TUnitOfWorkStore, TPersistenceModule> : IConvention
        where TUnitOfWorkStore : IUnitOfWorkStore
        where TPersistenceModule : IDatabase
    {
        private readonly Action<IServiceLocator> registrations;

        public NHibernateConvention(string connectionStringName, ISessionFactory sessionFactory)
        {
            registrations =
                serviceLocator =>
                    {
                        AddCommonRegistrations(serviceLocator);
                        serviceLocator.Register(
                            Given<ISessionFactory>
                                .When<SessionContext>(c => c.ConnectionStringName == connectionStringName)
                                .Then(sessionFactory));
                    };
        }

        public NHibernateConvention(ISessionFactory sessionFactory)
        {
            registrations =
                serviceLocator =>
                    {
                        serviceLocator.Register(Given<ISessionFactory>.Then(sessionFactory));
                        AddCommonRegistrations(serviceLocator);
                    };
        }

        public Action<IServiceLocator> Build()
        {
            return registrations;
        }

        private static void AddCommonRegistrations(IServiceLocator serviceLocator)
        {
            serviceLocator
                .Register<Singleton>(Given<IUnitOfWorkFactory>.Then<NHibernateUnitOfWorkFactory>())
                .Register<Singleton>(Given<IDatabase>.Then<TPersistenceModule>())
                .Register(
                    Given<IUnitOfWork>.ConstructWith(
                        l => l.GetInstance<IUnitOfWorkManager>().For<TPersistenceModule>()))
                .Register<Singleton>(Given<IUnitOfWorkStore>.Then<TUnitOfWorkStore>())
                .Register(Given<IRepository<TPersistenceModule>>.Then<NHibernateRepository<TPersistenceModule>>());

            var manager = new NHibernateUnitOfWorkManager();
            manager.Add(serviceLocator.GetInstance<IDatabase>());
            serviceLocator.Register(Given<IUnitOfWorkManager>.Then(manager));
        }
    }
}