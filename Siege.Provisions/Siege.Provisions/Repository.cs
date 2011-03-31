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

namespace Siege.Provisions
{
    public class Repository<TPersistenceModule>  : IRepository<TPersistenceModule> where TPersistenceModule : IDatabase
    {
        protected readonly IUnitOfWorkManager unitOfWork;

        public Repository(IUnitOfWorkManager unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public T Get<T>(object id) where T : class 
        {
            return unitOfWork.For<TPersistenceModule>().Get<T>(id);
        }

        public void Save<T>(T item) where T : class
        {
            unitOfWork.For<TPersistenceModule>().Transact(() => unitOfWork.For<TPersistenceModule>().Save(item));
        }

        public void Delete<T>(T item) where T : class
        {
            unitOfWork.For<TPersistenceModule>().Transact(() => unitOfWork.For<TPersistenceModule>().Delete(item));
        }

        public void Transact(Action<IRepository<TPersistenceModule>> transactor)
        {
            unitOfWork.For<TPersistenceModule>().Transact(() => transactor(this));
        }
    }
}